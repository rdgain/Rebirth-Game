using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>Command-line build entry point used by tools/unity.ps1.</summary>
public static class RebirthBuild
{
    /// <summary>Builds enabled scenes for the requested installed target without changing project build settings.</summary>
    public static void PerformBuild()
    {
        string targetName = GetArgument("-rebirthBuildTarget");
        string outputPath = GetArgument("-rebirthBuildPath");
        BuildTarget target;
        string expectedExtension;

        switch (targetName)
        {
            case "Windows64":
                target = BuildTarget.StandaloneWindows64;
                expectedExtension = ".exe";
                break;
            case "WebGL":
                target = BuildTarget.WebGL;
                expectedExtension = string.Empty;
                break;
            default:
                throw new ArgumentException("Unsupported Rebirth build target: " + targetName);
        }

        if (string.IsNullOrWhiteSpace(outputPath))
            throw new ArgumentException("Missing -rebirthBuildPath argument.");

        if (!string.IsNullOrEmpty(expectedExtension) && !outputPath.EndsWith(expectedExtension, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("The " + targetName + " output path must end with " + expectedExtension + ".");

        EditorBuildSettingsScene[] enabledScenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).ToArray();
        if (enabledScenes.Length == 0)
            throw new InvalidOperationException("EditorBuildSettings contains no enabled scenes.");

        foreach (EditorBuildSettingsScene scene in enabledScenes)
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), scene.path)))
                throw new FileNotFoundException("An enabled build scene does not exist.", scene.path);
        }

        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = enabledScenes.Select(scene => scene.path).ToArray(),
            locationPathName = outputPath,
            target = target,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;
        Debug.Log("Rebirth build result: " + summary.result + "; output=" + outputPath);
        if (summary.result != BuildResult.Succeeded)
            Debug.LogError("Rebirth build failed: " + summary.result + "; errors=" + summary.totalErrors + "; warnings=" + summary.totalWarnings);

        EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
    }

    private static string GetArgument(string name)
    {
        string[] arguments = Environment.GetCommandLineArgs();
        int index = Array.IndexOf(arguments, name);
        return index >= 0 && index + 1 < arguments.Length ? arguments[index + 1] : null;
    }
}
