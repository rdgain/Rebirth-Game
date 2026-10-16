# Unity Validation Report

## Environment

- Project Editor version: Unity `6000.6.6f1`, read from `ProjectSettings/ProjectVersion.txt`.
- Exact Editor installation found at `C:/Program Files/Unity/Hub/Editor/6000.6.6f1/Editor/Unity.exe`.
- The Editor installation has Windows Standalone and WebGL build support.
- Enabled build scenes: `Assets/Scenes/Rebirth1.unity`, `Assets/Scenes/Rebirth2.unity`.
- Package manifest declares Unity Test Framework `1.8.0`.

## Results

| Command | Result | Evidence |
| --- | --- | --- |
| GUI open (`Open`) | Not launched in this validation pass. The script supports launching the installed exact-version Editor and checks for an existing instance of that Editor first. | No GUI or visual gameplay result claimed. |
| Batch validation (`Validate`) | **Passed**, Unity exit code 0. Unity imported the project and compiled scripts. | `Artifacts/Logs/Validate-20261016-210448.log` |
| EditMode tests (`Test -TestPlatform EditMode`) | **Passed**, 5 total, 5 passed, 0 failed, 0 skipped, 0 inconclusive. | `Artifacts/Logs/Test-EditMode-20261016-210521.xml` and `Artifacts/Logs/Test-20261016-210521.log` |
| Windows 64-bit build (`Build -Target Windows64`) | **Passed**. Unity produced the executable and data folder. | `Artifacts/Builds/Windows64/Rebirth.exe`; log: `Artifacts/Logs/Build-20261016-210542.log` |
| WebGL build (`Build -Target WebGL`) | **Passed**. Unity produced the WebGL loader, framework, WASM, and data bundle. | `Artifacts/Builds/WebGL/Rebirth/index.html`; log: `Artifacts/Logs/Build-20261016-210839.log` |

The EditMode suite contains the five regression tests in `Assets/Tests/EditMode/GameplayRegressionTests.cs`. No PlayMode test assembly or additional project test suite was present in this project.

The first Unity compile exposed an API mismatch in the new build entry point's summary log (`BuildSummary.totalScenes` is unavailable in this Editor version). That build-tool line was corrected, then batch validation, tests, and builds were rerun; final runs completed successfully. Unity also logged an access-token refresh warning while connecting to the Licensing Client, but it connected and each final command returned exit code 0.

## Limitations

- The project was not played in the Unity GUI, and no gameplay behavior was visually inspected.
- The WebGL output was not opened in a browser. A successful WebGL build does not establish browser runtime compatibility.
- Batch Unity runs launched from the restricted sandbox could not connect to the signed-in Unity Licensing Client and stalled. The successful validation, test, and build runs used the approved signed-in Windows session.
- Unity 6 initially changed `ProjectSettings/UnityConnectSettings.asset` while opening the project. That generated settings change was restored; `ProjectSettings/ProjectSettings.asset` and `ProjectSettings/EditorBuildSettings.asset` have no changes from this work.

## Workflow

Use PowerShell with a per-command execution-policy bypass; this does not change the machine's persistent execution policy. The project launcher resolves the exact Editor version from `ProjectVersion.txt`, does not install Unity, applies a timeout to batch commands, and stores generated files under the ignored `Artifacts/` directory.

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Open
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Validate
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Test -TestPlatform EditMode
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Build -Target Windows64
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Build -Target WebGL
```
