using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class JenkinsBuild
{
    public static void BuildAndroid()
    {
        string buildPath = "Builds/Android";
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = $"{buildPath}/Game.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result != BuildResult.Succeeded)
        {
            Debug.LogError("Build failed.");
            EditorApplication.Exit(1);
        }
        else
        {
            Debug.Log("Build succeeded.");
            EditorApplication.Exit(0);
        }
    }
}