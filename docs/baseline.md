# Baseline: the game as it was (Unity 2020.3.35f1)

Recorded 2026-09-25 by Claude, driving the 2020.3 Editor itself through a
local-only bridge (see "How this was captured"). **Partial:** the flows
marked *not yet* are for the next session (see PLAN.md Phase 2 handoff).

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
| Inventory | I, then Tab and 1 to 4 inside it |
| Pause | Esc |

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

**Not yet covered:** boon pick, weapon drop, crafting, the shop, elites,
the TowerBoss fight, the Path/Bridge/Trial, death and restart, the
inventory screen, pause. Also still open: the three visual checks at the
end of `docs/recovery.md`.

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
  corridors. It could be the aim harness; check.
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
