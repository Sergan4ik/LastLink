using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[System.Serializable]
public class SceneButton
{
    [HideInInspector] 
    public string name;

    public string GetName  
    {
        get
        {
            Regex r = new Regex(@"(\w+\/)|(\.+)\w+");
            return r.Replace(name, "");
        }
    }

    private Color StateColor()
    {
        return EditorSceneManager.GetActiveScene().name == GetName ? Color.green : GUI.contentColor;
    }

    [Button("@GetName", ButtonSizes.Medium), GUIColor("StateColor"), DisableIf("@UnityEngine.Application.isPlaying==true")]
    [HorizontalGroup("Group")]
    public void OpenScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveOpenScenes();
        }

        EditorSceneManager.OpenScene(name, OpenSceneMode.Single);
    }

    [Button(ButtonSizes.Medium), GUIColor(0.2f, 0.2f, 1f)]
    [HorizontalGroup("Group")]
    public void SelectInAssets()
    {
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(name);
    }
}