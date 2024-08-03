using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NodeConnectionPoint : MonoBehaviour
{
    [SerializeField] Transform connectorTransform;
    [SerializeField] Image connectorImage;
    [SerializeField] float connectorRadius = 10;
    [SerializeField] Color jointDefaultColor = Color.grey;
    [SerializeField] Color jointConnectedColor = Color.cyan;

    public Node FromNode { get; private set; }
    public NodeConnectionPoint ConnectedTo { get; private set; }

    public event Action<NodeConnectionPoint> OnConnectionStart;
    public event Action<NodeConnectionPoint> OnConnectionEnd;

    public void Initialize(Node fromNode)
    {
        FromNode = fromNode;
        var connectionPoint = CastConnectionPoint();

        if (connectionPoint != null)
        {
            StartConnection(connectionPoint);
        }

        connectorImage.color = ConnectedTo == null ? jointDefaultColor : jointConnectedColor;
    }

    public NodeConnectionPoint CastConnectionPoint()
    {
        var colliders = Physics2D.OverlapCircleAll(connectorTransform.position, connectorRadius);
        return colliders
            .Select(c => c.GetComponent<NodeConnectionPoint>())
            .FirstOrDefault(c => c.GetInstanceID() != GetInstanceID());
    }

    public void DisconnectConnection()
    {
        if (ConnectedTo != null)
        {
            Disconnect(ConnectedTo);
        }
    }

    public void CheckForConnection()
    {
        var connectionPoint = CastConnectionPoint();

        if (connectionPoint != null && connectionPoint != ConnectedTo)
        {
            Connect(connectionPoint);
        }
    }

    private void Connect(NodeConnectionPoint connectionPoint)
    {
        StartConnection(connectionPoint);
        connectionPoint.StartConnection(this);
    }

    private void Disconnect(NodeConnectionPoint connectionPoint)
    {
        EndConnection(connectionPoint);
        connectionPoint.EndConnection(this);
    }

    public void StartConnection(NodeConnectionPoint connectionPoint)
    {
        if (connectionPoint == null)
        {
            return;
        }

        ConnectedTo = connectionPoint;
        connectorImage.color = jointConnectedColor;
        OnConnectionStart?.Invoke(ConnectedTo);
    }

    public void EndConnection(NodeConnectionPoint connectionPoint)
    {
        connectorImage.color = jointDefaultColor;

        if (connectionPoint == null || connectionPoint != ConnectedTo)
        {
            return;
        }

        ConnectedTo = null;
        OnConnectionEnd?.Invoke(connectionPoint);
    }

    private void OnDrawGizmos()
    {
        if (connectorTransform == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(connectorTransform.position, connectorRadius);
    }
}
