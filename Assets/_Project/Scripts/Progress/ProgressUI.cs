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

    private float lastFillAmount = 0;
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
        LeanTween.value(lastFillAmount, progress.ExperienceRatio, 1)
            .setDelay(1)
            .setOnUpdate((float value) =>
            {
                progressBarFill.fillAmount = value;
            })
            .setOnComplete(() =>
            {
                progressBarFill.fillAmount = progress.ExperienceRatio;
                lastFillAmount = progress.ExperienceRatio;
            });

        LeanTween.value(lastTotalExperienceValue, progress.TotalExperience, 1)
            .setDelay(1)
           .setOnUpdate((float value) =>
           {
               totalXpText.text = $"{Mathf.RoundToInt(value)}";
           })
           .setOnComplete(() =>
           {
               totalXpText.text = $"{progress.TotalExperience}";
               lastTotalExperienceValue = progress.TotalExperience;
           });

        LeanTween.value(lastCurrentExperienceValue, progress.CurrentExperience, 1)
            .setDelay(1)
          .setOnUpdate((float value) =>
          {
              currentXpText.text = $"{Mathf.RoundToInt(value)}/{progress.LevelThreshold}";
          })
          .setOnComplete(() =>
          {
              currentXpText.text = $"{progress.CurrentExperience}/{progress.LevelThreshold}";
              lastCurrentExperienceValue = progress.CurrentExperience;
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