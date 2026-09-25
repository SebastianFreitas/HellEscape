# Recovery report

Phase 1 of `PLAN.md`, 2026-09-25.

**Verdict: nothing the game needs is missing from this repo.** Nothing
needed recovering, so there is no `recovery` branch.

## Scope

The owner decided that **this repo is the only source**. Their words
were: "it should have everything, if it doesn't we deal with that later".
The other folders on this PC were not diffed:
- `Downloads/Voidscape.zip`
- `Downloads/HellEscape-main/`
- the Desktop `WebGame2-main`

Two things found while checking what those folders are:
- **`Downloads/buildWeb.zip` is not VoidScape.** Its product name is
  "UnamedWebGame", it was built in 2021.3.8f1, and it uses Cinemachine and
  Tilemap. It is another project.
- **`Downloads/voidscape.html` is not a build.** It is the Portfolio page
  about VoidScape.

No old playable build of VoidScape exists.

## How it was checked

`tools/refscan.py` does the whole check without opening Unity:
- It reads every `guid:` reference in the project's YAML and JSON assets
  and importer `.meta` files: scenes, prefabs, materials, `.asset`,
  controllers, anims, VFX files and shader graphs.
- It resolves each GUID in this order:
  - the project's own `.meta` files: 2,687 assets
  - the packages at the exact versions in `packages-lock.json`, downloaded
    from Unity's registry, plus the built-in packages that ship with
    Unity 2020.3.35f1: 6,543 assets
  - Unity's built-in resources (`0000000000000000…`)
- It then walks the dependency graph from `ProjectSettings` (which holds
  the build scene list), `MainLevel`, `MainMenu` and every `Resources/`
  folder. That shows which missing GUIDs the game can actually reach.

```bash
python tools/refscan.py <scratchpad>/pkgcache
```

| | GUIDs |
|---|---|
| Referenced in total | 1,676 |
| Project assets | 1,507 |
| Package assets | 103 |
| Built-in | 3 |
| **Unresolved** | **63** |

The quick scan in `PLAN.md` found 154 unresolved GUIDs. Of those, 91 were
package assets.

Unity itself has not confirmed any of this yet. Phase 2 opens the project
in 2020.3 and cross-checks against `Editor.log`.

## The 63 unresolved GUIDs

### Deleted on purpose (5)

These five were in git once, and later commits removed them:

| GUID | Was | Removed in | Still referenced by |
|---|---|---|---|
| `fefa5b83…` | `Prefabs/Enemies/SkullTurret/Turret.prefab` | `01ed7441` "making blue red turret" (2022-04-06) | `explosionEffect` on TurretRed, TurretGreen, TurretBlueBoss and Trash/TurretBlue |
| `70c2b71b…` | `Prefabs/text/PrefabTextDamage.prefab` | `6f0ac3f0` "some final touches in pop up text" (2021-10-11) | `damagePopup` on the player projectiles `normal`, `antigrav` and `horizontal` |
| `52dac95b…` | `Images/f7ae366e….jpg` | `f097dd38` "new health bar new lights" (2021-09-03) | the sprite of an `Image` in `Prefabs/rest/Trash/CanvasInventory.prefab` |
| `9dd9349a…` | `Materials/NotReduced/metals/paintedCannister/red.mat` | `afd1bff0` "changed to gamma…" (2021-09-03) | `Particules/trash/BloodExplosion` and `BloodSplater` |
| `be76e5f1…` | `Scripts/SimpleCameraController.cs` | `55fefce4` (2021-08-29) | `SampleScene.unity` (disabled in Build Settings) |

If any of them is wanted back, `git show <commit>^:<path>` restores it
together with its `.meta`.

### The 12 the game can reach from MainLevel

| Missing | Used by | Effect | Severity |
|---|---|---|---|
| Turret explosion prefab (`fefa5b83…`, deleted above) | `ConeMonster.explosionEffect` on TurretGreen and TurretRed | None. `ConeMonster.cs` declares the field but never uses it. | none |
| Damage-popup prefab (`70c2b71b…`, deleted above) | `PlayerProjectile.damagePopup` on `normal.prefab` | None. The field is declared and never read. | none |
| Sprite (`52dac95b…`, deleted above) | an `Image` in `CanvasInventory.prefab` (in MainLevel) | The Image draws as a plain white rectangle. Phase 2 checks whether it is visible. | cosmetic |
| `_BaseMap` texture `d9b0a55d…` | 9 materials in `Files/Materials/Simple/Transparent/`, including `Transparanet.mat` on SkullBoss and TriangleGreen | The material shows its flat base colour with no texture. It was probably always like this: the texture was never in git. | cosmetic |
| Material `c3c194fc…` | a renderer in `PathGen/pieces/old/Path.prefab` | That mesh renders pink or invisible. The prefab is under `old/` but is still linked from `StartPiece`. | cosmetic, check in Phase 2 |
| USP pistol textures `4dd757d3…`, `990d9c94…`, `70b06913…`, `2461875c…` | `USP/mats/Slide.mat`, `Suppressor.mat` and `Laser.mat` (emission, parallax and metallic maps) | These are secondary maps only; each albedo is present. The gun looks slightly flatter. | cosmetic |
| Importer remaps `f6ac77a0…` (USP.fbx) and `a1ddc248…` (Globe.fbx) | model-importer "external material" settings | Unity falls back to the model's embedded material. | none |
| VFX script `081ffb00…` | `Portal.vfx` and `VFXGraphBlood.vfx` | A pipeline sub-output stub. Unity's own VFX Graph 10.9 templates carry the same dangling GUID. | none |

### The other 51: pack, demo and trash content the game doesn't use

These are referenced only from pack demo scenes, pack example
materials, `SampleScene` (disabled in Build Settings), trash prefabs or
pack-internal `Resources/` folders. Three of them sit under `Resources/`,
so they ship in a build, but no game code loads them. All three are in
the Gridbox palette and TMP's `Bangers SDF Logo.mat`.
- **TextMesh Pro "Examples & Extras":** 17 GUIDs, including 2 missing
  example scripts used in 19 scenes and prefabs.
- **Viverna demo scene:** 10.
- **`SampleScene` and ExampleAssets:** 7, including the deleted
  `SimpleCameraController`.
- **Blood Decals demo:** 4.
- **2 each:**
  - the URP template `Readme` icons
  - the stray `Assets/Materials/*.mat` textures
  - the OldRadio `PostProcess.asset`, whose missing script is the old
    Post Processing v2 one
- **1 each:**
  - the Zombie pack (a missing script on `Zombie3`)
  - the AdventureForge cabinet animations
  - School Supplies `demo.unity`
  - the Gridbox ProBuilder palette (its script is from a newer
    ProBuilder)
  - the `USP-Tact.unity` demo font
  - `TurretBlueBoss.turretBullet` (TowerBoss uses its own turret copies,
    which are fine)
  - `Particules/trash/Blood*`

Phase 4 or 5 will prune most of this. The Phase 5 gate is that the
unresolved count must never grow above **63**.

## Still missing and worth caring about

**Nothing.** Everything above is either unused or cosmetic. Phase 2 must
confirm these in the running game:
1. whether the white `Image` in `CanvasInventory` is visible
2. whether a pink or invisible mesh shows up on the Path's start piece
   (`old/Path.prefab`)
3. whether SkullBoss and TriangleGreen look right with the untextured
   transparent material

## Backups

Made by Phase 3 on 2026-09-25. All of it lives in
`C:\Users\Traff\Backups\VoidScape\`, outside the repo.

- **`VoidScape-2026-09-25-unity2020.3.zip`** (5.8 GiB, 4,962 files,
  8.3 GiB unpacked): the whole working folder, `.git/` included, without
  `Library/` and `Temp/`. `7z t` passes. The only files left out are 24
  shader-compiler logs in `Logs/` that a stray process had locked.
  To restore, unzip it and open the folder from the Hub in 2020.3.35f1.
- **`oldsaves-registry/`:** the old build's PlayerPrefs, exported from
  the registry in Phase 2 (`oldsaves.reg`, plus a readable
  `oldsaves.txt`). The same files are still in `.claude-bridge/saves/`,
  which is untracked.
- **Git tag `v0-unity2020.3`:** the last 2020.3 state. It is the
  original game (up to `691cf353`, "trial becomes 0 cost on win") plus
  the revival's `PLAN.md`, `docs/` and `tools/`, and nothing else.
  A fresh clone of it matches the working project, apart from two
  Editor-written settings files (ProBuilder `Settings.json` line endings
  and `UserSettings/`).
