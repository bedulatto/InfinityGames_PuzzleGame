using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] ProgressSO progress;
    [SerializeField] int scoreValue = 250;

    public event Action<int> OnScored;

    private void Start()
    {
        NodeManager.OnAllPathCompleted += NodeManager_OnAllPathCompleted;
        NodeManager.OnPathCompleted += NodeManager_OnPathCompleted;
    }

    private void OnDestroy()
    {
        NodeManager.OnAllPathCompleted -= NodeManager_OnAllPathCompleted;
        NodeManager.OnPathCompleted -= NodeManager_OnPathCompleted;
    }

    private void NodeManager_OnAllPathCompleted()
    {
        progress.SetLastStage(progress.LastStage + 1);
    }

    private void NodeManager_OnPathCompleted(int pathId)
    {
        TriggerScore();
    }

    [ContextMenu("Trigger")]
    public void TriggerScore()
    {
        progress.AddExperience(scoreValue);
        OnScored?.Invoke(scoreValue);
    }
}