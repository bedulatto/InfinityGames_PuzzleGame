using UnityEngine;

public class StageTopBar : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float hideDuration = 0.2f;

    private void Start()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        NodeManager.OnAllPathCompleted += NodeManager_OnAllPathCompleted;
    }

    private void OnDestroy()
    {
        NodeManager.OnAllPathCompleted -= NodeManager_OnAllPathCompleted;
    }

    private void NodeManager_OnAllPathCompleted()
    {
        Hide();
    }

    public void Hide()
    {
        LeanTween.cancel(canvasGroup.gameObject);
        LeanTween.alphaCanvas(canvasGroup, 0, hideDuration);
    }
}
