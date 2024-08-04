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
    [SerializeField] Animator animator;

    [Header("Show And Hide Animation")]
    [SerializeField] LeanTweenType easingType = LeanTweenType.easeInOutBounce;
    [SerializeField] float enterDuration = 0.2f;
    [SerializeField] float updateUiDuration = 1;

    private float lastExperienceRatioValue = 0;
    private int lastTotalExperienceValue = 0;
    private int lastCurrentExperienceValue = 0;

    private void Start()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        lastExperienceRatioValue = progress.ExperienceRatio;
        lastTotalExperienceValue = progress.TotalExperience;
        lastCurrentExperienceValue = progress.CurrentExperience;
        UpdateUI();
        progress.OnExperienceIncreased += Progress_OnExperienceIncreased;
    }

    private void OnDestroy()
    {
        progress.OnExperienceIncreased -= Progress_OnExperienceIncreased;
    }

    private void Progress_OnExperienceIncreased()
    {
        ShowAnimation();
    }

    private void UpdateUI()
    {
        currentXpText.text = $"{progress.CurrentExperience} / {ProgressConstants.LevelThreshold}";
        totalXpText.text = $"Total XP {progress.TotalExperience}";
        currentLevelText.text = $"{progress.CurrentLevel + 1}";
        progressBarFill.fillAmount = progress.ExperienceRatio;
    }

    private void UpdateUIAnimation()
    {
        float fillAmount = progress.ExperienceRatio - lastExperienceRatioValue;

        if (lastExperienceRatioValue > progress.ExperienceRatio)
        {
            fillAmount = Mathf.Abs(lastExperienceRatioValue - 1);
        }

        LeanTween.value(0, fillAmount, updateUiDuration)
            .setEaseOutCubic()
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
                currentLevelText.text = $"{progress.CurrentLevel + 1}";
                progressBarFill.fillAmount = progress.ExperienceRatio;
                lastExperienceRatioValue = progress.ExperienceRatio;
            });

        LeanTween.value(lastCurrentExperienceValue, progress.CurrentExperience, updateUiDuration)
            .setEaseOutCubic()
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
            .setEaseOutExpo()
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

    private void ShowAnimation()
    {
        Handheld.Vibrate();
        showAudio.Play();
        animator.SetTrigger("show");

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
}