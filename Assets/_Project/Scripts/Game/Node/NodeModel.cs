using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Node))]
public class NodeModel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Image background;
    [SerializeField] Image pathImage;
    [SerializeField] Image edgeImage;

    [Header("Interaction")]
    [SerializeField] float interactionColorAnimationDuration = .1f;
    [SerializeField] Color bgInteractionColor = Color.grey;
    [SerializeField] float interactionScaleAnimationDuration = 1f;
    [SerializeField] LeanTweenType scaleEasing = LeanTweenType.easeOutElastic;

    [Header("Path")]
    [SerializeField] float doneAnimationDuration = .4f;
    [SerializeField] Color pathDoneColor = Color.cyan;
    [SerializeField] Color pathUndoneColor = Color.white;
    [SerializeField] Color bgDoneColor = Color.grey;

    Node node;
    Color storedBgColor;

    void Start()
    {
        pathImage.color = pathUndoneColor;
        storedBgColor = background.color;
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
        Handheld.Vibrate();
        LeanTween.cancel(background.gameObject);
        background.color = bgInteractionColor;
        LeanTween.value(0, 1, interactionColorAnimationDuration)
            .setOnUpdate((float value) =>
            {
                background.color = Color.Lerp(bgInteractionColor, storedBgColor, value);
            });

        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one * 1.1f;
        LeanTween.scale(gameObject, Vector3.one, interactionScaleAnimationDuration).setEase(scaleEasing);
    }

    private void Node_OnNodePathDone()
    {
        LeanTween.cancel(background.gameObject);
        background.color = bgDoneColor;
        LeanTween.value(0, 1, doneAnimationDuration)
            .setOnUpdate((float value) =>
            {
                background.color = Color.Lerp(bgDoneColor, storedBgColor, value);
            });


        pathImage.color = pathDoneColor;

        if (edgeImage != null)
        {
            edgeImage.color = pathDoneColor;
        }
    }

    private void Node_OnNodePathUndone()
    {
        pathImage.color = pathUndoneColor;

        if (edgeImage != null)
        {
            edgeImage.color = pathUndoneColor;
        }
    }
}
