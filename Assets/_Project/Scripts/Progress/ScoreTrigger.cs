using System;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    [SerializeField] ProgressSO progress;
    [SerializeField] int scoreValue = 250;

    public event Action<int> OnScored;

    private void Start()
    {
        NodeManager.OnPathCompleted += NodeManager_OnPathCompleted;
    }

    private void OnDestroy()
    {
        NodeManager.OnPathCompleted -= NodeManager_OnPathCompleted;
    }

    private void NodeManager_OnPathCompleted(int pathId)
    {
        progress.AddExperience(scoreValue);
        OnScored?.Invoke(scoreValue);
    }
}