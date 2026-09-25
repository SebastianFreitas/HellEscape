"""Asset audit for the VoidScape Unity project (no Unity needed, read-only).

Walks the asset dependency graph from what the game ships or opens and says
which files are used. Edges:
  - any GUID in an asset's YAML/JSON, or in its .meta (model material remaps,
    importer presets)
  - `#include "..."` paths in used shaders
  - C# type names: a used .cs pulls in every Assets/ .cs that defines a type
    it mentions (over-approximate on purpose; it only ever keeps more)
Roots:
  - MainLevel.unity (the only enabled build scene) and MainMenu.unity
    (disabled, but MainMenu/PauseMenu load scenes by name)
  - ProjectSettings/* (Graphics/Quality -> URP assets, Always Included
    Shaders, presets)
  - Assets/Scripts/**, Assets/Trial.cs (the game's own code)
  - TextMesh Pro/Resources (TMP loads its settings and default font by name)
Other Resources/ folders ship in a build but nothing loads from them by
string (no Resources.Load in the code), so they are reported separately as
"Resources-only" rather than counted as used.

Usage:
    python tools/assetaudit.py [--json out.json] [--depth N]
"""
import collections, json, os, re, struct, sys

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
ASSETS = os.path.join(REPO, "Assets")
GUID_META = re.compile(r"^guid: ([0-9a-f]{32})", re.M)
ANY_GUID = re.compile(r'guid[\\"]*:\s*[\\"]*([0-9a-f]{32})')
INCLUDE = re.compile(r'#include\s+"([^"]+)"')
TYPE_DEF = re.compile(r"\b(?:class|struct|enum|interface)\s+([A-Za-z_]\w*)")
IDENT = re.compile(r"[A-Za-z_]\w*")
TEX_EXT = {".png", ".jpg", ".jpeg", ".tga", ".tif", ".tiff", ".psd", ".exr", ".hdr", ".bmp"}


def rel(p):
    return os.path.relpath(p, REPO).replace("\\", "/")


def read_text(path):
    try:
        raw = open(path, "rb").read()
    except OSError:
        return None
    if not path.endswith(".meta") and b"\0" in raw[:8000]:
        return None
    return raw.decode("utf-8", "ignore")


def all_files():
    for dp, _, fn in os.walk(ASSETS):
        for f in fn:
            if not f.endswith(".meta"):
                yield os.path.join(dp, f)


def tex_dims(path):
    """(w, h) from the file header, or None."""
    ext = os.path.splitext(path)[1].lower()
    try:
        with open(path, "rb") as f:
            head = f.read(64)
        if ext == ".tga":
            return struct.unpack("<HH", head[12:16])
        if ext == ".psd":
            h, w = struct.unpack(">II", head[14:22])
            return w, h
        if ext == ".hdr":
            m = re.search(rb"\n-Y (\d+) \+X (\d+)", open(path, "rb").read(4096))
            return (int(m.group(2)), int(m.group(1))) if m else None
        from PIL import Image
        Image.MAX_IMAGE_PIXELS = None
        with Image.open(path) as im:
            return im.size
    except Exception:
        return None


def max_size(meta_text):
    """Default-platform maxTextureSize from a texture .meta."""
    m = re.search(r"platformSettings:\s*\n\s*- serializedVersion: \d+\s*\n\s*buildTarget: DefaultTexturePlatform\s*\n\s*maxTextureSize: (\d+)", meta_text or "")
    if not m:
        m = re.search(r"maxTextureSize: (\d+)", meta_text or "")
    return int(m.group(1)) if m else None


DOC = re.compile(r"^--- !u!(\d+) &(-?\d+)( stripped)?$", re.M)
FILE_ID = re.compile(r"\{fileID: (-?\d+)\}")
# Scripts that switch on their own children, so an inactive child is not dead.
CHILD_ACTIVATORS = {"ChanceToDesapear", "BridgeHandler"}


def dead_subtrees(scene, guid_to_file):
    """Inactive GameObjects in a scene that no component references (so no
    script can SetActive them) and whose parent doesn't switch children on.
    Returns [(name, fileID, text of every doc in the subtree)] and the text of
    the rest of the scene."""
    text = open(scene, encoding="utf-8", errors="ignore").read()
    heads = list(DOC.finditer(text))
    docs = {}
    for i, m in enumerate(heads):
        end = heads[i + 1].start() if i + 1 < len(heads) else len(text)
        docs[m.group(2)] = (m.group(1), text[m.start():end])
    go_of, father, active, name, inst_parent, comps = {}, {}, {}, {}, {}, collections.defaultdict(list)
    for fid, (cls, body) in docs.items():
        g = re.search(r"m_GameObject: \{fileID: (-?\d+)\}", body)
        if cls == "1" and "stripped" not in body.split("\n", 1)[0]:
            m = re.search(r"m_IsActive: (\d)", body)
            active[fid] = m and m.group(1) == "1"
            n = re.search(r"m_Name: (.*)", body)
            name[fid] = n.group(1) if n else "?"
        elif g:
            go_of[fid] = g.group(1)
            comps[g.group(1)].append(fid)
            f = re.search(r"m_Father: \{fileID: (-?\d+)\}", body)
            if f:
                father[fid] = f.group(1)
        if cls == "1001":
            inst_parent[fid] = re.search(r"m_TransformParent: \{fileID: (-?\d+)\}", body).group(1)
            root_off = re.search(r"propertyPath: m_IsActive\s+value: 0", body)
            n = re.search(r"propertyPath: m_Name\s+value: (.*)", body)
            name[fid] = n.group(1) if n else "?"
            active[fid] = not root_off  # approximation: any m_IsActive 0 override
    # Stripped GameObjects/Transforms belong to their PrefabInstance.
    owner = {}
    for fid, (cls, body) in docs.items():
        p = re.search(r"m_PrefabInstance: \{fileID: (-?\d+)\}", body)
        if p and p.group(1) != "0" and "stripped" in body.split("\n", 1)[0]:
            owner[fid] = p.group(1)
    # Who references each object (outside its own GameObject's docs)?
    refs = collections.Counter()
    for fid, (cls, body) in docs.items():
        if cls in ("1", "4", "224", "1001"):
            continue  # hierarchy bookkeeping, not script references
        for r in FILE_ID.findall(body):
            if r != fid and not (fid in go_of and go_of[fid] == r):
                refs[r] += 1

    def script_names(go):
        out = set()
        for c in comps.get(go, ()):
            m = re.search(r"m_Script: \{fileID: \d+, guid: ([0-9a-f]{32})", docs[c][1])
            if m and m.group(1) in guid_to_file:
                out.add(os.path.splitext(os.path.basename(guid_to_file[m.group(1)]))[0])
        return out

    def parent_go(node):
        """GameObject (or PrefabInstance) that parents node."""
        if node in docs and docs[node][0] == "1001":
            t = inst_parent[node]
        else:
            t = next((father[c] for c in comps.get(node, ()) if c in father), None)
        if not t or t == "0":
            return None
        return owner.get(go_of.get(t, t), go_of.get(t))

    children = collections.defaultdict(list)
    for node in list(active):
        p = parent_go(node)
        if p:
            children[p].append(node)

    def subtree(node):
        out, stack = [], [node]
        while stack:
            n = stack.pop(); out.append(n); stack += children.get(n, [])
        return out

    def own_ids(node):
        """node, its components, and for a PrefabInstance its stripped stubs
        plus the components added onto them."""
        stubs = {s for s, o in owner.items() if o == node}
        ids = {node} | stubs
        for n in {node} | stubs:
            ids |= set(comps.get(n, ()))
        return ids

    def referenced(node):
        return any(refs[i] for i in own_ids(node))

    dead = []
    for node, on in active.items():
        if on or referenced(node):
            continue
        p = parent_go(node)
        if p and script_names(p) & CHILD_ACTIVATORS:
            continue
        anc, alive = p, True
        while anc:
            if not active.get(anc, True) and not referenced(anc):
                alive = False  # an ancestor is already reported as dead
                break
            anc = parent_go(anc)
        if alive:
            dead.append(node)
    dead_ids = set()
    result = []
    for node in dead:
        ids = set()
        for n in subtree(node):
            ids |= own_ids(n)
        dead_ids |= ids
        result.append((name.get(node, "?"), node, "".join(docs[i][1] for i in ids if i in docs)))
    rest = "".join(b for f, (_, b) in docs.items() if f not in dead_ids)
    return result, rest


def main():
    out_json = sys.argv[sys.argv.index("--json") + 1] if "--json" in sys.argv else None

    files = list(all_files())
    guid_to_file = {}
    for p in files:
        t = read_text(p + ".meta")
        m = GUID_META.search(t or "")
        if m:
            guid_to_file[m.group(1)] = p

    # C# type index
    cs_files = [p for p in files if p.endswith(".cs")]
    type_to_file = collections.defaultdict(set)
    cs_idents = {}
    for p in cs_files:
        t = read_text(p) or ""
        t_nc = re.sub(r"//.*|/\*.*?\*/", "", t, flags=re.S)
        for name in TYPE_DEF.findall(t_nc):
            type_to_file[name].add(p)
        cs_idents[p] = set(IDENT.findall(t_nc))

    def edges(p):
        out = set()
        for q in (p, p + ".meta"):
            t = read_text(q)
            if not t:
                continue
            for g in ANY_GUID.findall(t):
                n = guid_to_file.get(g)
                if n and n != p:
                    out.add(n)
        ext = os.path.splitext(p)[1].lower()
        if ext in (".shader", ".cginc", ".hlsl", ".compute"):
            for inc in INCLUDE.findall(read_text(p) or ""):
                for cand in (os.path.join(os.path.dirname(p), inc), os.path.join(REPO, inc)):
                    cand = os.path.normpath(cand)
                    if os.path.isfile(cand):
                        out.add(cand)
        if ext == ".cs":
            for name in cs_idents.get(p, ()):
                for d in type_to_file.get(name, ()):
                    if d != p:
                        out.add(d)
        return out

    def walk(roots, seed_text=""):
        roots = list(roots) + [guid_to_file[g] for g in ANY_GUID.findall(seed_text) if g in guid_to_file]
        parent, stack, seen = {}, list(roots), set(roots)
        while stack:
            p = stack.pop()
            for n in edges(p):
                if n not in seen:
                    seen.add(n); parent[n] = p; stack.append(n)
        return seen, parent

    scenes = [os.path.join(ASSETS, "Base", "Scenes", s) for s in ("MainLevel.unity", "MainMenu.unity")]
    # EditorBuildSettings lists every scene, disabled ones too (SampleScene);
    # the scenes that matter are roots on their own.
    ps = [os.path.join(REPO, "ProjectSettings", f) for f in os.listdir(os.path.join(REPO, "ProjectSettings"))
          if f != "EditorBuildSettings.asset"]
    own_code = [p for p in cs_files if rel(p).startswith("Assets/Scripts/")] + [os.path.join(ASSETS, "Trial.cs")]
    tmp_res = [p for p in files if "/TextMesh Pro/Resources/" in rel(p)]
    other_res = [p for p in files if "/Resources/" in rel(p) and p not in tmp_res]

    used, parent = walk(scenes + ps + own_code + tmp_res)
    by_root = {
        "MainLevel": walk([scenes[0]])[0],
        "MainMenu": walk([scenes[1]])[0],
        "settings": walk(ps)[0],
        "code": walk(own_code)[0],
    }
    res_only = walk(other_res)[0] - used

    size = {p: os.path.getsize(p) for p in files}

    # Hidden scene objects: what only they pull in ships but is never seen.
    dead, rest_text = [], ""
    for s in scenes:
        d, rest = dead_subtrees(s, guid_to_file)
        dead += [(os.path.basename(s), *x) for x in d]
        rest_text += rest
    live = walk(ps + own_code + tmp_res, rest_text)[0]
    hidden = []
    for scene, nm, fid, txt in dead:
        only = walk([], txt)[0] - live
        if only:
            hidden.append({"scene": scene, "object": nm, "fileID": fid,
                           "bytes": sum(size[p] for p in only), "files": sorted(rel(p) for p in only)})
    hidden.sort(key=lambda h: -h["bytes"])
    status = {}
    for p in files:
        if p in used:
            status[p] = "used"
        elif p in res_only:
            status[p] = "resources-only"
        else:
            status[p] = "unused"
    only_menu = {p for p in used if p in by_root["MainMenu"] and not any(
        p in by_root[k] for k in ("MainLevel", "settings", "code")) and "/TextMesh Pro/Resources/" not in rel(p)}

    # Rollup by folder, to --depth levels under Assets/ (deeper for the big bins).
    depth = int(sys.argv[sys.argv.index("--depth") + 1]) if "--depth" in sys.argv else 3
    roll = collections.defaultdict(lambda: collections.Counter())
    for p in files:
        parts = rel(p).split("/")[:-1]
        for d in range(2, min(len(parts), depth + 1) + 1):
            k = "/".join(parts[:d])
            roll[k]["total"] += size[p]
            roll[k][status[p]] += size[p]
            roll[k]["n_" + status[p]] += 1

    tex = []
    for p in used:
        if os.path.splitext(p)[1].lower() in TEX_EXT:
            dims = tex_dims(p)
            if dims and max(dims) >= 4096:
                ms = max_size(read_text(p + ".meta"))
                tex.append({"path": rel(p), "w": dims[0], "h": dims[1], "maxTextureSize": ms,
                            "bytes": size[p], "via": rel(parent[p]) if p in parent else None})

    total = sum(size.values())
    by = collections.Counter()
    for p in files:
        by[status[p]] += size[p]
    mb = lambda b: f"{b / 2**20:,.0f} MB"
    print(f"Assets/: {len(files)} files, {mb(total)}  " + "  ".join(f"{k}: {mb(v)}" for k, v in by.items()))
    print(f"used only via MainMenu: {len(only_menu)} files, {mb(sum(size[p] for p in only_menu))}")
    print(f"used textures >= 4096 px: {len(tex)}, {mb(sum(t['bytes'] for t in tex))}")
    hb = sum(size[p] for p in used - live - set(scenes))
    print(f"used only by hidden, never-activated scene objects: {mb(hb)} in {len(hidden)} objects")
    for h in hidden[:15]:
        print(f"  {mb(h['bytes']):>9}  {h['scene']}: {h['object']} ({len(h['files'])} files)")

    if out_json:
        json.dump({
            "status": {rel(p): status[p] for p in files},
            "size": {rel(p): size[p] for p in files},
            "via": {rel(p): rel(parent[p]) for p in used if p in parent},
            "only_menu": sorted(rel(p) for p in only_menu),
            "rollup": {k: dict(v) for k, v in roll.items()},
            "big_textures": tex,
            "hidden": hidden,
            "live": sorted(rel(p) for p in live),
        }, open(out_json, "w"), indent=1)


if __name__ == "__main__":
    main()
