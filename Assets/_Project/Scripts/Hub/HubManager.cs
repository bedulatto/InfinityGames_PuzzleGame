using System;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField] ProgressSO progress;
    [SerializeField] HubButton hubButtonPrefab;
    [SerializeField] Transform hubContentPanel;

    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.StageList.Count; i++)
        {
            var stageInfo = GameManager.Instance.StageList[i];
            bool isLocked = progress.LastStageBeat + 1 < i;
            bool hasCompleted = progress.LastStageBeat >= i;
            var btn = Instantiate(hubButtonPrefab, hubContentPanel);
            btn.SetHubButton(stageInfo, isLocked, hasCompleted);
        }
    }
}
