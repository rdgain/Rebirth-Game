# Unity Project Guidelines

## Goals
- Preserve the original gameplay, visual identity and feel.
- Modernise the project incrementally and prepare it for browser deployment.
- Prioritise maintainability, small download size, fast startup and responsive controls.

## Working rules
- Inspect the project before proposing significant changes.
- Never upgrade Unity versions, packages or project settings without an explicit plan.
- Make small, reviewable changes and explain their purpose.
- Do not change gameplay rules unless explicitly requested.
- Prefer built-in Unity features over adding new dependencies.
- Avoid broad refactors during compatibility fixes.
- Do not delete assets or modify scenes and prefabs without explaining the impact.
- Never commit secrets, credentials or machine-specific settings.
- Do not look outside the Rebirth-Game folder.

## Validation
- Report which changes were made, which files were affected and what remains untested.
- Check compilation, Console errors and relevant gameplay behaviour after each change.
- For Web builds, test in an actual browser, not only in the Unity Editor.
- Measure performance and build size before and after optimisation.
- Write unit tests to check core functionality. Rerun tests and fix errors that may arise.