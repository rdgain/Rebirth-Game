<#
.SYNOPSIS
Launches the Unity Editor recorded in ProjectSettings/ProjectVersion.txt.

.DESCRIPTION
Open starts the GUI. Validate performs a batch import and script compilation.
Test runs Unity Test Framework tests. Build builds enabled EditorBuildSettings scenes.
Logs, test results, and builds are stored beneath Artifacts/.

.EXAMPLE
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Open

.EXAMPLE
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Validate

.EXAMPLE
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Test -TestPlatform EditMode

.EXAMPLE
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Build -Target Windows64

.EXAMPLE
powershell -NoProfile -ExecutionPolicy Bypass -File tools/unity.ps1 Build -Target WebGL
#>
[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [ValidateSet("Open", "Validate", "Test", "Build")]
    [string]$Mode = "Open",

    [ValidateSet("EditMode", "PlayMode")]
    [string]$TestPlatform = "EditMode",

    [ValidateSet("Windows64", "WebGL")]
    [string]$Target = "Windows64",

    [ValidateRange(30, 3600)]
    [int]$TimeoutSeconds = 900
)

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$versionFile = Join-Path $projectRoot "ProjectSettings/ProjectVersion.txt"

if (-not (Test-Path -LiteralPath $versionFile)) {
    throw "Cannot find ProjectSettings/ProjectVersion.txt in $projectRoot"
}

$versionText = Get-Content -LiteralPath $versionFile -Raw
if ($versionText -notmatch 'm_EditorVersion:\s*(\S+)') {
    throw "Could not read the required Unity Editor version from $versionFile"
}

$version = $Matches[1]
$editor = Join-Path "C:/Program Files/Unity/Hub/Editor" "$version/Editor/Unity.exe"
if (-not (Test-Path -LiteralPath $editor)) {
    throw "Required Unity $version is not installed at $editor. This script will not install or upgrade Unity."
}

$artifactsRoot = Join-Path $projectRoot "Artifacts"
$logsRoot = Join-Path $artifactsRoot "Logs"
New-Item -ItemType Directory -Path $logsRoot -Force | Out-Null

if ($Mode -eq "Open") {
    $editorFullPath = [IO.Path]::GetFullPath($editor)
    $runningEditors = @(Get-Process -Name Unity -ErrorAction SilentlyContinue | Where-Object {
        $_.Path -and [IO.Path]::GetFullPath($_.Path) -ieq $editorFullPath
    })
    if ($runningEditors.Count -gt 0) {
        throw "A Unity Editor process is already running. Close it before opening this project to avoid multiple instances competing for the same Library folder."
    }

    $process = Start-Process -FilePath $editor -ArgumentList @("-projectPath", "`"$projectRoot`"") -PassThru
    Write-Host "Started Unity $version for $projectRoot (PID $($process.Id))."
    exit 0
}

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$logFile = Join-Path $logsRoot "$Mode-$timestamp.log"
$commonArgs = @("-projectPath", $projectRoot, "-logFile", $logFile)

switch ($Mode) {
    "Validate" {
        $unityArgs = @("-batchmode", "-nographics", "-quit") + $commonArgs
        Write-Host "Running Unity batch import/compile validation with Unity $version."
    }

    "Test" {
        $resultsFile = Join-Path $logsRoot "Test-$TestPlatform-$timestamp.xml"
        $unityArgs = @("-batchmode", "-nographics", "-runTests", "-testPlatform", $TestPlatform, "-testResults", $resultsFile) + $commonArgs
        Write-Host "Running Unity $TestPlatform tests with Unity $version."
    }

    "Build" {
        $playbackEngines = Join-Path (Split-Path $editor -Parent) "Data/PlaybackEngines"
        $modulePath = switch ($Target) {
            "Windows64" { Join-Path $playbackEngines "WindowsStandaloneSupport" }
            "WebGL" { Join-Path $playbackEngines "WebGLSupport" }
        }
        if (-not (Test-Path -LiteralPath $modulePath)) {
            throw "Unity $version does not have the $Target build support module at $modulePath. Install it through Unity Hub only if you choose to add that module."
        }

        $buildDirectory = Join-Path (Join-Path $artifactsRoot "Builds") $Target
        New-Item -ItemType Directory -Path $buildDirectory -Force | Out-Null
        $buildPath = if ($Target -eq "Windows64") {
            Join-Path $buildDirectory "Rebirth.exe"
        } else {
            Join-Path $buildDirectory "Rebirth"
        }

        $unityArgs = @("-batchmode", "-nographics", "-executeMethod", "RebirthBuild.PerformBuild", "-rebirthBuildTarget", $Target, "-rebirthBuildPath", $buildPath) + $commonArgs
        Write-Host "Building the enabled EditorBuildSettings scenes for $Target with Unity $version."
        Write-Host "Build output: $buildPath"
    }
}

$quotedArgs = $unityArgs | ForEach-Object { '"' + $_.Replace('"', '\"') + '"' }
$unityProcess = Start-Process -FilePath $editor -ArgumentList $quotedArgs -PassThru
if (-not $unityProcess.WaitForExit($TimeoutSeconds * 1000)) {
    Write-Warning "Unity exceeded the $TimeoutSeconds second timeout. Stopping the Unity process started by this command."
    $unityProcess.Kill()
    $unityProcess.WaitForExit()
    $unityExitCode = 124
} else {
    $unityExitCode = $unityProcess.ExitCode
}

Write-Host "Unity exit code: $unityExitCode"
Write-Host "Unity log: $logFile"

if ($Mode -eq "Test") {
    Write-Host "Test results: $resultsFile"
    if (-not (Test-Path -LiteralPath $resultsFile)) {
        Write-Error "Unity did not produce a test results file."
        exit 1
    }

    try {
        [xml]$testResults = Get-Content -LiteralPath $resultsFile -Raw
        $summary = $testResults.SelectSingleNode("//test-run")
        if ($null -eq $summary) {
            Write-Error "Test results XML has no test-run summary."
            exit 1
        }
        Write-Host "Tests: total=$($summary.total), passed=$($summary.passed), failed=$($summary.failed), skipped=$($summary.skipped), inconclusive=$($summary.inconclusive)"
        if ([int]$summary.failed -gt 0) {
            $unityExitCode = 1
        }
    }
    catch {
        Write-Error "Could not parse Unity test results: $_"
        exit 1
    }
}

exit $unityExitCode
