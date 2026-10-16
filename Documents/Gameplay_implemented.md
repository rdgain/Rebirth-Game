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
- The first mirror room has a `MirrorSolver2`, but its mirror-angle validation is commented out. The active solver checks that all child switches are triggered before unlocking the chest.
- The red, green, and blue switches have state-reset interactions. However, `RGBSwitchSolver` currently checks `switchB && switchR && switchB`; it checks blue twice and does not require green. This differs from the room-file comment describing an R-G-B sequence.
- The trap switch enables surrounding pit colliders; the darkness controller changes which character’s light is enabled from the two light switches.
- The stealth room’s Clone 3 AI wanders, detects the player with cardinal raycasts, chases when detected, and applies timer penalties while chasing.
- The final room contains sliding tiles, hooks, and permanent switches. Sliding scripts apply directional impulse to the player, and hooks react to player/clone occupancy. The room’s Clone 4 control/tether behavior is inactive because its controller code is commented out.
- `PermaSwitchSolver` unlocks its chest when all tagged child permanent switches are triggered.

## Code-observed caveats for review

These are observations from the current code, not proposed changes:

- `Temple.LevelFinished()` does not stop the timer. If the timer remains active and later expires, `Temple.Update()` can still call `Failure()` after a level has been marked finished.
- `Temple.Restart()` reloads the current level while the game is active, but after the terminal state it always loads level 1.
- `MapEngine.CreateMap()` passes `StartRoomX` as both the X and Y coordinate when placing the start room, even though level files include a `StartRoomY` value.
- The Clone 4 room notes describe control and tether rules that are not active in the script.
- Some room comments describe puzzle conditions that differ from the active solver checks, as noted above.

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

