# Asset audit (Phase 4)

Which of the 5.4 GB in `Assets/` the game uses, and what Phase 5 may delete.
Made on 2026-09-25 with `tools/assetaudit.py` (read-only, no Unity needed).
Re-run it after every prune batch:

```
python tools/assetaudit.py --json <scratchpad>/audit.json
```

## How "used" is decided

An asset is **used** if the game can reach it from:
- `MainLevel.unity` (the only enabled build scene)
- `MainMenu.unity` (disabled in Build Settings, but kept as a root)
- `ProjectSettings/` (Graphics and Quality lead to the URP assets and
  Always Included Shaders; `EditorBuildSettings` is left out, because it
  also lists the disabled `SampleScene`)
- the game's own code (`Assets/Scripts/**`, `Assets/Trial.cs`)
- `TextMesh Pro/Resources` (TMP loads its settings and default font by name)

It follows every GUID in a file and in its `.meta`, `#include` paths in
shaders, and C# type names (a used script keeps every script that defines
a type it mentions).

**String loads the GUID graph can't see:**
- No `Resources.Load` anywhere in the code, so the other `Resources/`
  folders (TMP Examples, Gridbox) ship but are never loaded. They count as
  unused.
- `Shader.Find` appears only in `Light/PrefabMaper.cs`, and it looks up a
  material's own shader name, so it adds nothing new.
- `SceneManager.LoadScene` loads `"MainLevel"` by name (from `MainMenu`).

**Hidden objects:** an asset that only an inactive scene object uses is
counted separately as **hidden**. An object counts as hidden when it is
inactive, no component references it (so no script can `SetActive` it),
and its parent doesn't switch its children on (`ChanceToDesapear`,
`BridgeHandler`). Tag lookups only find active objects, and no animation
toggles `m_IsActive`, so neither can wake one up.

## The result

| | Size | Share |
|---|---:|---:|
| **Used**, visible in the game | 762 MB | 14% |
| **Hidden**: ships in the build, but only inactive objects use it | 2,546 MB | 47% |
| **Unused**: nothing reaches it | 2,165 MB | 40% |
| **All of `Assets/`** | **5,473 MB** | 2,130 files |

**The big surprise:** the Cobble Games Spaceship (2.4 GB) is not unused.
MainLevel has a `Space Station` prefab instance under a `rotator` object.
Its root is switched off (`m_IsActive: 0`) and nothing ever turns it on,
so the whole pack ships in every build and is never seen. The two F3
Corvette ships (`F3_Green Variant`, `F3_Grey Variant`) are the same case.

**Also found:**
- Everything the game's own code needs is present, and 139 of the 140
  files in `Assets/Scripts` are used (the other is a stray
  `player/w.lighting`).
- The TMP "Examples & Extras", the URP `TutorialInfo`, `SampleScene` and
  every pack demo scene are unused, as expected.

## After pruning

| Step | `Assets/` size |
|---|---:|
| Today | 5.47 GB |
| After dropping the **unused** list below (no scene edits needed) | ~3.31 GB |
| After also dropping the **hidden** ships (R1, R2) | **~0.77 GB** |
| **Done in Phase 5** (measured) | **0.85 GB** (1,001 files) |

**What Phase 5 did:**
- **Dropped 4.6 GB:** 1,042 unused files (2,013 MB) and the two ship
  packs (2,609 MB).
  - Everything dropped is in `Archive/pruned-assets.zip`: 2,479 entries,
    2.4 GB. It stays on this PC and git ignores it.
  - The lists are in `Archive/batches/`.
- **What's left:**
  - 762 MB used by the game.
  - 88 MB unused but kept on purpose: the "keep anyway" list, R3, and
    22 pack files that kept files use. Those are two rock materials in
    the ProBuilder palette, plus the Dumpster, `_Pack` and blood files
    that R3 prefabs use.
- **The gate held:** unresolved GUIDs went from 63 to 31, with none
  new. Played in 2020.3 afterwards (hub, run start, one encounter), and
  nothing went pink or missing.

These numbers are for the working tree. The git history (2.93 GiB) keeps
every old file unless Phase 6 starts a fresh repo (its option A). A fresh
repo from the pruned tree would hold about 0.75 GB of binaries, which
should go to LFS.

---

## Keep/drop by pack and folder

MB = megabytes. "Used" includes files kept only by hidden objects in the
two hidden rows. Paths are under `Assets/`.

### Drop whole: nothing uses them (707 MB)

| Folder | MB |
|---|---:|
| `Prefabs/rest/Props/rest/office` | 208.2 |
| `Files/Thirdparty/PBR Table` | 122.0 |
| `Files/Materials/NotReduced/Rocks` | 69.0 |
| `Files/Thirdparty/PBR Folding Chairs` | 66.5 |
| `Files/Thirdparty/Chair` | 65.0 |
| `Files/Thirdparty/01_AssetStore` (DoorPackFree) | 50.2 |
| `Files/Thirdparty/Zombie` | 30.1 |
| `Files/Thirdparty/BloodDecalsAndEffects` | 30.1 |
| `Files/Thirdparty/Old Military Bed (HQ)` | 27.1 |
| `Files/Materials/NotReduced/concrete` | 15.2 |
| `Files/Thirdparty/AdventureForge` | 9.2 |
| `Files/Thirdparty/Rust Key` | 4.6 |
| `Files/Materials/NotReduced/panels`, `brick`, `net`, `lights` | 5.2 |
| `Prefabs/rest/Props/rest/Dumpster` | 2.7 |
| `Files/Thirdparty/Gridbox Prototype Materials` (a `Resources/` folder nothing loads) | 0.7 |
| `Base/TutorialInfo` (URP template readme) | 0.2 |
| Loose files: `Base/` readme, `SciFi Warehouse Kit/` readme, `Prefabs/rest/` | 0.5 |

### Partly used: keep the used files, drop the rest (1,386 MB to drop)

| Folder | Total | Used | Drop | What the game takes from it |
|---|---:|---:|---:|---|
| `Files/Materials/NotReduced/random` | 663 | 0 | 663 | One material, `New Material 1.mat` (PortalEffect); none of its 131 textures |
| `SciFi Warehouse Kit/Art` | 460 | 235 | 225 | The corridor, wall, catwalk, stair and floor meshes, materials and textures that the room and bridge prefabs are built from |
| `Files/Thirdparty/SkySerie Freebie` | 358 | 54 | 303 | One skybox: the six `CosmicCoolCloud` HDRs and their material |
| `Prefabs/rest/Props/rest/_Pack` | 119 | 84 | 36 | Containers, barrels, wooden barrel, jerry cans |
| `Files/Thirdparty/First_aid_kit` | 74 | 5 | 69 | Kits 1 and 6 (the heal pickup) |
| `Files/Materials/NotReduced/metals` | 33 | 22 | 11 | Rusted floor, hammered copper (PortalEffect), bare plate (LaserTrap), tactile paving |
| `Files/Images` | 29 | 4 | 25 | Two metal floor textures, `hellBackground` (MainMenu), crosshair, border |
| `SciFi Warehouse Kit/Demo` | 18 | 1 | 17 | `Fan_St.wav` (Wall Fan) and one material |
| `Files/Thirdparty/ExampleAssets` (URP template) | 17 | 1 | 16 | `Metal_Blue_Simple_Mat` and `DryWallPainted_Mat` with their textures. The rest was only for `SampleScene` |
| `Files/Materials/NotReduced/lava` | 16 | 8 | 8 | `lava 2` textures (Skull.mat on the spikes) |
| `Base/TextMesh Pro` | 7 | 3 | 4 | TMP core. Drop **Examples & Extras** |
| `Files/Thirdparty/FireExplosionVFX` | 5 | 0.2 | 4 | Fire, flame, glow, smoke textures and materials |
| `Files/LaserMachine` | 2 | 0 | 2 | `Laser_RED.mat` (Triangle enemy). Its scripts and demo are unused |

### Used: keep (324 MB)

Kept whole, or with less than 1 MB of stray files that can go with the
rest. Largest first: `HQ Laptop` 72, `Files/Audio` 64 (60.6 of it used,
mostly the OST), `AcousticGuitar` 40, `Pizza` 36, `Sword_Scabbard` 23,
`OldRadio` 23, `Fire_Extinguisher` 16, `USP` (the pistol) 10,
`Viverna_Assets` 9, `PathGen` 7, `Files/Models` 6, `School Supplies` 4,
and the small ones: `HubProps`, `Projectiles`, `Fonts`, `Particules`,
`_SCI-FI_Barrels_40_Sample`, `Base/Scenes`, `Enemies`,
`SciFi Warehouse Kit/Prefabs`, `Files/Materials`, `Clock`, `Books`,
`BrokenVector`, `Scripts`, `Menu`, `VisualEffects`, `Bowling Kegel & Ball`,
`Meshes`, `Player`, `Presets`, `Samples`, `Animations`, `Light`,
`symbols`, `ItemDrop`, `Settings`, `text`, `RunGen`.

### Hidden: ships but is never seen (the owner decides, see R1 and R2)

| Folder | Total | Only for hidden objects | Unused anyway |
|---|---:|---:|---:|
| `Files/Thirdparty/Cobble Games` | 2,451 | 2,433 (`Space Station`) | 18 |
| `Files/Thirdparty/F3_Corvette` | 158 | 113 (the two ship variants) | 45 |

---

## Keep anyway (the audit says unused, but they stay)

- `Base/Scenes/MainLevel/ReflectionProbe-0.exr`: baked lighting output.
  The binary `LightingData.asset` points at it, and the audit can't read
  binary files. It is the only binary-serialized asset that matters.
- `Base/ProBuilder Data/*`, `Base/Samples/**/.sample.json`: ProBuilder and
  the Package Manager find these by path.
- `Base/Presets/*`: the URP template's import presets (30 KB).
- `MainMenu.unity`: the game's own menu scene.

## Risky drops: the owner decides

- **R1. The Space Station (2,433 MB, the Cobble Games pack).** It is
  inactive in MainLevel, and nothing turns it on. Dropping it saves 44%
  of `Assets/`.
  - **Recommendation: drop it.**
  - Phase 5 must first delete the `Space Station` instance from
    MainLevel **in the Editor** (it sits under `rotator`, and scene YAML
    must not be hand-edited). Only then can the pack go, or its GUIDs
    would show up as new unresolved references.
- **R2. The F3 Corvette ship variants (113 MB, plus 45 MB unused).**
  `F3_Green Variant` and `F3_Grey Variant` in MainLevel are the same
  case as R1.
  - **Recommendation: drop them,** the same way (instances first, then
    the pack).
- **R3. The game's own orphans (about 9 MB).** Nothing reaches them, but
  they are the owner's own work, not a pack's:
  - Prefabs:
    - `RunGen/Stage1/Boss/BossRoomA`
    - `Boss/CorridorExit`
    - `Corridor/Corridor Incline Full`
    - `Enemies/TurretBlueBoss`
    - all of `Prefabs/rest/Trash/` (the Blue skull and turret bosses,
      `TurretRedBoss`, `Void`, `Target`, `lava (1)`)
    - `Menu/MainMenu.prefab`
    - three `symbols/*Symbol` prefabs
    - six `Particules` prefabs
    - `Projectiles/Turret/TurretBullet 1`
    - `Props/prefabs/Dumpster`, `crate 1`, `jerry can 2`
  - 21 unused sound clips in `Files/Audio`: pain, landing, launches,
    `reload`, `shoot`, and others.
  - `Files/Shaders` (`BloodShader`, `PixelStuff`, `PixelsAndStuff`) and
    `VisualEffects/VFXGraphBlood.vfx`.
  - `Files/Meshes/turorialGen/*.fbx`, the `Materials/` loose materials,
    and `Scripts/player/w.lighting`.
  - **Recommendation: keep all of R3 for now.** It costs almost nothing,
    and some of it (the blue bosses, `BossRoomA`, the blood VFX) may be
    unfinished content that Phase 30 wants back. Phase 27 can drop what
    is truly dead.
- **Small hidden objects:** `Cube`, `Cube (1)` and `CameraPixelation` in
  MainLevel are inactive and unreferenced, but everything they use is
  also used elsewhere. **Leave them;** it's a Phase 27 tidy-up.

## Texture sizes (for Phase 28)

Of the textures that are visible in the game, 13 are 4096 px or larger.
- **Ten are SciFi Warehouse Kit textures** (`walls_a`, `Wall Alpha a`,
  `stairs_a`, `walkway_a`, `structure_misc_a`, `floor_tile_a` and their
  normals). They are already **capped at 512** by their import settings,
  so they cost disk space (147 MB) but no build size or VRAM.
- **Three are 4096² UI symbols** in `Files/Materials/symbols`
  (WorkBench, skull, Health: `output-onlinepngtools*.png`). They have no
  cap (8192), so they ship at full size for tiny icons. Cap them at
  256–512 in Phase 28.
- The rest of the 4K+ textures are all in the hidden Space Station
  (46 `module*` TGAs at 64 MB each). They go with R1.

## Binary types to put in LFS (for Phase 6)

The kept files include: `fbx`, `obj`, `dae`, `png`, `jpg`, `tga`, `tif`,
`psd`, `bmp`, `hdr`, `exr`, `wav`, `mp3`, `ogg`, `aiff`, `ttf`, plus the
binary `LightingData.asset`. The plan's list is missing `dae`, `bmp` and
`aiff`.

## Notes for later phases

- `GameManagement/PauseMenu.cs` is not attached to anything in any scene
  or prefab. It is dead code for Phase 27. The audit keeps all of `Assets/Scripts` as a root
  anyway, so dead scripts show as "used".
