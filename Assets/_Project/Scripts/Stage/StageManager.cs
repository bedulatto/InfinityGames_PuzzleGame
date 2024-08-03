using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] ProgressSO progress;
    [SerializeField] StageSO stage;
    [SerializeField] float waitToLoadNextStage = 3;

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
        var stageList = GameManager.Instance.StageList;
        int stageIndex = stageList.IndexOf(stage);
        progress.RegisterStageBeat(stageIndex);
        int nextStageIndex = Mathf.Min(progress.LastStagePlayed + 1, stageList.Count - 1);
        var nextStageName = stageList[nextStageIndex].SceneName;
        GameManager.Instance.WaitAndLoadScene(nextStageName, waitToLoadNextStage);
    }

    private void NodeManager_OnPathCompleted(int pathId)
    {
        TriggerScore();
    }

    [ContextMenu("Trigger")]
    public void TriggerScore()
    {
        progress.AddExperience(stage.StageScoreValue);
        OnScored?.Invoke(stage.StageScoreValue);
    }
}