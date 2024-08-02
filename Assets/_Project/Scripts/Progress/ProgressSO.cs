using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Progress", menuName = "Puzzle/Progress")]
public class ProgressSO : ScriptableObject
{
    public int LastPlayedStage = 0;
    public int CurrentExperience = 0;
    public int TotalExperience = 0;
    public int CurrentLevel = 0;
    public int LevelThreshold = 1000;

    public float ExperienceRatio
    {
        get
        {
            return (float)CurrentExperience / LevelThreshold;
        }
    }

    public event Action OnExperienceIncreased;
    public event Action OnPlayedStageChanged;

    public void AddExperience(int xp)
    {
        CurrentExperience += xp;
        TotalExperience += xp;
        LevelUp();
        OnExperienceIncreased?.Invoke();
    }

    private void LevelUp()
    {
        if (CurrentExperience >= LevelThreshold)
        {
            CurrentLevel++;
            var remainingXp = CurrentExperience - LevelThreshold;
            CurrentExperience = remainingXp;
            LevelUp();
        }
    }

    public void SetLastPlayedStage(int stage)
    {
        LastPlayedStage = stage;
        OnPlayedStageChanged?.Invoke();
    }
}
