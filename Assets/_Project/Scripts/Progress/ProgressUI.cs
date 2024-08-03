using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] ProgressSO progress;

    [Header("Components")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] Image progressBarFill;
    [SerializeField] TextMeshProUGUI totalXpText;
    [SerializeField] TextMeshProUGUI currentXpText;
    [SerializeField] AudioSource showAudio;

    [Header("Show And Hide Animation")]
    [SerializeField] LeanTweenType easingType = LeanTweenType.easeInOutBounce;
    [SerializeField] float enterDuration = 0.2f;
    [SerializeField] float leaveDuration = 0.2f;
    [SerializeField] float showDuration = 1f;
    [SerializeField] float updateUiDuration = 1;

    private float lastExperienceRatioValue = 0;
    private int lastTotalExperienceValue = 0;
    private int lastCurrentExperienceValue = 0;
    private int lastLevel = 0;

    private void Start()
    {
        canvasGroup.alpha = 0;
        lastExperienceRatioValue = progress.ExperienceRatio;
        lastTotalExperienceValue = progress.TotalExperience;
        lastCurrentExperienceValue = progress.CurrentExperience;
        lastLevel = progress.CurrentLevel;
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
        currentLevelText.text = $"{lastLevel + 1}";
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
                    currentLevelText.text = $"{progress.CurrentLevel + 1}";
                    newFill = newFill - 1;
                }

                progressBarFill.fillAmount = newFill;
            })
            .setOnComplete(() =>
            {
                progressBarFill.fillAmount = progress.ExperienceRatio;
                lastExperienceRatioValue = progress.ExperienceRatio;
                lastLevel = progress.CurrentLevel;
            });

        LeanTween.value(lastCurrentExperienceValue, progress.CurrentExperience, updateUiDuration)
          .setDelay(enterDuration)
          .setOnUpdate((float value) =>
          {
              currentXpText.text = $"{Mathf.RoundToInt(value)} / {ProgressConstants.LevelThreshold}";
          })
          .setOnComplete(() =>
          {
              currentXpText.text = $"{progress.CurrentExperience} / {ProgressConstants.LevelThreshold}";
              lastCurrentExperienceValue = progress.CurrentExperience;
          });

        LeanTween.value(lastTotalExperienceValue, progress.TotalExperience, updateUiDuration)
           .setDelay(enterDuration)
           .setOnUpdate((float value) =>
           {
               totalXpText.text = $"Total XP {Mathf.RoundToInt(value)}";
           })
           .setOnComplete(() =>
           {
               totalXpText.text = $"Total XP {progress.TotalExperience}";
               lastTotalExperienceValue = progress.TotalExperience;
           });
    }

    private void ShowAndHideAnimation()
    {
        showAudio.Play();
        LeanTween.cancel(canvasGroup.gameObject);
        canvasGroup.transform.localScale = new Vector3(1, 0, 1);
        LeanTween.alphaCanvas(canvasGroup, 1, enterDuration);
        LeanTween.scaleY(canvasGroup.gameObject, 1, enterDuration)
            .setEase(easingType);
        LeanTween.alphaCanvas(canvasGroup, 0, leaveDuration)
            .setDelay(showDuration);
        LeanTween.scaleY(canvasGroup.gameObject, 0, enterDuration)
            .setEaseInCirc()
            .setDelay(showDuration);
    }
}