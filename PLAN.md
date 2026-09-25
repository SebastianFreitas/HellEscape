# VoidScape: revival plan

VoidScape is a first-person sci-fi roguelite shooter. It was built by hand
in Unity 2020.3 between Oct 2020 and Jul 2022, over 715 commits. Its
GitHub repo is called **HellEscape**.

The goals, in order:
1. Recover everything that exists.
2. Bring it to Unity 6.3 LTS.
3. Make it a first-class Claude Code project.
4. Fix the bugs.
5. Decide what the game can become.

> An earlier plan for this was written against the wrong repo
> (`Documents/HellScape`, a different and much smaller 2020.1 project).
> Ignore it. This file replaces it.

---

## How to use this file

**One phase = one fresh Claude session.** Start each one with:

```
Read PLAN.md at the repo root and do Phase <N>. Start with the phase's
research pass, show me a short plan for the phase, wait for my OK, then
do it. Finish by filling in the phase's Log and ticking it in Progress.
```

- **Directions, not recipes.** Each phase says what to reach and what to
  watch out for. The session works out the how, and it researches first
  whenever the phase says so or whenever a version, tool or step here
  might be out of date.
  - Everything below was true on **2026-09-25**.
  - Unity's tooling for AI agents changes monthly.
- **One phase at a time.** Never start a phase while an earlier one is
  still open.
  - From Phase 9 onward, every phase ends with the project opening on
    6.3, compiling with zero errors, and playable.
- **If a phase is too big for one session,** it writes a handoff (see
  Phase 7) and the next session continues it. It does not skip ahead.
- **The owner is away from the PC.** Claude drives Unity itself (open,
  play, click, screenshot, read the console) and never asks the owner to
  play, click or watch. The owner's jobs are decisions, logins, pushes
  and anything outward-facing. Leave the Editor open; Claude closes and
  reopens it when a step needs batch mode.
  - Up to Phase 9 (Unity 2020.3), Claude uses the local-only bridge
    built in Phase 2 (`.claude-bridge/`, see `docs/baseline.md`).
  - From Phase 9 on, Unity's official Claude Code plugin and the Unity
    CLI (Unity 6+ only).
- **If a phase finds that this plan is wrong,** it fixes the plan (the
  phase text, the snapshot, the order) and says so in its Log.

**Rules for every session until Phase 7 creates CLAUDE.md:**
- Never hand-edit `.unity`, `.prefab`, `.asset`, `.mat` or `.meta` YAML.
  Reading it is fine.
- Never touch `Library/`, `Temp/`, `Logs/` or `UserSettings/`.
- Never create, move or rename an asset without its `.meta` file.
- No destructive git commands: no `reset --hard`, `clean`, force push,
  history rewrite or branch deletion unless the phase says so and the
  owner has said yes in chat.
- Stage by path, never `git add -A`.
- **Commits and pushes while this plan runs:**
  - Claude commits its own work without asking, by path, on the phase's
    branch (or `main` when the phase has none). It commits at least once,
    at the end of the phase, and more often when a phase has natural
    steps.
  - **Claude never pushes.** At the end of a phase it gives the owner the
    push command in its own `bash` block, and the owner runs it.
- Keep command output short: pipe it through `tail`.
- Scratch files and screenshots go in the session scratchpad, never in
  the repo.

---

## Progress

| # | Phase | Status |
|---|-------|--------|
| 0 | Owner setup (you, no Claude) | [ ] |
| **A** | **Save what exists** | |
| 1 | Hunt for missing files | [x] |
| 2 | Baseline: see the game as it was | [x] (cut short, see Log) |
| 3 | Safety net | [x] (tag push is the owner's) |
| **B** | **Repo** | |
| 4 | Asset audit | [x] |
| 5 | Prune | [ ] |
| 6 | Repo home and Git LFS | [ ] |
| **C** | **Claude environment (thin layer, before the migration)** | |
| 7 | Claude foundations (port the Portfolio setup) | [ ] |
| 8 | Knowledge map | [ ] |
| **D** | **Migration to Unity 6.3 LTS** | |
| 9 | Open in 6.3 and upgrade packages | [ ] |
| 10 | Zero compile errors | [ ] |
| 11 | Rendering: URP, shaders, VFX, lighting | [ ] |
| 12 | Parity play-test and the first 6.3 build | [ ] |
| **E** | **Full Claude tooling** | |
| 13 | Editor bridge | [ ] |
| 14 | Verify loop | [ ] |
| 15 | Test harness and dev cheats | [ ] |
| **F** | **Bugs** | |
| 16 | Bug audit | [ ] |
| 17 | Fix: crashes, null refs, softlocks | [ ] |
| 18 | Fix: player, camera, interaction | [ ] |
| 19 | Fix: guns, projectiles, status effects, boons | [ ] |
| 20 | Fix: enemies and bosses | [ ] |
| 21 | Fix: level generation, rooms, portals | [ ] |
| 22 | Fix: hub, missions, mirror, shop, crafting, inventory | [ ] |
| 23 | Fix: the Path (bridge and trial) | [ ] |
| 24 | Fix: UI, menus, pause, settings | [ ] |
| **G** | **Modernize** | |
| 25 | Save system | [ ] |
| 26 | Input System | [ ] |
| 27 | Code health | [ ] |
| 28 | Performance | [ ] |
| 29 | Builds and distribution | [ ] |
| **H** | **What's possible** | |
| 30 | Design doc and roadmap | [ ] |

---

## Where the project stands (snapshot, 2026-09-25)

**Engine and settings**
- **Batch mode does not work in 2020.3 with the Personal licence**
  (credentials error). Only the Hub-opened GUI Editor works.
- Unity **2020.3.35f1**, an LTS release. **URP 10.9** is already in use
  (`Assets/Base/Settings/UniversalRP-*.asset`), so there is no render
  pipeline switch to make, only a big URP version jump.
- **Input:** the legacy Input Manager only. There are about 26 `Input.*`
  calls in 10 files, with hard-coded key strings ("e", "q", "i", "tab",
  "1" to "4").
- **Serialization:** Force Text. Scenes and prefabs are readable YAML.
- **Packages:**
  - `render-pipelines.universal` 10.9.0
  - `visualeffectgraph` 10.9.0
  - `probuilder` 4.5.2
  - `progrids` (deprecated preview)
  - `textmeshpro` 3.0.6
  - `timeline` 1.4.8
  - `test-framework` 1.1.31
  - `collab-proxy` (dead)
  - `ide.vscode` (deprecated)
  - `ide.rider`
  - `ide.visualstudio`

**Code**
- **Totals:** 188 `.cs` files. 139 of them (**about 14,750 lines**) are
  yours: `Assets/Scripts/**` plus a stray `Assets/Trial.cs`. The rest are
  pack or template scripts, mostly TextMesh Pro examples.
- **Biggest files:**
  - `VoidBoon` 991 lines
  - `RoomGenerator` 652
  - `Monster` 606
  - `ModData` 571
  - `GunGenerator` 505
  - `PlayerBasicMovement` 490
  - `CraftingDevice` 442
  - `RoomActivator` 424
- **Structure:**
  - No asmdefs, no namespaces, no tests.
  - About 390 public fields.
  - 154 `StartCoroutine` calls, mostly string-based.
  - 17 `Find`/`FindObjectOfType` calls, several of them
    `FindGameObjectsWithTag("Dude")[0]`.
  - A 231-line `Update` in `player/InteractBehaviour.cs`.
  - About 300 commented-out code lines.
  - `LevelGen/LevelBuilder.cs` is entirely commented out.
- **Saves:** PlayerPrefs only, 98 calls in 19 files (for example
  `PathLevel` and the inventory).
- **Known build blocker:** `Scripts/Light/PrefabMaper.cs:2` has
  `using UnityEditor;` outside `#if UNITY_EDITOR`.

**The game, as the code reads**
- **Hub:**
  - A Hades-style Mirror of permanent upgrades.
  - A Shop and a SoundDevice.
  - A Mission Selector that rolls missions with mods and prices, entered
    through a Portal.
- **Runs:**
  - Procedurally generated rooms (`RoomGenerator`, `RoomActivator`).
  - Room types: encounter, elite, boss, weapon, heal, crafting bench and
    exit.
  - Red/Blue "influence".
- **Boons:** `VoidBoon`, with about 40 boons (cold, poison, fire,
  physical, crit, dash, HP) and rerolls.
- **Loot guns:** built from affix tables (`ModData`, `GunGenerator`) with
  rarity.
  - Crafting uses "gunParts" as currency: add or remove mods, disassemble
    guns.
  - Grenades.
- **Enemies:**
  - `Monster` is the base class.
  - Cone, Skull, SpikeSkull and green Triangle enemies, plus a ghost mob.
  - Bosses: ConeBoss, SkullBoss and TowerBoss (with phases and an
    explosion).
- **Traps:** lava, spikes, lasers.
- **The Path / Bridge / Trial:** an endless-style mode.
  - A `Trial` costs `10 + PathLevel` gunParts, and is free after a win.
  - `BridgeHandler` spawns bridge pieces with encounters.
- **Scenes:**
  - `Assets/Base/Scenes/MainLevel.unity` (1.2 MB) is the only scene
    enabled in Build Settings. Hub and runs appear to live in it, switched
    by `GameMan`.
  - `MainMenu.unity` exists but is disabled.
  - Packs bring 51 demo scenes.

**Assets and repo**
- **`Assets/` is about 5.4 GB:**
  - Third-party packs take 3.4 GB. The Cobble Games Spaceship pack alone
    is **2.4 GB**, and its 64 MB TGAs are near GitHub's 100 MB limit.
  - **Phase 4 found:** only 762 MB is visible in the game. 2.5 GB ships
    but is only used by inactive objects in MainLevel (the Space Station
    and two F3 ships), and 2.2 GB is unused. See `docs/assets.md`.
  - `Materials/NotReduced` is 804 MB.
  - `Prefabs/rest/Props` is 572 MB.
  - `SciFi Warehouse Kit` is 482 MB.
- **Git:** there is **no LFS** and no `.gitattributes`. The pack is
  **2.93 GiB**, with 4,847 tracked files. There is one branch, `main`,
  and the remote is `github.com/SebastianFreitas/HellEscape`.
- **Claude:** none yet. No CLAUDE.md, no `.claude/`. `.sln` and `.csproj`
  files aren't generated yet because there is no `Library/`.

**Missing files (settled by Phase 1)**
- **This repo is the only source.** The owner's decision: every other
  folder and zip on the PC is ignored. If something turns out to be
  missing, it gets dealt with later.
- **The scan:** `tools/refscan.py` finds **63 unresolved GUIDs**. All of
  them are unused or cosmetic, and **nothing the game needs is missing**.
  Details are in `docs/recovery.md`.
- **No old playable build exists.**
  - `Downloads/buildWeb.zip` is another project ("UnamedWebGame",
    2021.3).
  - `Downloads/voidscape.html` is the Portfolio page about the game.

---

## Phase 0: Owner setup (you, no Claude)

Already done: Git LFS, Unity 6.3, and Visual Studio Community 2026
(installed with Unity).

Still to do or check:
1. **Unity Hub:** sign in with a Personal licence. Batch mode uses the
   licence the Hub activated, so Claude needs this too.
2. **Unity 6.3: check it is the latest `6000.3.x` patch.**
   - Why 6.3: it is LTS, supported until Dec 2027.
   - 6.7 LTS is due around Q4 2026. The hop to it later should be small.
   - Ignore Unity 7 for now.
3. **Visual Studio:** open the Visual Studio Installer and make sure the
   **"Game development with Unity"** workload is ticked.
   - Rider (free for non-commercial use) is a fine alternative. You need
     only one IDE.
4. **Recommended: Unity 2020.3.35f1** from the Unity archive (the
   `unityhub://` link on the download archive page). It is the exact
   version the game was made in, and Phase 2 uses it to see the game as it
   was and to get Unity's own list of missing references. It is about
   3 GB of disk; you can uninstall it after Phase 12.
5. **Find your old machines and drives.** If VoidScape was ever on
   another PC, laptop, external drive, OneDrive, Google Drive or a USB
   stick, have it plugged in or synced for Phase 1.
6. **For Phase 13 onward** (you can do these later):
   - `uv` (`winget install astral-sh.uv`)
   - the .NET SDK (LTS)
   - Unity CLI (beta). Phase 13 confirms the install command.
7. **Disk space:** 5.4 GB of assets turns into a `Library/` of 10 GB or
   more per open project. Keep at least 40 GB free.

Don't install Unity AI or the Unity AI Assistant subscription, and don't
install anything from a pack that ships `.exe` files.

---

# Part A: Save what exists

## Phase 1: Hunt for missing files

**Goal:** know for certain whether anything the game needs is missing
from git, and recover it. This phase is read-only on the repo until the
last step.

**Research first:**
- How Unity resolves GUIDs: `.meta` files, package GUIDs, built-in
  `0000000000000000e000000000000000`/`f000...` IDs, and `fileID`s inside
  a file.
- How to tell a missing package asset from a missing project asset
  without opening Unity. For example, compare against the GUIDs inside
  the exact package versions in `Packages/packages-lock.json`, which can
  be fetched from Unity's package registry as tarballs into the
  scratchpad.

**Directions**
1. **Build a proper missing-reference report.**
   - Go through every GUID referenced from scenes, prefabs, materials,
     assets, controllers, anims, VFX, shader graphs and
     ScriptableObjects.
   - Resolve each one to: a project asset, a package asset (from the
     locked package versions), built-in, or **unresolved**.
   - Also flag scripts (`m_Script`) that point at a GUID with no `.cs`.
     Those become "Missing Script" in Unity.
2. **Diff the recovery candidates against the repo.** Work in the
   scratchpad, and never unzip into the repo.
   - `Downloads/Voidscape.zip`: list it first, then extract to the
     scratchpad.
   - `Downloads/HellEscape-main/`.
   - Anything else the owner points to, such as old drives, cloud
     folders, `buildWeb.zip` and `voidscape.html`.
   - For each one, find files that exist there but not in git. Pay
     particular attention to `.meta` GUIDs that match the unresolved
     list.
3. **Ask the owner** about anything still unresolved: which PC it was
   made on, and whether an old backup exists. Search the disk by
   filename and by GUID in `.meta` files, with a sensible scope and a
   timeout.
4. **Write `docs/recovery.md`:**
   - what was missing
   - where each file was found
   - what is still missing, and which scene or prefab it breaks
   - what the old build (if any) shows
5. **Bring recovered files in on a `recovery` branch.**
   - Copy each file together with its original `.meta` (the GUID must
     match), then commit by path.
   - Don't merge the branch until Phase 3 has made the backup.

**Done when:** every unresolved GUID is either recovered, explained as a
package or built-in asset, or listed in `docs/recovery.md` as truly lost,
with its impact.

**Log:**
- 2026-09-25, done.
- **Scope changed by the owner:** this repo is the only source.
  - Steps 2, 3 and 5 (diff the candidates, search the disk, make a
    `recovery` branch) were dropped.
  - Nothing was recovered, so no branch was made.
- **Wrong facts, now fixed in the snapshot:**
  - `buildWeb.zip` is another project, and `voidscape.html` is the
    Portfolio page. No old build exists.
  - The 154 GUIDs from the quick scan came down to **63** once the exact
    package versions were counted.
- **Added `tools/refscan.py`.** It resolves GUIDs against the project,
  the locked packages (downloaded from the registry into a cache dir) and
  the built-ins, then walks reachability from the build settings and
  `Resources/`. Phase 5 uses it as its gate.
- **Found:**
  - 12 unresolved GUIDs are reachable from MainLevel.
    - 3 are cosmetic: a white UI Image in `CanvasInventory`, untextured
      transparent materials, and one material missing in
      `PathGen/pieces/old/Path.prefab`.
    - 4 are USP pistol secondary maps.
    - The rest have no effect: fields that are never read, and importer
      remaps.
  - 5 GUIDs were assets deleted on purpose, still recoverable from git
    history.
  - 51 are pack, demo or trash content.
- **Also checked (Phase 0):**
  - Installed: Unity 2020.3.35f1, 2021.3.14f1 and 6000.3.25f1.
  - The VS 2026 Unity workload is installed.
  - Git LFS is 3.6.1, and 102 GB of disk is free.

---

## Phase 2: Baseline: see the game as it was

**Goal:** a record of how the game looked and played before anything
changes. Every later phase compares against it. **Claude plays it
itself** through the local bridge; the owner is not needed.

**Directions**
1. Open the project in 2020.3.35f1 (from the Hub; batch mode can't be
   licensed). Read `Editor.log` and cross-check it against
   `docs/recovery.md`.
2. Drive the game through the bridge: the main menu, the hub (Mirror,
   Shop, Mission Selector, Portal), a full run (rooms, an elite, a boss,
   a boon pick, a weapon drop, crafting), the Path/Bridge/Trial, death
   and restart, the inventory and pause. Note whether each one works,
   looks broken or throws errors, with screenshots.
3. Check the three open questions at the end of `docs/recovery.md`.
4. Write `docs/baseline.md`: controls, flow, what works, console errors,
   FPS, and every PlayerPrefs key.
5. At the end: delete `Assets/Editor/ClaudeBridge/` (plus its `.meta`
   and `Assets/Editor.meta`) and the `.git/info/exclude` lines, but keep
   `.claude-bridge/saves/` until Phase 3 has moved it off-repo.

**Done when:** `docs/baseline.md` covers every flow above.

**Log:**
- 2026-09-25 (session 1, stopped at about 125k tokens for a handoff):
  - **Plan changed:** the owner won't play. Claude drives Unity itself,
    and Phase 2 builds a local bridge for 2020.3, because no official
    tool supports 2020.3 (the Unity plugin needs Unity 6+, and unity-mcp
    needs 2021.3+).
  - **Found:**
    - 2020.3 batch mode can't use the Personal licence.
    - The old build's saves survive in the registry.
    - `MainMenuManager`'s `DeleteAll` is commented out (earlier notes
      said it ran).
    - Bug candidates are listed in `docs/baseline.md`.
- 2026-09-25 (session 2, **closed early by the owner's call**):
  - Recorded weapon drops, the slots HUD, pause, death and restart, and
    the Path from its code. Answered recovery questions 1 and 2 (neither
    asset is ever shown). Question 3 moves to Phase 12.
  - **Not played:** a boon pick, crafting, the shop, elites, the
    TowerBoss and the Path. The 2020.3 bridge was too slow and fragile
    (teleports, aim snapping, health 100000). The owner chose to
    convert to 6.3 first and build real play/test tools there, and only
    then fix bugs and refactor. That was already the plan's order, so
    only Phase 12 changed.
  - **Step 5, mostly done:** the auto-mode guard blocked deleting the
    bridge, so it was **moved** out of `Assets/` into
    `.claude-bridge/removed-from-assets/` instead (untracked, nothing
    lost). Two things are still open:
    - ~~Trim `.git/info/exclude` to just `/.claude-bridge/`.~~ Done in
      Phase 3.
    - **Next session:** confirm the Editor recompiled with zero errors
      after the move (Phase 3 doesn't need the Editor, so this can wait
      until Phase 4).

---

## Phase 3: Safety net

**Goal:** make it impossible to lose the original.

**Directions**
- **An off-repo backup:** a full copy of the working folder (without
  `Library/`) in a dated zip, somewhere outside `Documents/VoidScape`.
- **In git:**
  - Tag `v0-unity2020.3`. Phase 1 recovered nothing, so there is no
    `recovery` branch to merge.
  - Push the tag to the current remote: **ask the owner first**, since
    it is outward-facing.
- **Record in `docs/recovery.md`** where the backups live.

**Done when:** the tag is on GitHub, the zip exists, and a fresh clone of
the tag equals the working project.

**Log:**
- 2026-09-25, done except the push (the owner pushes).
  - The zip, the old saves and the tag are listed in
    `docs/recovery.md` under Backups.
  - A fresh clone of the tag matches the working project.
  - **Owner:** push the tag (the command is in the session's last
    message). Until then the tag exists only on this PC, next to the
    zip.
  - Also closed from Phase 2: `.git/info/exclude` is trimmed to
    `/.claude-bridge/`, so `Assets/Editor/` is tracked again.

---

# Part B: Repo

## Phase 4: Asset audit

**Goal:** know which of the 5.4 GB the game actually uses. This phase is
read-only and produces a keep/drop list.

**Research first:**
- **Unity dependency rules.** An asset is used if it is reachable from:
  - a build scene
  - a `Resources/` folder (anything under `Resources` ships and can be
    loaded by string)
  - a prefab referenced by a used asset
  - a ScriptableObject
  - the URP or quality settings
  - `Always Included Shaders`
- `Resources.Load` or `Shader.Find` with string names in the code, which
  the GUID graph can't see.

**Directions**
1. **Build the reachability graph** from `MainLevel.unity`,
   `MainMenu.unity` and every `Resources/` folder, and add the string
   loads found in the code.
2. **Classify every top-level folder and pack** as used, partly used or
   unused, with sizes. Name the handful of assets each partly-used pack
   actually supplies.
   - Expect the Cobble Games Spaceship (2.4 GB), the TMP Examples, the
     URP template `TutorialInfo`, the demo scenes and many prop packs to
     be mostly unused. Prove it.
3. **Check texture sizes** in what *is* used. `Materials/NotReduced` is
   804 MB. Flag 4K and 8K textures used on small props.
4. **Write `docs/assets.md`:** the keep/drop table, its sizes, and the
   expected repo size after pruning.
5. **Flag risky drops,** meaning anything referenced only from disabled
   scenes, commented-out code or prefabs that nothing spawns. The owner
   decides those.

**Done when:** `docs/assets.md` gives a clear drop list, and the owner
has approved it line by line for the risky ones.

**Log:**
- 2026-09-25, done.
  - **The owner's answers:**
    - R1 and R2: drop them.
    - R3: keep them.
    - Nothing gets thrown away. Every dropped file goes into a local zip
      in `/Archive/`, which git ignores (see Phase 5).
  - **New tool:** `tools/assetaudit.py`. It is read-only, needs no Unity
    and runs in 5 seconds. The results are in `docs/assets.md`.
  - **The split:** of 5,473 MB, 762 MB is used, 2,546 MB is hidden and
    2,165 MB is unused. The pruned `Assets/` would be about 3.3 GB after
    the unused drops, or about 0.77 GB with R1 and R2 as well.
  - **Plan corrected:** the Cobble Games Spaceship is **not** unused.
    MainLevel holds an inactive `Space Station` instance that nothing
    ever activates. Phase 5 now removes such instances in the Editor
    first (see its directions).
  - **Closed from Phase 2:** after the bridge was moved out, the Editor
    was refreshed with Ctrl+R. It recompiled with zero errors.
  - **For Phase 6:** add `dae`, `bmp` and `aiff` to the LFS list.

---

## Phase 5: Prune

**Goal:** remove what Phase 4 approved, without breaking a single
reference.

**Directions**
- **Work on a `prune` branch.** Delete assets together with their `.meta`
  files (in Unity 2020.3 if it is installed, or on disk with the Editor
  closed).
- **Archive before deleting (the owner's rule):** zip every file you drop,
  with its `.meta` and its path under `Assets/`, into
  `Archive/pruned-assets.zip` at the repo root. Add each batch to the
  same zip. `/Archive/` is in `.gitignore`, so the zip stays on this PC
  only. It sits outside `Assets/`, so Unity never imports it. Check the
  zip (`7z t`) before deleting anything.
- **The list is `docs/assets.md`,** plus the owner's answers on R1 to
  R3 in Phase 4's Log.
- **Hidden ships first (R1, R2, if approved):** delete the `Space
  Station`, `F3_Green Variant` and `F3_Grey Variant` instances from
  MainLevel **in the Editor**, save the scene, and only then delete their
  packs. Scene YAML must not be hand-edited. The Phase 2 bridge is in
  `.claude-bridge/removed-from-assets/`; copy it back into `Assets/`
  for this step, then move it out again.
- **Delete in batches** (one pack per commit), re-running the Phase 1
  reference scan after each batch. Zero new unresolved GUIDs is the gate.
  Also re-run `tools/assetaudit.py`: "used" must stay at 762 MB.
- **Also remove:**
  - the pack demo scenes
  - the TMP "Examples & Extras"
  - the URP template readme (`Assets/Base/TutorialInfo`)
  - `SampleScene` and `example.unity`, if Phase 4 confirmed they are
    unused
- **Leave the code alone**, even dead code like `LevelBuilder.cs`.
  Phase 27 handles it, once there is a bridge and tests.
- **Downsizing textures** (max-size import settings) is a Phase 28
  job. Don't do it here.
- **Claude plays MainLevel for a minute** in 2020.3 through the Phase 2
  bridge (a run start and one encounter) to confirm nothing went pink or
  missing.

**Done when:** the pruned project has zero new unresolved references, you
have played it (if 2020.3 is available), and `docs/assets.md` records
the final size.

**Log:**

---

## Phase 6: Repo home and Git LFS

**Goal:** a repo that can live on GitHub for years: LFS for binaries,
Unity-aware merging, and a clean ignore list.

**Research first:**
- **GitHub's current Git LFS quotas and pricing** for free accounts
  (storage and bandwidth). Every clone and CI run pulls LFS bandwidth.
- **The options for the 2.93 GiB of history:**
  - **A: a fresh repo.** Start it from the pruned snapshot, and archive
    `HellEscape` read-only, which keeps the full history there.
  - **B: `git lfs migrate import --everything`.** This rewrites history
    and pushes every old binary version into LFS, so it could blow the
    quota.
  - **C: LFS for new files only.**
- **GitHub's current Unity `.gitignore` template.** Also Unity Smart
  Merge (`UnityYAMLMerge`) and how to wire it as a git merge driver.

**Directions**
- **Present A, B and C with numbers** (sizes and quota impact) and
  recommend one. **Option A is the likely winner:** the old history is
  still on GitHub, and the new repo starts small and clean.
- **The owner decides:**
  - the option
  - the repo name (VoidScape or HellEscape?)
  - public or private
- **Add `.gitattributes`:**
  - LFS for fbx, obj, blend, png, jpg, tga, tif, psd, exr, hdr, wav,
    mp3, ogg, mp4, ttf, otf, pdf, zip, and any other binaries the audit
    found
  - `unityyamlmerge` for `.unity`, `.prefab`, `.asset`, `.mat`,
    `.anim`, `.controller`
  - `text eol=lf` or `auto` for `.cs` and `.meta`, whichever research
    says is least noisy on Windows
- **Refresh `.gitignore`** from the template, then add:
  - `.claude/worktrees/`
  - `.claude/handoff.md`
  - `Logs/`, `UserSettings/`
  - `/Archive/` (Phase 5's zip of pruned assets; keep this line)
  - `Builds/`
  - the Claude verify/screenshot output folder (Phase 14)
- **Doing the push:** Claude prepares everything and gives the exact
  commands; **the owner runs the push** (and the history rewrite, if
  they chose option B).
- **Record the decision in `docs/recovery.md`.**

**Done when:**
- the new remote holds the pruned project with its binaries in LFS
- a fresh clone plus `git lfs pull` gives a working project
- `git lfs ls-files` looks right

**Log:**

---

# Part C: Claude environment (thin layer, before the migration)

The migration is exactly when you want Claude reading Unity's logs and
fixing errors. So the rules, hooks and map come first. The Editor bridge
(Phase 13) comes after, because the good tools need Unity 6.

## Phase 7: Claude foundations (port the Portfolio setup)

**Goal:** bring over as much of the Portfolio Claude environment as makes
sense, adapted to Unity.

**Source:** `C:/Users/Traff/Desktop/sebas/Portfolio/CLAUDE.md` and
`C:/Users/Traff/Desktop/sebas/Portfolio/.claude/`. Read them first.

**Research first:**
- The current Claude Code docs for memory (CLAUDE.md, `.claude/rules/`
  with `paths:` frontmatter), settings and permissions, hooks (events,
  matchers, exit code 2, `permissionDecision`), subagents (frontmatter,
  `omitClaudeMd`, `isolation: worktree`), skills and auto-compaction.
- What Unity-specific CLAUDE.md files and hooks people use now, and any
  Unity guidance in Unity's own agent plugin (it may already ship rules
  worth copying).

**Port nearly as-is (generic pieces)**
- **`hooks/run.sh`:** the Python launcher.
- **`hooks/git-guard.py`:** blocks blanket git commands (`add -A`/`.`,
  `commit -a`, `stash`, `checkout --`, `restore`, `reset --hard`,
  `clean`, `rebase`, force push), and keeps `main` and origin
  owner-only.
  - Also block `git lfs migrate`, `git filter-repo` and anything that
    deletes `.meta` files in bulk.
- **`hooks/stop-guard.py`:** no uncommitted work left at the end of a
  turn.
- **`hooks/context-watch.py`:** context lines per agent with a hard deny
  for subagents, plus the `CLAUDE_CODE_AUTO_COMPACT_WINDOW` env var.
- **The `handoff` skill,** including auto-continue.
- **The `implementer` agent** (sonnet, `omitClaudeMd`, spec-driven,
  60k-token context line). Rewrite its "Project conventions" section for
  Unity C#:
  - Keep `.meta` files with their assets.
  - Never touch scene or prefab YAML.
  - Verify with the compile check.
- **CLAUDE.md sections:**
  - "One prompt, one finished result" and the report format
  - "Main session role"
  - "Delegation"
  - "Spec format"
  - "Context budget"
  - "Token rules"
  - "Git and the owner's commands" (PowerShell-safe commands in `bash`
    blocks). Write in the commit rule for **after the plan is finished**:
    - Claude doesn't commit on its own any more.
    - Its **first answer** after the work is done includes a ready-to-run
      commit command in a `bash` block (staged by path, with the message
      filled in), so the owner can press Run.
    - The push command gets its own block. Pushing is always the owner's.
    - While the plan is still running, the automatic-commit rule at the
      top of `PLAN.md` applies instead.

**Adapt**
- **Session modes.** In Unity, a git worktree is a separate project: it
  needs its own multi-GB `Library/` import and its own Editor, and only
  one Editor can hold a project open.
  - The **default mode is the main checkout**, with the Editor open and
    Claude working on a phase branch.
  - Keep a **code-only worktree** mode for pure C# work that is checked
    in batch mode, if research says that is practical at this project
    size.
  - The **cloud** mode can only read and review code, since there is no
    Unity licence or GPU there. Keep it minimal or drop it.
  - Update `session-start.py` to match.
- **Report format:**
  - **Try** becomes "open Unity, press Play, do X".
  - **How it looks** becomes a Game-view screenshot from Phase 14 on.
    Until then, "none yet".
- **`implementer-wt`:** probably drop it (see modes). Explain the choice.
- **`try.py`:** this doesn't carry over (it relies on a sibling worktree
  plus serving). Replace it with a documented owner merge step.

**Drop:** the web-only pieces: `snap.py`, `nav-flows`, `jscheck`,
`gframes`, `bump.py`, `serve.py` and `launch.json`, and the art-style
and instruments rules.

**New for Unity**
- **Root `CLAUDE.md`:**
  - the Unity version and Editor path
  - how to read `Editor.log` (`%LOCALAPPDATA%/Unity/Editor/Editor.log`)
  - the batch-mode compile check, and its catch: it refuses to run while
    the Editor has the project open
  - the hard YAML, `.meta` and `Library` rules from the top of this file
- **`.claude/settings.json` permissions:**
  - Deny edits to `Library/**`, `Temp/**`, `Logs/**`, `UserSettings/**`
    and `**/*.meta`.
  - Allow read-only git and search commands, and the Unity batch
    command.
- **A PreToolUse guard** that blocks Write and Edit on `*.unity`,
  `*.prefab`, `*.mat`, `*.asset` and `*.meta`. Phase 13 replaces this
  with the bridge's tools.

**Done when:** a fresh session reads CLAUDE.md and the modes, can explain
the project and its rules, and the hooks demonstrably block a
`git add -A` and an edit to a `.meta` file.

**Log:**

---

## Phase 8: Knowledge map

**Goal:** Claude never has to read 14,750 lines to find something. This
is the Unity equivalent of the Portfolio's `MAP.md`.

**Directions**
- **`.claude/MAP.md`** (grep-only, never read whole):
  - **Per system** (player, guns and projectiles, boons, crafting and
    inventory, enemies and bosses, level gen and rooms, hub and missions
    and mirror, path/bridge/trial, UI, game management, lights): its
    files, what each one does, key public methods, and which prefabs or
    scene objects use it.
  - The **list of files over 300 lines**.
  - **Tags and layers**, and what uses each (the "Dude", "Inventory",
    "Slot", "Mirror" tags, and so on).
  - **Every PlayerPrefs key**, with who reads it and who writes it.
  - **Scenes and key prefabs** (Player, RunGen, PathGen, enemies).
  - The **static data tables** (`ModData`, `ModDataRoom`) and the
    inheritance quirks (`GameMan` and `MissionSelector` extend
    `ModDataRoom`).
  - The **string coroutine names**, since renaming a method silently
    breaks them.
- **`.claude/rules/*.md`:** path-scoped rules for the systems with
  traps. Examples: string coroutines, and boons that must stay in sync
  with the Mirror enum. Only write rules where a real trap exists.
- **Use parallel `Explore` subagents,** one per system, then merge their
  findings. Keep MAP rows short.
- **Cross-check against `docs/baseline.md`:** every feature seen in play
  should map to code.

**Done when:** for five random questions ("where is crit calculated?",
"what spawns the TowerBoss?", ...), a fresh session answers from MAP.md
plus at most two file reads.

**Log:**

---

# Part D: Migration to Unity 6.3 LTS

Fix only what the upgrade breaks. Gameplay bugs wait for Part F, and the
input and save rewrites wait for Part G. The migration diff must stay
about the migration.

## Phase 9: Open in 6.3 and upgrade packages

**Research first:**
- **Unity's upgrade guides** for 2021.3, 2022.3, 6.0 and each 6.x up to
  6.3, and the 6.3 release notes.
- **URP 10 to URP 17:** Render Graph, whether Compatibility Mode still
  exists in 6.3, renderer features, and changes to the URP asset.
- **VFX Graph 10 to 17.**
- **ProBuilder 4 to 6:** and what replaces ProGrids (built-in grid
  snapping).
- **TextMesh Pro** being merged into `ugui` 2.x.
- **Jump size:** does Unity advise one major version at a time, and does
  a direct jump from 2020.3 LTS work in practice?

**Directions**
1. **Set up:** create an `upgrade/unity-6.3` branch. Close every Editor
   and delete `Library/`.
2. **Try the direct jump** from 2020.3 to 6.3 first. If the console is a
   wall of errors nobody can explain, reset the branch and step through
   instead: 2021.3, then 2022.3, then 6.3.
3. **Open with the Hub and accept the API Updater.** Commit immediately,
   untouched, so the diff shows exactly what Unity rewrote.
4. **Packages:**
   - Remove `collab-proxy`, `ide.vscode` and `progrids`.
   - Update `ide.visualstudio` and `ide.rider`.
   - Upgrade URP, VFX Graph, ProBuilder, Timeline and Test Framework to
     their 6.3 versions.
   - Accept the TMP-to-ugui upgrade and the TMP essential resources
     upgrade.
   - Commit each package step separately.
5. **Explain the API Updater diff** in the Log.
6. **Install Unity's official Claude Code plugin and the Unity CLI now**
   (the minimal part of Phase 13), so Phases 10 to 12 are checked by
   Claude without the owner. Phase 13 finishes the setup.

**Done when:** the project opens in 6.3 (errors are allowed at this
point), the packages are at their 6.3 versions, and every step is its own
commit.

**Log:**

---

## Phase 10: Zero compile errors

**Directions**
- **Work from `Editor.log`,** or from the batch-mode compile with the
  Editor closed. Fix compile errors in our scripts first, then in the
  pack scripts.
  - If a pack script is hopeless and nothing uses it (see
    `docs/assets.md`), delete it.
  - Otherwise, fix it or re-download the pack from My Assets.
- **Fix the known build blocker:** guard `PrefabMaper.cs`'s
  `UnityEditor` use. Audit every runtime script for other unguarded
  Editor APIs.
- **Triage warnings:** fix the deprecation warnings that are a line or
  two. Record the rest in the Log for Part G (the legacy input warning
  stays).
- **Specs to `implementer`** for anything more than trivial, as
  CLAUDE.md says.

**Done when:** the Editor opens with zero compile errors, the console has
no errors on load, and there are no "missing script" entries beyond those
`docs/recovery.md` lists as lost.

**Log:**

---

## Phase 11: Rendering: URP, shaders, VFX, lighting

**Research first:**
- URP 17 material upgrade paths (the Render Pipeline Converter).
- Shader Graph versioning.
- Post-processing Volumes.
- Lighting changes since 2020.3: the Progressive Lightmapper,
  Enlighten's status and ambient baking.
- Adaptive Probe Volumes: an option, not a requirement.

**Directions**
- **Find every pink or broken material,** in the used assets first.
  Convert or fix them. Pack materials made for the Built-in pipeline need
  the converter.
- **Check the custom Shader Graphs,** every VFX Graph (the fire,
  explosion and blood FX) and `PostProcessUpdater.cs`'s Volume
  overrides.
- **URP assets and quality levels:** check the High, Medium and Low URP
  assets, the renderer data, and any renderer features.
- **Open MainLevel (and MainMenu) and re-bake lighting.** Compare the
  look against `docs/baseline.md`'s screenshots.

**Done when:** MainLevel looks like the baseline (or better), with no
pink materials and no rendering errors.

**Log:**

---

## Phase 12: Parity play-test and the first 6.3 build

**Directions**
- **Replay the `docs/baseline.md` checklist on 6.3,** with Claude
  driving it through the official Unity plugin/CLI.
  Every difference is either an upgrade regression, which gets fixed
  here, or a pre-existing bug, which gets logged for Phase 16.
- **Smoke-check the flows Phase 2 never played** (a boon pick,
  crafting, the shop, an elite, the TowerBoss, the Path), plus recovery
  question 3 (SkullBoss and TriangleGreen materials). Compare against
  the code, since no 2020.3 record exists. Keep it to "does it run and
  look right"; deep testing waits for Phases 13 to 15.
- **Produce a Windows build** with the Editor closed. Research the
  6.3 build command-line (Build Profiles changed how builds are set up).
  Launch the build and play for a minute.
- **Update CLAUDE.md** with what changed: versions, gotchas found and the
  build command.
- **Merge `upgrade/unity-6.3`** into `main`. That is the owner's step.
  Then tag `v1-unity6.3`.

**Done when:**
- the game plays like the baseline on 6.3
- the Windows build runs
- the branch is merged and tagged

**Log:**

---

# Part E: Full Claude tooling

## Phase 13: Editor bridge

**Goal:** Claude can talk to the running Editor: compile, read the
console, enter Play Mode, take screenshots, and inspect or edit scenes
safely.

**Research first** (everything here moves fast, so check it that week):
- **Unity CLI plus the `com.unity.pipeline` package** (official, free,
  beta, Unity 6+).
  - `unity recompile`, `unity command` (Editor commands such as Play
    Mode, tests and screenshots), `unity eval`, and `unity test` /
    `unity build` with the Editor closed.
  - Unity positions the CLI as cheaper in tokens than MCP for coding
    agents.
- **Unity's official Claude Code plugin**
  (`Unity-Technologies/unity-agent-plugin`, Sep 2026): skills that drive
  the CLI.
- **CoplayDev/unity-mcp** ("MCP for Unity"): MIT licence, needs `uv`.
  - It covers the console, compile, Roslyn validation, tests, play
    control, scene/GameObject/prefab editing and screenshots.
  - It is the mature fallback.
- **The `csharp-lsp` plugin:** go-to-definition and diagnostics. It has
  had Windows issues and may not work in subagents, so it is nice to
  have.
- **Skip:** the deprecated or paid Unity AI Assistant MCP.

**Directions**
- **Pick one primary bridge.** Install it project-scoped (a plugin or
  `.mcp.json` in the repo, not personal config). Document its gotchas:
  domain reloads drop connections, and Unity may not recompile until its
  window has focus.
- **Replace the Phase 7 YAML-edit guard's advice:** scenes and prefabs
  change only through the bridge's tools, or through throwaway Editor
  scripts deleted afterwards.
- **Update the `implementer` agent:** can it use the bridge? Research
  whether MCP and plugins reach subagents. If they don't, the main
  session runs the verification.

**Done when:** from a Claude session, you can ask for "recompile, read
the console, enter Play Mode for 5 s, screenshot the Game view", and it
works.

**Log:**

---

## Phase 14: Verify loop

**Goal:** "done" means Claude checked it itself, the way `snap.py` and
`nav-flows` work in the Portfolio.

**Directions**
- **Build a `verify` skill:**
  1. Recompile.
  2. Confirm zero console errors.
  3. Run the EditMode tests (Phase 15 fills them).
  4. Optionally, Play Mode for N seconds, or run a named scenario.
  5. Take Game-view screenshots saved outside the repo.
  6. Output a short pass/fail summary.
- **Add screenshot compare** if cheap, meaning a before/after pair on
  the same camera. Research whether the bridge can place the camera
  deterministically.
- **Add a PostToolUse hook** on `.cs` edits that reminds Claude to
  recompile or verify.
- **Update CLAUDE.md:** the verify loop becomes step 4 of every task, and
  **How it looks** in the report uses its screenshots.

**Done when:** you ask "make the pistol fire 20% faster", and Claude
changes it, verifies it with test results and a screenshot, and you only
look at the result.

**Log:**

---

## Phase 15: Test harness and dev cheats

**Directions**
- **Assembly definitions.** Research the trade-off for a 139-script
  project with no namespaces and heavy cross-references.
  - The likely setup: one `VoidScape` runtime asmdef, one Editor asmdef,
    and `Tests/EditMode` and `Tests/PlayMode` asmdefs.
  - Pack scripts either get their own asmdefs or stay in Assembly-CSharp,
    whichever compiles.
- **Smoke tests:**
  - MainLevel loads.
  - The player exists.
  - A shot damages a Monster.
  - A room closes its doors on entry and opens them after the encounter.
  - GunGenerator makes a valid gun for each rarity.
  - ModData tables are consistent.
  - The Mission Selector rolls valid missions.
- **Dev cheats** (editor or development builds only), so Claude can
  reach any system fast:
  - teleport to the hub, a room type or the Path
  - spawn any enemy or boss
  - grant a boon, gun or gunParts
  - god mode
  - set PathLevel
- **Put the harness in the verify skill:** "verify scenario X" should
  mean a cheat sequence plus a screenshot.

**Done when:** the tests run green from the bridge, and each major system
can be reached in under 10 seconds of Play Mode.

**Log:**

---

# Part F: Bugs

## Phase 16: Bug audit

**Goal:** one prioritized, reproducible bug list. Nothing is fixed in
this phase.

**Directions**
- **Static audit,** with parallel `Explore` agents per system from
  MAP.md, looking for:
  - null references after `Destroy`
  - `Find(...)[0]` on empty arrays
  - string coroutines naming methods that don't exist
  - floating-point `==`
  - state that is never reset between runs
  - PlayerPrefs key typos
  - `Update` doing work every frame that belongs to events
  - duplicate listeners
  - `Time.timeScale` leaks (the pause menu)
  - boons whose effects stack wrongly
- **Play audit,** with the verify loop and cheats, going through every
  flow in `docs/baseline.md`.
- **Mine the git log:** commits like "fixed bridge mob bug maybe" and
  "its all fcuked with the selector" point at fragile code. Check those
  areas.
- **Write `docs/bugs.md`.** Each bug gets an ID, system, severity
  (crash, softlock, wrong, cosmetic), steps to reproduce, suspected
  `file:line`, and which Phase 17 to 24 it belongs to.
- **Owner review:** you mark anything that is actually a feature, and
  add bugs you remember.

**Done when:** `docs/bugs.md` exists, and you have reviewed and
reprioritized it.

**Log:**

---

## Phases 17 to 24: Fix batches

Every fix phase works the same way:
1. Take the bugs for that system from `docs/bugs.md`.
2. Where it's practical, write a failing test first (EditMode or
   PlayMode); otherwise write a verify scenario.
3. Send a spec to `implementer`.
4. Run the verify loop.
5. Commit, one bug per commit, with the bug ID in the message.
6. Mark the bug fixed in `docs/bugs.md`.

- **No refactoring beyond what the fix needs.** Phase 27 does the
  refactoring. **No balance changes** unless the bug *is* balance.
- **A phase can split into a/b/c** if the list is long; add rows to
  Progress.
- **Each fix phase first re-reads its MAP.md section and path rules,**
  and researches any Unity API it is unsure of in 6.3.

### Phase 17: Crashes, null refs, softlocks
Anything that throws, freezes, traps the player or loses progress, in any
system. This goes first because it hides other bugs.

### Phase 18: Player, camera, interaction
- **Scope:** `PlayerBasicMovement` (jump, dash, sprint), `MouseLook`,
  `PlayerHpManager` (death and respawn), `InteractBehaviour`.
- **`InteractBehaviour`'s 231-line tag chain:** fix its bugs only; the
  restructure waits for Phase 27.
- **Research:** CharacterController versus Rigidbody behaviour changes
  in 6.3 physics, if movement feels different from the baseline.

### Phase 19: Guns, projectiles, status effects, boons
- **Scope:**
  - `Gun`, `PlayerProjectile` (fire, cold, poison, bleed)
  - `GrenadeHolder`
  - `GunGenerator` and `ModData` rolls
  - the `VoidBoon` stacking and math
  - boon rerolls
  - crit
- Write tests for the damage math.

### Phase 20: Enemies and bosses
- **Scope:**
  - the `Monster` base class and every enemy type
  - the `Spawner`
  - line of sight
  - NavMesh use (research the NavMesh changes since 2020: the
    AI Navigation package and NavMeshSurface)
  - the TowerBoss phases
  - the SkullBoss and ConeBoss
  - enemy projectiles
  - traps (lava, spikes, laser)

### Phase 21: Level generation, rooms, portals
- **Scope:**
  - `RoomGenerator` (overlaps, unreachable rooms, seeds)
  - `RoomActivator` (door lock and unlock, `IsEncounterDone`)
  - each room type
  - the influence rooms
  - the portal
  - the exit room
- **Consider adding a seed** so bugs can be reproduced.

### Phase 22: Hub, missions, mirror, shop, crafting, inventory
- **Scope:**
  - `MissionSelector` and its prices
  - `MirrorManager` upgrades
  - the Shop
  - `CraftingDevice` (add or remove mods, disassemble)
  - `Inventory` and `Slot`
  - item duplication ("fixed weapon dup" suggests it has happened
    before)
  - the gunParts economy

### Phase 23: The Path (bridge and trial)
- **Scope:**
  - `BridgeHandler` spawning and healing
  - `PathFloor` and `PathCombat` encounters
  - PathLevel difficulty scaling
  - `Trial` costs and the free-after-win rule
  - "bridge mob bug maybe"

### Phase 24: UI, menus, pause, settings
- **Scope:**
  - `MainMenuManager` and whether the disabled `MainMenu.unity` scene
    should come back
  - `PauseMenu` (timeScale, cursor lock)
  - HUD and HP bars
  - settings persistence
  - UI scaling at different resolutions
  - TMP text that broke during the migration

---

# Part G: Modernize

Each phase here is **behaviour-preserving**: the tests and the verify
scenarios from Parts E and F must stay green.

## Phase 25: Save system
- **Research:** Unity 6 save practices (JSON in
  `Application.persistentDataPath`, versioned save files, atomic writes).
- **Replace the 98 PlayerPrefs calls** with one `SaveSystem` that has a
  versioned data model.
- **Write a one-time importer** from the old PlayerPrefs keys (the list
  from Phase 2 and MAP.md), so existing progress survives.
- **Keep settings apart:** audio and mouse sensitivity can stay in
  PlayerPrefs, or move with the rest. Decide and document it.

## Phase 26: Input System
- **Research:**
  - the current Input System package
  - the status of the legacy Input Manager in 6.3 (marked for
    deprecation)
  - Input Actions assets
  - rebinding UI samples
- **Set up Input Actions** for every current control (the hard-coded
  keys are listed in the snapshot) plus a gamepad scheme.
- **Port the 10 input files** to the actions.
- **Add a rebind menu** if you want one.
- **Switch Active Input Handling** to the new system only once nothing
  legacy is left.

## Phase 27: Code health
- **Remove dead code:** `LevelBuilder.cs`, commented-out blocks,
  `testNav.cs`, `Assets/Trial.cs` (move it into `Scripts/`), debug logs.
- **Replace `Find`/tag lookups** with serialized references or a small
  service locator.
- **Replace string `StartCoroutine`** calls with method-reference ones.
- **Split `InteractBehaviour`** into interactable components (an
  `IInteractable` interface).
- **Split `VoidBoon`** (991 lines) by damage type or boon family.
- **Add namespaces**, and fix the misspellings in names *only* where it
  is safe: a serialized field rename needs `[FormerlySerializedAs]`, and
  a coroutine rename needs every caller updated.
- **Do it in small, test-backed commits.** Keep MAP.md in sync in the
  same commits.

## Phase 28: Performance
- **Research:** the Unity 6 Profiler, Frame Debugger, GPU Resident
  Drawer, and texture import and compression settings.
- **Profile** the hub, a busy room, a boss fight and the Path.
- **Fix the obvious costs:**
  - GC allocations in `Update`
  - `GetComponent` in hot paths
  - `Instantiate`/`Destroy` churn: pool projectiles and enemies
  - real-time lights
  - oversized textures (the 804 MB `NotReduced` folder)
- **Set a target FPS** and record before/after numbers in the Log.

## Phase 29: Builds and distribution
- **Windows build from one command,** by Claude with the Editor closed,
  producing a versioned output folder that is gitignored.
- **Optional WebGL build** for the Portfolio, which already has a
  VoidScape page (`voidscape.html`) but no playable build.
  - **Research:** WebGL size limits, compression, and whether
    `Portfolio` on GitHub Pages can host the build (file-size limits,
    LFS doesn't work on Pages).
  - Then coordinate with the Portfolio repo's own rules (its CLAUDE.md)
    rather than editing it from here.
- **Optional itch.io page:** research `butler` uploads. **Publishing is
  the owner's click.**

---

# Part H: What's possible

## Phase 30: Design doc and roadmap
- **Write `docs/design.md`:**
  - what VoidScape is: the core loop across hub, run and Path
  - its fantasy, its controls and its progression
  - what sets it apart
  - what was half-finished in 2022, taken from the git log and the dead
    code
- **Take stock:** list the unfinished and cut content found during
  Parts A to G (disabled scenes, unused prefabs, commented-out
  features).
- **Propose a roadmap** in slices, each with a size: new enemies or
  bosses, more room types, boon synergies, a meta-progression pass, a
  story or lore layer, audio, controller support, a Steam page. The owner
  picks.
- **Engine hop:** check whether 6.7 LTS has shipped. If it has, plan
  the hop as its own small phase.

**Done when:** you agree with the design doc, and the next slices are
added to this file as new phases.

---

## Sources behind the claims above (re-check each phase)

**Unity versions and upgrades**
- https://unity.com/releases/unity-6/support
- https://endoflife.date/unity
- https://docs.unity3d.com/6000.3/Documentation/Manual/UpgradeGuideUnity6.html
- https://docs.unity3d.com/6000.3/Documentation/Manual/upgrade-project.html
- The Unity download archive: https://unity.com/releases/editor/archive

**Rendering and input**
- https://unity.com/topics/render-pipelines-strategy-for-2026
- https://docs.unity3d.com/6000.3/Documentation/Manual/InputLegacy.html

**Agent tooling**
- https://unity.com/blog/meet-the-unity-cli
- https://docs.unity.com/en-us/unity-cli/unity-cli-reference
- https://github.com/Unity-Technologies/unity-agent-plugin
- https://github.com/CoplayDev/unity-mcp
- https://claude.com/plugins/csharp-lsp
- https://docs.unity3d.com/Packages/com.unity.test-framework@latest

**Claude Code**
- https://code.claude.com/docs (memory, settings, hooks, subagents, MCP,
  skills, plugins)

**Git**
- https://github.com/github/gitignore/blob/main/Unity.gitignore
- https://docs.unity3d.com/6000.3/Documentation/Manual/SmartMerge.html
- https://docs.github.com/en/repositories/working-with-files/managing-large-files

**Your own Claude environment to port:**
- `C:/Users/Traff/Desktop/sebas/Portfolio/CLAUDE.md`
- `C:/Users/Traff/Desktop/sebas/Portfolio/.claude/`
