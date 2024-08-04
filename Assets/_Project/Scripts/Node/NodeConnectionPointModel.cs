using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NodeConnectionPoint))]
public class NodeConnectionPointModel : MonoBehaviour
{
    [SerializeField] Image connectorImage;
    [SerializeField] Color jointDefaultColor = Color.grey;
    [SerializeField] Color jointConnectedColor = Color.cyan;
    [SerializeField] float animationDuration = 1;
    private NodeConnectionPoint nodeConnectionPoint;

    private void Start()
    {
        nodeConnectionPoint = GetComponent<NodeConnectionPoint>();
        nodeConnectionPoint.OnConnectionStart += NodeConnectionPoint_OnConnectionStart;
        nodeConnectionPoint.OnConnectionEnd += NodeConnectionPoint_OnConnectionEnd;
        connectorImage.color = nodeConnectionPoint.ConnectedTo == null ? jointDefaultColor : jointConnectedColor;
    }

    private void OnDestroy()
    {
        nodeConnectionPoint.OnConnectionStart -= NodeConnectionPoint_OnConnectionStart;
        nodeConnectionPoint.OnConnectionEnd -= NodeConnectionPoint_OnConnectionEnd;
    }

    private void NodeConnectionPoint_OnConnectionStart(NodeConnectionPoint nodeConnectionPoint)
    {
        LeanTween.cancel(connectorImage.gameObject);
        LeanTween.value(0, 1, animationDuration)
            .setEaseOutCubic()
            .setOnUpdate((float value) =>
            {
                connectorImage.color = Color.Lerp(jointConnectedColor, jointDefaultColor, value);
            });
    }

    private void NodeConnectionPoint_OnConnectionEnd(NodeConnectionPoint nodeConnectionPoint)
    {
        LeanTween.cancel(connectorImage.gameObject);
        connectorImage.color = jointDefaultColor;
    }
}