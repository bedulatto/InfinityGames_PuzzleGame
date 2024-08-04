using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Progress", menuName = "Puzzle/Progress")]
public class ProgressSO : ScriptableObject
{
    public int LastStageBeat = 0;
    public int LastStagePlayed = 0;
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
    public event Action OnStageBeat;
    public event Action OnStagePlayed;

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

    public void RegisterStageBeat(int stage)
    {
        LastStagePlayed = stage;
        OnStagePlayed?.Invoke();

        if (stage > LastStageBeat)
        {
            LastStageBeat = stage;
            OnStageBeat?.Invoke();
        }
    }

    public void ResetProgress()
    {
        LastStageBeat = 0;
        LastStagePlayed = 0;
        CurrentExperience = 0;
        TotalExperience = 0;
        CurrentLevel = 0;
    }
}
