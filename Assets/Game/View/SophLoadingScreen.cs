using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SophLoadingScreen : MonoBehaviour
{
    public Slider progress;

    public TextMeshProUGUI loadingText;
    
    public Coroutine loadingCoro;
    public RectTransform panel;
    
    public static SophLoadingScreen Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Button]
    public void Test()
    {
        LoadScene("LevelTest");
    }
    
    public async Task LoadScene(string scene)
    {
        panel.gameObject.SetActive(true);
        
        var prevScene = SceneManager.GetActiveScene();
        var op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        op.allowSceneActivation = true;
        
        float endLoadAt = 0.6f;
        await ShowAsyncProgress(op, endAt: endLoadAt, autoShow: false);
        
        var op2 = SceneManager.UnloadSceneAsync(prevScene);

        await ShowAsyncProgress(op2, endLoadAt, 1, false);
        
        panel.gameObject.SetActive(false);
    }

    public async Task ShowAsyncProgress(AsyncOperation operation, float startFrom = 0, float endAt = 1, bool autoShow = true)
    {
        if (autoShow)
        {
            panel.gameObject.SetActive(true);
        }
        await Show(operation, startFrom, endAt, 2).ToUniTask(this);
        if (autoShow)
        {
            panel.gameObject.SetActive(false);
        }
    }

    public IEnumerator Show(AsyncOperation op, float startFrom, float endAt, float fakeTime)
    {
        string[] dots = new []{"", ".", "..", "..."};
        float elapsedTime = 0;
        while (op.isDone == false || elapsedTime < fakeTime)
        {
            progress.value = Mathf.Lerp(progress.minValue, progress.maxValue, Mathf.Lerp(startFrom, endAt, op.progress));
            yield return null;
            loadingText.text = $"Loading{dots[(int)(Time.time) % 4]}";
            elapsedTime += op.isDone ? Time.deltaTime : 0;
        }
        
    }
}