using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] ProgressSO progress;

    [Header("Components")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image progressBarFill;
    [SerializeField] TextMeshProUGUI totalXpText;
    [SerializeField] TextMeshProUGUI currentXpText;

    [Header("Show And Hide Animation")]
    [SerializeField] float enterDuration = 0.2f;
    [SerializeField] float leaveDuration = 0.2f;
    [SerializeField] float showDuration = 1f;
    [SerializeField] float updateUiDuration = 1;

    private float lastExperienceRatioValue = 0;
    private int lastTotalExperienceValue = 0;
    private int lastCurrentExperienceValue = 0;

    private void Start()
    {
        canvasGroup.alpha = 0;
        UpdateUI();
        progress.OnExperienceIncreased += Progress_OnExperienceIncreased;
    }

    private void OnDestroy()
    {
        progress.OnExperienceIncreased -= Progress_OnExperienceIncreased;
    }

    private void Progress_OnExperienceIncreased()
    {
        UpdateUI();
        ShowAndHideAnimation();
    }

    private void UpdateUI()
    {
        float fillAmount = progress.ExperienceRatio - lastExperienceRatioValue;

        if (lastExperienceRatioValue > progress.ExperienceRatio)
        {
            fillAmount = Mathf.Abs(lastExperienceRatioValue - 1);
        }

        LeanTween.value(0, fillAmount, updateUiDuration)
            .setDelay(enterDuration)
            .setOnUpdate((float value) =>
            {
                float newFill = lastExperienceRatioValue + value;

                if (newFill >= 1)
                {
                    newFill = newFill - 1;
                }

                progressBarFill.fillAmount = newFill;
            })
            .setOnComplete(() =>
            {
                progressBarFill.fillAmount = progress.ExperienceRatio;
                lastExperienceRatioValue = progress.ExperienceRatio;
            });

        LeanTween.value(lastCurrentExperienceValue, progress.CurrentExperience, updateUiDuration)
          .setDelay(enterDuration)
          .setOnUpdate((float value) =>
          {
              currentXpText.text = $"{Mathf.RoundToInt(value)}/{progress.LevelThreshold}";
          })
          .setOnComplete(() =>
          {
              currentXpText.text = $"{progress.CurrentExperience}/{progress.LevelThreshold}";
              lastCurrentExperienceValue = progress.CurrentExperience;
          });

        LeanTween.value(lastTotalExperienceValue, progress.TotalExperience, updateUiDuration)
           .setDelay(enterDuration)
           .setOnUpdate((float value) =>
           {
               totalXpText.text = $"{Mathf.RoundToInt(value)}";
           })
           .setOnComplete(() =>
           {
               totalXpText.text = $"{progress.TotalExperience}";
               lastTotalExperienceValue = progress.TotalExperience;
           });
    }

    private void ShowAndHideAnimation()
    {
        LeanTween.cancel(canvasGroup.gameObject);
        canvasGroup.transform.localScale = new Vector3(1, 0, 1);
        LeanTween.alphaCanvas(canvasGroup, 1, enterDuration);
        LeanTween.scaleY(canvasGroup.gameObject, 1, enterDuration)
            .setEaseOutElastic();
        LeanTween.alphaCanvas(canvasGroup, 0, leaveDuration)
            .setDelay(showDuration);
        LeanTween.scaleY(canvasGroup.gameObject, 0, enterDuration)
            .setEaseInCirc()
            .setDelay(showDuration);
    }
}