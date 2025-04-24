using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class JenkinsBuild
{
    public static void BuildAndroid()
    {
        string buildPath = "Builds/Android";
        string[] scenes = {
            "Assets/_Common/MAIN_MainScene.unity",
            "Assets/Application/MainScreens/Scenes/MAIN_StartScreen.unity",
            "Assets/Application/MainScreens/Scenes/MAIN_Menu_Principal.unity",
            "Assets/Application/MainScreens/Scenes/MAIN_Parametres.unity",
            "Assets/Application/MainScreens/Scenes/MAIN_TransitionMinigames.unity",
            "Assets/Application/MainScreens/Scenes/MAIN_Boutique.unity",
            "Assets/Minigames/Hit The Road/MAIN Hit the road.unity" 
        };

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


