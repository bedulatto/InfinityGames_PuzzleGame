using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Node))]
public class NodeModel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject background;
    [SerializeField] Image pathImage;

    [Header("Interaction")]
    [SerializeField] float interactionAnimationDuration = .4f;
    [SerializeField] LeanTweenType easing = LeanTweenType.easeOutElastic;

    [Header("Path")]
    [SerializeField] Color pathDoneColor = Color.cyan;
    [SerializeField] Color pathUndoneColor = Color.white;

    Node node;

    void Start()
    {
        node = GetComponent<Node>();
        node.OnNodeInteraction += Node_OnNodeInteraction;
        node.OnNodePathDone += Node_OnNodePathDone;
        node.OnNodePathUndone += Node_OnNodePathUndone;
    }

    private void OnDestroy()
    {
        node.OnNodeInteraction -= Node_OnNodeInteraction;
        node.OnNodePathDone -= Node_OnNodePathDone;
        node.OnNodePathUndone -= Node_OnNodePathUndone;
    }

    private void Node_OnNodeInteraction()
    {
        LeanTween.cancel(background);
        background.transform.localScale = Vector3.one * .9f;
        LeanTween.scale(background, Vector3.one, interactionAnimationDuration).setEase(easing);
    }

    private void Node_OnNodePathDone()
    {
        pathImage.color = pathDoneColor;
    }

    private void Node_OnNodePathUndone()
    {
        pathImage.color = pathUndoneColor;
    }
}
