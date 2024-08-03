using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    const float defaultInTime = 0.1f;
    const float defaultOutTime = 0.2f;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image loadProgressBar;

    public void EnterTransition(float inTime = defaultInTime)
    {
        UpdateProgress(0);

        LeanTween.alphaCanvas(canvasGroup, 1, inTime);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void LeaveTransition(float outTime = defaultOutTime)
    {
        LeanTween.alphaCanvas(canvasGroup, 0, outTime);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void UpdateProgress(float progress)
    {
        if (loadProgressBar != null)
        {
            loadProgressBar.fillAmount = progress;
        }
    }
}
