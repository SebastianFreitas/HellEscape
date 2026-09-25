# Baseline: the game as it was (Unity 2020.3.35f1)

Recorded 2026-09-25 by Claude, driving the 2020.3 Editor itself through a
local-only bridge (see "How this was captured"). **Partial:** the flows
under "Not played" were left for Phase 12 (see "Why the baseline stops here").

## Opening the project

- **Opens clean in 2020.3.35f1.** Zero compile errors on first import, and
  no missing-script or missing-reference messages when `MainLevel` loads.
- **Only noise in the import log:**
  - URP/TMP shader "fallback not found" lines (first-import order)
  - a `Debug.Log` from the URP template readme (`Base/TutorialInfo`)
- **Unity deleted two orphan metas** on open:
  `Cobble Games/Spaceship/{HDRP,URP}/*.unitypackage.meta`. Their
  `.unitypackage` files are gitignored and were never in git.
- **Headless (batch mode) does not work in 2020.3 with a Personal
  licence.** It stops with "Missing or bad username or password". The GUI
  Editor, opened from the Hub, is fine. Recheck batch mode on 6.3
  (Phase 9).
- **Security:** the Hub flags 2020.3.35f1 with a security alert. Never
  ship or share a build made with 2020.3.

## Controls (from the code)

| Action | Input |
|---|---|
| Move | WASD. Holding W ramps speed up in tiers |
| Jump | Space |
| Fire | Left mouse (or Left Ctrl) |
| Dash | **Right mouse only** (no key) |
| Interact | E (raycast, 10 m, from screen centre) |
| Grenade | Q |
| Weapon slots | 1 to 4 equip a slot. Tab shows the equipped gun's stats and mods |
| Inventory | I does nothing: `AcessInventory` is not in the scene. The slot list is an always-on HUD panel, top left |
| Pause | Esc (see "Pause" below) |

## Flow of the game (verified by play)

1. **Main menu** (inside `MainLevel`, not `MainMenu.unity`): PLAY,
   OPTIONS, QUIT.
   - The Options screen has resolution, fullscreen, target FPS (60),
     volume, sensitivity, FOV and brightness.
2. **Hub.** It starts dormant: the stations are hidden until you press E
   on the hub **Message**. Then the Mission Selector, the Mirror (5
   upgrades, each with +/−), the Trial, the Crafting Device and the Sound
   Device appear.
3. **Mission Selector:** E on Search, and missions roll onto monitors
   (depth, elite %, special-room %, weapon %, one mutation such as "+5%
   monster action speed"). E on a monitor selects it. E on Open Portal
   opens the portal.
4. **Portal:** it sits about 50 m below the hub, and you drop into it.
   The level generates in about 3 s. The player is disabled meanwhile,
   which causes the "no audio listeners" spam.
   - A run seen: 30 main rooms (corridors plus Encounter/Boon rooms) and
     24 side rooms, ending at TowerBoss.
5. **Encounters:** entering a room closes it and spawns 3 to 5 enemies
   (red Skulls seen). Killing them all reopens it.
   - Gun parts drop: 16 became 21 after one room.
   - Clearing works with real mouse fire.

   - A second run: 43 main rooms (Boon room at 41, TowerBoss at 43).
6. **Weapon drops:** a cleared encounter can leave a `Drop` (tag `Item`).
   Walking over it (or E) puts a random gun in the first free slot of 4.
   Seen: a BASIC gun with 4 mods (+8% move speed, +10% fire rate, +7
   fire, +9 poison), 145 DPS against the starting gun's 100. When all 4
   slots are full, it goes to the 8 "layouts" instead.
7. **Pause (Esc):** shows the main menu (PLAY/OPTIONS/QUIT) by switching
   off the whole `RunningGame` object. `Time.timeScale` stays 1. PLAY
   resumes the run intact (same position, health and rooms). A second
   Esc does not close the menu. `PauseMenu.cs` (the timeScale version)
   is not in use.
8. **Death and restart:** falling below y = −500 kills the player and
   returns them to the hub. Health resets to 50 and the hub goes back to
   dormant (press E on the Message again). **Weapons and gun parts are
   kept.** `isInEncounter` was false this time.
9. **The Path/Bridge/Trial (from the code, not played):** the hub's Trial
   costs 10 + PathLevel gun parts, or 0 after a won run. It builds a
   chain of floating bridge pieces, PathLevel × 2 + 5 long. Every second
   piece spawns monsters (elites once the distance is under PathLevel),
   and the last one spawns a boss. At the end, a new hub is built,
   PathLevel goes up by 1, and the Mission Selector unlocks.

**Not played (stopped by the owner's call, see below):** a boon pick,
crafting, the shop, elites, the TowerBoss fight and the Path. Phase 12's
parity play-test on 6.3 covers them for the first time, with the proper
tools from Phases 13 to 15, and compares against the code rather than
this file.

## Why the baseline stops here

The 2020.3 bridge works: it sends real keys and clicks. But it teleports
between rooms, snaps aim onto enemies and needs health set to 100000.
Each fight takes about 30 s, and a teleport once dropped the player
through a corridor. The owner decided (2026-09-25) that deep play-testing
waits for Unity 6.3 and real test tools, so this baseline records only
what was reached.

## Recovery open questions (from `docs/recovery.md`)

1. **White `Image` in `CanvasInventory`:** never visible. The prefab is
   only referenced by `Gun.inventory`, a field no code reads, so it is
   never instantiated.
2. **Pink mesh on the Path start piece (`old/Path.prefab`):** never
   visible. It is only referenced by `StartHub.path`, which no code uses
   (`StartPath` builds bridge pieces instead).
3. **SkullBoss and TriangleGreen with the untextured material:** not
   seen. TriangleGreen spawns on Path pieces and SkullBoss is referenced
   from `MainLevel`. Check them in Phase 12.

## Performance

- 56 to 60 FPS everywhere seen (hub, generation, fights). Target FPS
  defaults to 60, so this is the cap.
- One 100 to 150 ms hitch when entering Play.

## Console in play

- Warnings, every run start:
  - `GunOfAType must be instantiated using ScriptableObject.CreateInstance`
  - `You are trying to create a MonoBehaviour using the 'new' keyword`
- Logs:
  - `This is the total boons -> 48`, once per `VoidBoon` construction
  - `Reset level generator`, logged as an **Error** by
    `RoomGenerator.ResetLevelGenerator` (`RoomGenerator.cs:603`) when a
    generation attempt fails and retries. It is not a crash; the run
    still generates.
  - `There are no audio listeners in the scene`, repeated during
    generation

## Oddities and bug candidates (for Phase 16, not verified further)

- **`GameMan.isInEncounter` stays `true` after dying in an encounter**,
  back in the hub.
- **Enemies spawn at the room's centre.** A player standing there takes
  10 damage every quarter-second and dies in about 1 s.
- **Some enemies were out of reach:** in 2 of 5 rooms, 2 enemies were
  never killed in 90 s of aimed fire, and they followed the player into
  corridors. It happened again in run 2 (1 enemy left in a RoomDonut).
  It could be the aim harness; check.
- **Esc can't be closed with Esc,** and it doesn't pause time (see Pause).
- **The I key does nothing** (`AcessInventory` isn't in the scene).
- **A corridor teleport fell through the world** (run 1, room 10). This
  may be the harness teleporting before the floor was enabled.
- **Options:** the Brightness label reads 0.00 with its slider at full.
- **`PlayerInventory`:** the first-run fallback writes `"Gunparts"`
  (lowercase p) but reads `"GunParts"`. It's harmless, because
  `OnDisable` saves the right key, but the stray key exists in real saves.
- **Deaths below y = −500:** falling kills via
  `TakeDamage(10000000)` (`PlayerBasicMovement.cs:162`), and `Die()`
  returns you to the hub.

## Saves (PlayerPrefs)

- **Where they live:**
  - A built game uses `HKCU\Software\WeMakeGames\HellEscape`.
  - Editor Play Mode uses `HKCU\Software\Unity\UnityEditor\WeMakeGames\HellEscape`.
  - So the old saves are untouched by Editor play.
- **The owner's old build saves still exist** (155 values). They are
  backed up and decoded, locally only, in `.claude-bridge/saves/`
  (`oldsaves.reg`, `oldsaves.txt`). Phase 3 moves them off-repo.
  - 5 stored guns, all level 10: 2 basic, 2 sniper, 1 sniper_A1.
  - PathLevel 1, GunParts 9.
  - Mirror: MissionChance 3, the rest 0.
  - FOV 90, Sensitivity 1.0, Volume 0.3, REFRESH 244.
- **Keys** (floats are stored as 64-bit doubles in the registry):

| Key | Type | Written by | Read by |
|---|---|---|---|
| `PathLevel` | int | `PathFloor` | `StartHub`, `Trial`, path code (19 calls) |
| `GunParts` (+ stray `Gunparts`) | int | `PlayerInventory` | `PlayerInventory` |
| `Volume`, `Sensitivity`, `FOV`, `BRIGHT`, `REFRESH` | float | `MainMenuManager` | `MainMenuManager`, `MouseLook` |
| `MaxLife`, `MissionChance`, `ItemLevel`, `MissionMaxMods`, `BoonChance` | int | `MirrorManager` (enum names) | `MirrorManager` |
| `attemptsTower` | int | `TowerBoss` | `TowerBoss` |
| `WeaponExists{slot}` | int ±1 | `Inventory` | `Inventory`, `Gun` (slot 0) |
| `StoredEnumStringType{slot}`, `maxLevel{slot}`, `GradeWeight{0..2}{slot}` | string/int | `Inventory` | `Inventory` |
| `id`, `value`, `tier`, `StoredEnumStringGrade` + `{slot}{mod}` | int/string | `Inventory` | `Inventory` |

`MainMenuManager` has a commented-out `PlayerPrefs.DeleteAll()` in
`Awake`. The earlier count of "1 DeleteAll call" is that comment.

## How this was captured

- **A local-only Editor bridge.** `Assets/Editor/ClaudeBridge/ClaudeBridge.cs`
  plus helper scripts in `.claude-bridge/tools/`. Both are excluded from
  git in `.git/info/exclude`, and must be deleted when Phase 2 closes.
  Commands go in as files under `.claude-bridge/in/`.
  - It can open scenes, play and stop, and click UI buttons.
  - It can reach any field or method by reflection.
  - It can teleport onto floors, aim, list rooms and take Game-view
    screenshots.
  - It mirrors the console to a file and reads FPS.
- **Real input:** the legacy Input Manager can't be faked from code, so
  the tools focus the Game view and send real keys and clicks
  (`in.sh`, `use.sh`, `fight.sh`).
- **A health override:** runs used health 100000, set in memory only.
- **Screenshots** are in `.claude-bridge/shots/`, outside git.
