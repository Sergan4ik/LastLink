using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildMethods
{
    public static List<string> pathToScenes
    {
        get
        {
            var scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(scene.path);
                }
            }
            
            return scenes;
        }
    }
    
    [MenuItem("📦Build/Build server+client")]
    public static void BuildServerAndClient()
    {
        BuildServer();
        BuildClient();
    }
    
    [MenuItem("📦Build/Build server")]
    public static void BuildServer()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(NamedBuildTarget.Server, BuildTarget.StandaloneWindows);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = pathToScenes.ToArray(),
            locationPathName = $"Builds/Servers/V {DateTime.Now:dd_MM}-{DateTime.Now:hh_mm}/soph_server.exe",
            target = BuildTarget.StandaloneWindows64,
            subtarget = (int)StandaloneBuildSubtarget.Server,
        };
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Server build succeeded: " + report.summary.totalSize + " bytes");
        }
        else
        {
            Debug.LogError($"Server build failed. See console for details.");
        }
    }
    
    [MenuItem("📦Build/👌Build client")]
    public static void BuildClient()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(NamedBuildTarget.Standalone, BuildTarget.StandaloneWindows);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = pathToScenes.ToArray(),
            locationPathName = $"Builds/Clients/V {DateTime.Now:dd_MM}-{DateTime.Now:hh_mm}/soph.exe",
            target = BuildTarget.StandaloneWindows64,
            subtarget = (int)StandaloneBuildSubtarget.Player
        };
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Client build succeeded: " + report.summary.totalSize + " bytes");
        }
        else
        {
            Debug.LogError($"Client build failed. See console for details.");
        }
    }
    
    [MenuItem("📦Build/Clear builds")]
    public static void ClearBuilds()
    {
        string clientBuilds = "Builds/Clients";
        string serverBuilds = "Builds/Servers";
        
        if (System.IO.Directory.Exists(clientBuilds))
        {
            System.IO.Directory.Delete(clientBuilds, true);
            Debug.Log("Deleted client builds.");
        }
        
        if (System.IO.Directory.Exists(serverBuilds))
        {
            System.IO.Directory.Delete(serverBuilds, true);
            Debug.Log("Deleted server builds.");
        }
    }
}