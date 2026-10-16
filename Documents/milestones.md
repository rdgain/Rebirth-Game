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

**Status: Not started**

- Derive tests from the design document and the Milestone 3 comparison.
- Focus on independently testable gameplay rules and state transitions.

## Milestone 5: Run tests and report validation results

**Status: Not started**

- Run the gameplay unit tests and Unity framework tests.
- Write a results report and list failures or other issues for review.
- Do not change implementation to fix issues found during this validation milestone.

## Current validation notes

- The user confirmed that input works after restarting Unity.
- The current turn removed temporary UI diagnostics and the `Restart`, `Intro`, and `Start` debug messages.
- Unity compilation and automated tests have not been run by this assistant during these milestones.
