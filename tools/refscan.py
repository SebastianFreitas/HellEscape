"""Missing-reference scan for the VoidScape Unity project (no Unity needed).

Resolves every GUID referenced from Unity YAML/JSON assets (and importer .meta
files) to: a project asset, a package asset (exact versions from
Packages/packages-lock.json), a built-in resource, or UNRESOLVED. Then walks
the dependency graph from the enabled build scenes, MainMenu, every Resources/
folder and ProjectSettings to say which unresolved GUIDs the game can reach.

Usage:
    python tools/refscan.py <cache_dir> [--json out.json]

<cache_dir> holds downloaded package tarballs; keep it outside the repo
(the session scratchpad). First run downloads ~270 MB of packages.

Phase 5 gate: the unresolved count must not grow after a prune batch.
Baseline (2026-09-25): 63 unresolved, 22 reachable (12 via MainLevel),
all explained in docs/recovery.md.
"""
import collections, json, os, re, sys, tarfile, urllib.request

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
EDITOR_VERSION = re.search(r"m_EditorVersion: (\S+)",
    open(os.path.join(REPO, "ProjectSettings", "ProjectVersion.txt")).read()).group(1)
BUILTIN_PKGS = rf"C:\Program Files\Unity\Hub\Editor\{EDITOR_VERSION}\Editor\Data\Resources\PackageManager\BuiltInPackages"
REGISTRY = "https://download.packages.unity.com"

GUID_META = re.compile(r"^guid: ([0-9a-f]{32})", re.M)
# Matches YAML `fileID: N, guid: G` and escaped JSON inside shader graphs.
REF = re.compile(r'fileID[\\"]*:\s*(-?\d+),\s*[\\"]*guid[\\"]*:\s*[\\"]*([0-9a-f]{32})')
ANY_GUID = re.compile(r'guid[\\"]*:\s*[\\"]*([0-9a-f]{32})')
SCRIPT = re.compile(r"m_Script: \{fileID: (-?\d+), guid: ([0-9a-f]{32})")
SRC_EXT = {".unity", ".prefab", ".mat", ".asset", ".controller", ".overrideController", ".anim",
    ".vfx", ".vfxoperator", ".vfxblock", ".shadergraph", ".shadersubgraph", ".physicMaterial",
    ".physicsMaterial2D", ".mask", ".playable", ".signal", ".lighting", ".spriteatlas", ".guiskin",
    ".fontsettings", ".flare", ".renderTexture", ".cubemap", ".mixer", ".preset", ".terrainlayer",
    ".brush", ".meta", ".asmdef", ".giparams"}
# GUIDs that are known not to matter. Keep this list short and explained.
KNOWN_HARMLESS = {
    # VFX Graph 10.x sub-output for a pipeline that is not installed; Unity's own
    # VFX templates carry the same dangling reference.
    "081ffb0090424ba4cb05370a42ead6b9": "VFX Graph SRP sub-output stub (Unity's templates have it too)",
}


def read_text(path):
    raw = open(path, "rb").read()
    if not path.endswith(".meta") and b"\0" in raw[:8000]:
        return None  # binary asset
    return raw.decode("utf-8", "ignore")


def metas(root):
    out = {}
    for dp, _, fn in os.walk(root):
        for f in fn:
            if f.endswith(".meta"):
                p = os.path.join(dp, f)
                m = GUID_META.search(open(p, encoding="utf-8", errors="ignore").read(4000))
                if m:
                    out[m.group(1)] = p[:-5]
    return out


def package_guids(cache):
    lock = json.load(open(os.path.join(REPO, "Packages", "packages-lock.json")))["dependencies"]
    guids = {}
    os.makedirs(cache, exist_ok=True)
    for name, info in lock.items():
        if info["source"] != "registry":
            continue
        key = f"{name}-{info['version']}"
        dest = os.path.join(cache, key)
        if not os.path.isdir(dest):
            tgz = dest + ".tgz"
            if not os.path.exists(tgz):
                print("downloading", key, file=sys.stderr)
                urllib.request.urlretrieve(f"{REGISTRY}/{name}/-/{key}.tgz", tgz)
            with tarfile.open(tgz) as t:
                t.extractall(dest, filter="data")
        for g in metas(dest):
            guids[g] = key
    if os.path.isdir(BUILTIN_PKGS):
        for d in os.listdir(BUILTIN_PKGS):
            for g in metas(os.path.join(BUILTIN_PKGS, d)):
                guids[g] = d
    else:
        print("warning: built-in packages not found at", BUILTIN_PKGS, file=sys.stderr)
    return guids


def main():
    if len(sys.argv) < 2:
        sys.exit(__doc__)
    cache = sys.argv[1]
    out_json = sys.argv[sys.argv.index("--json") + 1] if "--json" in sys.argv else None

    proj = metas(os.path.join(REPO, "Assets"))
    pkg = package_guids(cache)

    def classify(g):
        if g in proj: return "project"
        if g in pkg: return "package"
        if g.startswith("0000000000000000"): return "builtin"
        return "unresolved"

    refs = collections.defaultdict(lambda: collections.defaultdict(set))
    scripts = collections.defaultdict(set)
    for top in ("Assets", "ProjectSettings", "Packages"):
        for dp, _, fn in os.walk(os.path.join(REPO, top)):
            for f in fn:
                if os.path.splitext(f)[1] not in SRC_EXT:
                    continue
                p = os.path.join(dp, f)
                t = read_text(p)
                if t is None:
                    continue
                rel = os.path.relpath(p, REPO).replace("\\", "/")
                for fid, g in REF.findall(t):
                    refs[g][rel].add(fid)
                for _, g in SCRIPT.findall(t):
                    scripts[g].add(rel)

    unresolved = {g for g in refs if classify(g) == "unresolved"}

    # Reachability from what ships or opens: enabled/disabled build scenes,
    # Resources/ folders and ProjectSettings.
    roots = [os.path.join(REPO, "ProjectSettings", f) for f in os.listdir(os.path.join(REPO, "ProjectSettings"))]
    for dp, _, fn in os.walk(os.path.join(REPO, "Assets")):
        if "Resources" in dp.split(os.sep):
            roots += [os.path.join(dp, f) for f in fn if not f.endswith(".meta")]
    reach = {}
    for root in roots:
        seen, parent, stack = set(), {}, [root]
        while stack:
            p = stack.pop()
            for q in (p, p + ".meta"):
                if not os.path.isfile(q):
                    continue
                t = read_text(q)
                if t is None:
                    continue
                for g in ANY_GUID.findall(t):
                    if g in unresolved and g not in reach:
                        chain = [p]
                        while chain[-1] in parent and len(chain) < 8:
                            chain.append(parent[chain[-1]])
                        reach[g] = [os.path.relpath(c, REPO).replace("\\", "/") for c in chain]
                    n = proj.get(g)
                    if n and n not in seen:
                        seen.add(n); parent[n] = p; stack.append(n)

    summary = collections.Counter(classify(g) for g in refs)
    print(f"GUIDs referenced: {len(refs)}  " + "  ".join(f"{k}: {v}" for k, v in sorted(summary.items())))
    print(f"unresolved reachable from build scenes/Resources/settings: {len(reach)}")
    for g in sorted(reach, key=lambda g: reach[g][-1]):
        note = f"  [{KNOWN_HARMLESS[g]}]" if g in KNOWN_HARMLESS else ""
        print(f"  {g}  via " + " <- ".join(reach[g]) + note)
    missing_scripts = {g: sorted(s) for g, s in scripts.items() if classify(g) == "unresolved"}
    print(f"missing scripts (m_Script with no .cs): {len(missing_scripts)}")
    for g, s in missing_scripts.items():
        print(f"  {g}  in {len(s)} file(s), e.g. {s[0]}")

    if out_json:
        json.dump({
            "summary": summary,
            "unresolved": {g: {s: sorted(f) for s, f in refs[g].items()} for g in unresolved},
            "reachable": reach,
            "missing_scripts": missing_scripts,
        }, open(out_json, "w"), indent=1)


if __name__ == "__main__":
    main()
