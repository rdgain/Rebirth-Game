# Project Milestones

This document tracks the requested modernization and validation work. Progress describes work completed in the project; it does not imply that Unity tests or browser builds have passed unless a result is recorded below.

## Milestone 1: Update input to the Input System

**Status: Complete**

- Migrated gameplay movement and action input to `UnityEngine.InputSystem` through `Assets/Scripts/GameplayInput.cs`.
- Migrated the EventSystem UI module to `InputSystemUIInputModule` and removed the legacy UI input modules.
- Replaced deprecated Unity object lookup APIs encountered during compilation cleanup.
- Unity was restarted; input devices appeared again in Input Debugger, and the user confirmed that input works as expected.
- Temporary UI device and raycast diagnostics have been removed.

## Milestone 2: Document implemented gameplay from code

**Status: Complete**

- Added [Gameplay_implemented.md](Gameplay_implemented.md), describing scene and level flow, controls, player and clone behavior, puzzles, UI, and code-observed caveats.
- The document distinguishes active behavior from comments or room notes whose behavior is not implemented by the current scripts.
- Updated the document after fixing timer completion, level restart, and start-room coordinates; added room-by-room comparisons for Temple 2 comments.

## Milestone 3: Cross-reference gameplay with the requirements and plan

**Status: Not started**

- Compare `Documents/Gameplay_implemented.md` with `Documents/RequirementsAndPlanByGroup8.pdf`.
- Record requirements that are implemented, partly implemented, missing, or differ from the active code.
- Highlight differences for review without changing gameplay.

## Milestone 4: Write gameplay unit tests

**Status: Tests written; execution deferred to Milestone 5**

- Added an EditMode test assembly at `Assets/Tests/EditMode` using Unity Test Framework 1.8.0.
- Tests cover mirror-path tolerance, Temple 2 colored-switch sequence/reset, stopping the timer on level completion, and locating Temple 2's configured start room at its level-data coordinates.
- The requirements PDF comparison remains pending from Milestone 3; these tests cover behaviors described by active code and the room-level design notes.
- Tests have not been run. Execution and reporting are Milestone 5.

## Milestone 5: Run tests and report validation results

**Status: Complete for the available EditMode suite; visual/browser checks remain outstanding**

- Ran Unity batch import/script compilation validation successfully with Unity 6000.6.6f1.
- Ran all available Unity EditMode tests: 5 passed, 0 failed, 0 skipped.
- Built the enabled scenes for Windows 64-bit and WebGL successfully.
- Wrote [Unity_validation_report.md](Unity_validation_report.md) with command results and limitations.
- The project was not played in the GUI, and the WebGL output was not tested in a browser.

## Current validation notes

- The user confirmed that input works after restarting Unity.
- The current turn removed temporary UI diagnostics and the `Restart`, `Intro`, and `Start` debug messages.
- Unity 6000.6.6f1 batch validation, the available EditMode test suite, and Windows 64-bit/WebGL builds completed successfully; see [Unity_validation_report.md](Unity_validation_report.md).
- The GUI was not opened for visual gameplay validation, and the WebGL output was not browser-tested.
