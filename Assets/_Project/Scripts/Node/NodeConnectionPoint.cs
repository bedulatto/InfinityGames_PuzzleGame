using System;
using UnityEngine;

public class NodeConnectionPoint : MonoBehaviour
{
    public Node CurrentConnection { get; private set; }
    Node baseNode;

    public event Action<NodeConnectionPoint, Node> OnConnectionEstablished;
    public event Action<NodeConnectionPoint, Node> OnConnectionCeased;

    public void InitializeConnectionPoint(Node baseNode)
    {
        this.baseNode = baseNode;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Node>(out var node) && !baseNode.Equals(node))
        {
            CurrentConnection = node;
            OnConnectionEstablished?.Invoke(this, CurrentConnection);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Node>(out var node) && node == CurrentConnection)
        {
            CurrentConnection = null;
            OnConnectionCeased?.Invoke(this, node);
        }
    }
}
