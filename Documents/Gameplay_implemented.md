# Gameplay Implemented in Code

This document summarizes behavior implemented by the active scripts and scene data in the current project. It is a code survey, not a statement of intended design. Room-file comments and UI text are called out separately where active code differs. The requirements comparison is Milestone 3.

## Scenes and level setup

The build settings include `Assets/Scenes/Rebirth1.unity` and `Assets/Scenes/Rebirth2.unity`. Both scenes use the shared `Game` prefab and override its `Temple.CurrentLevel` and `MapEngine.LevelData` references:

- `Rebirth1` sets level 1 and loads `Assets/Resources/Levels/temple1.txt`.
- `Rebirth2` sets level 2 and loads `Assets/Resources/Levels/temple2.txt`.

`MapEngine` reads level and room text assets from `Resources/Levels`. It places the configured start room, adds the remaining rooms in randomized order, rotates or flips rooms to connect their doors, creates connecting corridors, then instantiates each room’s objects and tile background. The room data defines five rooms per temple. Objects are loaded by resource path through the level or room legend. `MapEngine.ActiveRoom()` determines which room contains the player; clone behaviors use it to limit movement or AI effects to that room.

## Starting, timer, pause, and completion flow

`Temple` disables player movement during initialization, sets the level-specific overlay text, initializes the timer from `InitialTime` (300 seconds on the shared prefab), and shows the introduction. The Start button calls `Temple.MainGame()`, which hides the introduction, enables player control, and starts the timer.

The timer uses `Time.realtimeSinceStartup`. `Pause()` stops the timer and disables player movement; unpausing resumes both. When the timer expires, `Temple` disables player movement and shows the game-over overlay. The overlay offers restart and exit actions; the completion overlay offers continue for level 1 and credits for level 2. `LoadNextLevel()` loads the scene named `Rebirth` plus the next level number.

Level 1 completion is driven by `BigButton`: when the player enters its trigger, it checks the four `Reward` components. Each reward is marked gathered when an unlocked chest adds its contents to one of the four altar positions. If all four rewards are gathered, `Temple.LevelFinished()` is called.

In level 2, `TempleSolver` waits until four chests have been opened, then destroys the clone and spawns the sister. Touching the sister with the player calls `Temple.LevelFinished()`. Since the current level is 2, this also starts the sister conversation; the conversation’s continue and exit buttons advance and close it.

## Controls

`GameplayInput` creates and enables Input System actions in code:

- Movement: WASD or arrow keys. The gamepad left stick supplies an axis when that keyboard axis is not pressed.
- Action: E, detected on release. It is used for room clone-control toggles, opening unlocked chests, and pushing moving blocks.
- Tether: T, detected on release by `GameplayInput`; there is no active gameplay consumer because `Clone4Script`’s movement implementation is commented out.
- UI: `InputSystemUIInputModule` handles EventSystem UI input. The Start, pause, restart, conversation, credits, and exit buttons are wired in the shared Game prefab.

`Player` applies movement in `FixedUpdate`, updates directional animation, and respects both the player-control flag and `canMove`. Other scripts can disable player movement while controlling a clone or during dialogs and game states.

## Clone behavior

- **Clone 0:** The player can toggle direct clone movement with E while in the clone’s room. Movement uses the shared movement actions. The camera follows this clone while it is controlled, and player control is disabled during that control mode.
- **Clone 1:** E toggles direct clone movement while the player is in its room. It follows the shared movement actions. This controller does not disable main-player control.
- **Clone 3:** An AI clone wanders when it has not detected the player, using four cardinal raycasts to detect the player. When detected, it steers toward the player and penalizes the timer. When it is wandering in the player’s room, it adds time while the timer is below its initial value. The active chase uses `speed`; the `chaseSpeed` field is not used.
- **Clone 4:** Its `FixedUpdate` movement, control, and tether logic is commented out. The script currently performs no clone-control behavior. Room-file comments describing clone control and a four-tile tether are therefore not implemented by this script.
- **ShadowPlayer:** Applies its velocity only while its parent room is active, and manages directional animation. Clone bumpers set its velocity; doors reverse it on collision.

## Puzzle behavior

### Rebirth1 / Temple 1

The level data includes an altar room and four puzzle rooms. These use switches, moving blocks, traps, mirrors and light-path objects, darkness/light controls, chests, and rewards.

- Switch solvers and `BridgeFunc` connect switch state to bridge creation or removal and block destruction.
- The mirror solver for the light-path room uses the room’s `lightPath` rule, configures each `LightCheck`, and unlocks the chest when the light sink is within the configured angle tolerance from the source.
- The darkness scripts switch the active light between the player and clone lights based on the named room switches.
- Opening an unlocked chest adds its item to an altar position based on which side of the altar the chest occupies. The four resulting `Reward` components drive level completion.

The Temple 1 introduction text says the player has five minutes; this agrees with the shared prefab’s 300-second initial timer.

### Rebirth2 / Temple 2

The level data includes bridge and clone-control rooms, mirror and switch puzzles, a clone stealth room, and a sliding-tile/permanent-switch room.

- In the bridge room, ordinary switches activate bridges while held and deactivate them when released; permanent switches activate their assigned bridges. Another switch destroys a named block.
- The first mirror room's `MirrorSolver2` requires the configured light path to be within tolerance and every child switch to be triggered before unlocking the chest.
- The red, green, and blue switches accept the R → G → B order. Pressing a switch out of order resets all three; `RGBSwitchSolver` unlocks the chest only when all three are triggered.
- The trap switch enables surrounding pit colliders; the darkness controller changes which character’s light is enabled from the two light switches.
- The stealth room’s Clone 3 AI wanders, detects the player with cardinal raycasts, chases when detected, and applies timer penalties while chasing.
- The final room contains sliding tiles, hooks, and permanent switches. The room-file differences for sliding and hook behavior, plus the inactive Clone 4 controller, are described below.
- `PermaSwitchSolver` unlocks its chest when all tagged child permanent switches are triggered.

## Room-file comments compared with active behavior

Temple 1’s room files have no `Comments` sections. Its `Rules` sections are parsed configuration, not prose commentary. Temple 2’s `puzzle0` notes about switch-to-bridge mappings and temporary/permanent bridges match `BridgeFunc` and `BridgeScript`; switch 6 removes `BlockDestroy`, as the comment says.

The following differences are in Temple 2 room comments:

### Temple2/puzzle1.txt

- **Room note (line 42):** “win condition: mirros in right position, switch pressed => chest opens, get reward.” `MirrorSolver2.Update()` now requires the sink's `LightCheck.inToleranceFromSource()` to pass and every child `Switch` to be triggered before unlocking the chest. The reward wording is not reflected as an altar reward: Temple 2 has no altar and `ChestScript` has no altar to receive an item.
- **Room note (line 43):** the clone can be controlled directly and uses player movement input. `Clone1Script` toggles direct clone control with E and applies the shared movement input, matching the updated note.
- The reward wording on line 42 is not reflected as an altar reward in this scene. Temple 2’s room data contains no altar. `ChestScript` therefore has no altar to receive an item; it still sets `openned` for level progression.

### Temple2/puzzle2.txt

- **Room notes (lines 67–69):** the R → G → B order and reset-on-error behavior are implemented by the colored switch scripts; `RGBSwitchSolver` unlocks the chest once all three are triggered. The solver polls switch state, while the scripts enforce which switch can activate next. The room note's “get reward” wording is not reflected as an altar reward: Temple 2 has no altar and `ChestScript` has no altar to receive an item.
- **Room note (line 74):** the clone-light vision description now matches the `ShadowPlayer` prefab setup: range 25 and spot angle 25 degrees. `CloneDarkness` switches between the player and clone lights; it does not separately enforce a 5-by-5 gameplay boundary.
- The reward wording on line 69 is not reflected as an altar reward in this scene. Temple 2’s room data contains no altar. `ChestScript` still opens the chest and sets `openned` for level progression.

### Temple2/puzzle3.txt

- **Room notes (lines 46–48):** the player “have[s] to cross room without being seen” and being spotted causes a time penalty. `Clone3Script` detects and chases the player, and penalizes the timer while doing so, but being seen does not itself block the player, fail the room, or trigger completion. Level progression is handled by the four opened chests and the spawned sister.
- **Room note (line 47):** the player can “spawn at any door” and doors are locked selectively. This room has one `Spawn` object at a fixed position and fixed `Door`/`Door_Locked` objects in its room data. `Spawn` uses the placed spawn object; no active script selects a spawn door or randomly chooses which doors to lock.
- The comments that the clone wanders, chases when it spots the player, and cannot be controlled match the active Clone 3 behavior at a high level. Detection uses four cardinal raycasts.

### Temple2/puzzle4.txt

- **Room notes (lines 42–44):** directional tiles slide until a wall, pit, or normal floor. `SlideRightScript`, `SlideUpScript`, and `SlideDownScript` apply directional impulses to the player while the player remains in the trigger and zero the player’s Rigidbody velocity on trigger exit. They do not move the tile or explicitly test for those three stopping surfaces.
- **Room note (line 48):** either character slides through when the other occupies a hook. `HookScript` applies an impulse to the player if the clone is already there. The reverse case, pushing the clone when the player is already there, is commented out.
- **Room notes (lines 46, 50, and 52):** a four-tile tether, anchoring at the tether limit, and switching control to the clone. `Clone4Script.FixedUpdate()` is entirely commented out, so none of those behaviors is active.
- **Room note (line 51):** the permanent-switch win condition matches `PermaSwitchSolver`, which unlocks the chest once all tagged child permanent switches are triggered. The reward wording has the same limitation as the other Temple 2 chests: no altar is present to record an altar reward.
- The pit-reset comment is supported by `PitScript`, which moves the player or clone to the corresponding room spawn point when they enter a pit.

## Previously noted code issues resolved

- `Temple.LevelFinished()` now stops an active timer before marking the level complete, preventing a later timeout from replacing the success state with game over.
- `Temple.Restart()` now always reloads `CurrentLevel`, including after success or failure.
- `MapEngine.CreateMap()` now places the start room using `StartRoomX` and `StartRoomY`.

## Additional code issues for review

- `Rule_Win` is registered in `AppliedRules`, but its `isThisRule`, `implement`, and `parse` methods throw `NotImplementedException`. Current room rule data only uses `lightPath`, so the stub is not invoked by the present scenes.
- `RuleParser.parse(List<string>)` checks `sections[0][0]` for every rule line instead of checking `sections[i][0]`. Current room data has one rule line per room; adding multiple different rule lines would dispatch them using the first line’s key.

## Main scripts

| Area | Scripts |
| --- | --- |
| Input and player | `GameplayInput`, `Player`, `CameraControl` |
| Level and room generation | `LevelGeneration/MapEngine`, `Spawn` |
| Timer, UI, and scene flow | `Temple`, `Overlay`, `StopWatch` |
| Rewards and chests | `ChestScript`, `Reward`, `Altar1`, `BigButton`, `TempleSolver` |
| Clones | `Clone0Script`, `Clone1Script`, `Clone3Script`, `Clone4Script`, `ShadowPlayer`, `CloneBumper`, `CloneDarkness` |
| Puzzle mechanisms | `Switch`, `PermaSwitch`, colored switch scripts, switch solvers, `BridgeScript`, `BridgeFunc`, `DoorController`, `MovingBlock`, sliding scripts, `HookScript`, `PitScript`, `TrapSwitchT2Script` |
| Mirror and light puzzles | `RuleSystem/RuleParser`, `RuleSystem/Rules/Rule_LightPath`, `LightPuzzle/LightCheck`, `LightPuzzle/LightCollision`, mirror solvers |

