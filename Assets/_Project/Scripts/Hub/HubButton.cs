using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HubButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject completedIcon;
    [SerializeField] GameObject notCompletedIcon;
    [SerializeField] GameObject lockedIcon;
    StageSO stageInfo;

    public void SetHubButton(StageSO stageInfo, bool isLocked, bool hasCompleted)
    {
        this.stageInfo = stageInfo;
        nameText.text = stageInfo.DisplayName;
        lockedIcon.SetActive(isLocked);
        completedIcon.SetActive(hasCompleted && !isLocked);
        notCompletedIcon.SetActive(!hasCompleted && !isLocked);
        button.interactable = !isLocked;

        if (!isLocked)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        GameManager.Instance.LoadStage(stageInfo);
    }
}