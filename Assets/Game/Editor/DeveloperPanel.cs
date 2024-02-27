using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.GameCore;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public partial class DevelopersPanel : OdinEditorWindow
{
    [TabGroup("Features", "SceneButtons")]
    [ListDrawerSettings(DefaultExpandedState = true, IsReadOnly = true, ShowFoldout = false), ShowInInspector, HideLabel]
    public List<SceneButton> scenes;

    [Button(ButtonSizes.Large)]
    [TabGroup("Features", "SceneButtons")]
    public void RefreshSceneList()
    {
        scenes = EditorBuildSettings.scenes
            // .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToList().ConvertAll(s => new SceneButton() { name = s });
    }

    [Button(ButtonSizes.Large), GUIColor(1, 0.3f, 0.1f), PropertyOrder(-1)]
    [TabGroup("Features", "ConfigManipulations")]
    public static void ResetConfigs()
    {
        if (EditorUtility.DisplayDialog("Reset configs", "Are you sure?", "Yes", "No") == false)
            return;

        GameConfig.ResetConfigs();
    }
    
    [MenuItem("GameTools/Play #&d")]
    [TabGroup("Features", "SceneButtons")]
    public static async void PlayFromPrelaunchScene()
    {
        if (EditorApplication.isPlaying == true)
        {
            EditorApplication.isPlaying = false;
            return;
        }

        var scenePath = EditorSceneManager.GetActiveScene().path;
        Debug.Log($"active scene {scenePath}");
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        EditorSceneManager.OpenScene("Assets/Scenes/LevelTest.unity");
        EditorApplication.isPlaying = true;

        await Task.Delay(500);
        while (EditorApplication.isPlaying) await Task.Yield();

        Debug.Log($"return to scene {scenePath}");
        EditorSceneManager.OpenScene(scenePath);
    }

    
    [MenuItem("Tools/Developer panel")]
    private static void OpenWindow()
    {
        GetWindow<DevelopersPanel>().Show();
    }

    protected override void Initialize()
    {
        base.Initialize();
        RefreshSceneList();
    }
}