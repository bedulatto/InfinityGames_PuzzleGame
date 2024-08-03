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
    string sceneName;

    public void SetHubButton(string sceneName, string displayName, bool isLocked, bool hasCompleted)
    {
        this.sceneName = sceneName;
        nameText.text = displayName;
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
        GameManager.Instance.LoadScene(sceneName);
    }
}