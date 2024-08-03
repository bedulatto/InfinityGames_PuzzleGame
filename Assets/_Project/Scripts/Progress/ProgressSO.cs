using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Progress", menuName = "Puzzle/Progress")]
public class ProgressSO : ScriptableObject
{
    public int LastStage = 0;
    public int CurrentExperience = 0;
    public int TotalExperience = 0;
    public int CurrentLevel = 0;

    public float ExperienceRatio
    {
        get
        {
            return (float)CurrentExperience / ProgressConstants.LevelThreshold;
        }
    }

    public event Action OnExperienceIncreased;
    public event Action OnLastStageChanged;

    public void AddExperience(int xp)
    {
        CurrentExperience += xp;
        TotalExperience += xp;
        LevelUp();
        OnExperienceIncreased?.Invoke();
    }

    private void LevelUp()
    {
        if (CurrentExperience >= ProgressConstants.LevelThreshold)
        {
            CurrentLevel++;
            var remainingXp = CurrentExperience - ProgressConstants.LevelThreshold;
            CurrentExperience = remainingXp;
            LevelUp();
        }
    }

    public void SetLastStage(int stage)
    {
        LastStage = stage;
        OnLastStageChanged?.Invoke();
    }
}
