using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image fill;
    public TextMeshProUGUI text;

    public void SetProgress(float progress, float fullAmount, string formatString = ".0")
    {
        fill.fillAmount = Mathf.Clamp01(progress / fullAmount);
        text.text = $"{progress.ToString(formatString)}/{fullAmount.ToString(formatString)}";
    }
}