using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image fill;
    public TextMeshProUGUI text;

    public void ChangeProgress(float progress, float fullAmount)
    {
        fill.fillAmount = Mathf.Clamp01(progress / fullAmount);
        text.text = $"{progress.ToString(".0")}/{fullAmount.ToString(".0")}";
    }
}