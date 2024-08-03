using System;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField] ProgressSO progress;
    [SerializeField] HubButton hubButtonPrefab;
    [SerializeField] Transform hubContentPanel;

    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.StageInfoList.Count; i++)
        {
            var stageInfo = GameManager.Instance.StageInfoList[i];
            bool isLocked = progress.LastStage < i;
            bool hasCompleted = progress.LastStage > i;
            var btn = Instantiate(hubButtonPrefab, hubContentPanel);
            btn.SetHubButton(stageInfo.SceneName, stageInfo.DisplayName, isLocked, hasCompleted);
        }
    }
}
