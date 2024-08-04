using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeConnection : MonoBehaviour
{
    List<NodeConnectionPoint> connectionPointList;

    public List<NodeConnectionPoint> ConnectedNodes { get; private set; }

    private void Awake()
    {
        connectionPointList = GetComponentsInChildren<NodeConnectionPoint>().ToList();
    }

    private void OnDestroy()
    {
        foreach (var connectionPoint in connectionPointList)
        {
            connectionPoint.OnConnectionStart -= ConnectionPoint_OnConnectionStart;
            connectionPoint.OnConnectionEnd -= ConnectionPoint_OnConnectionEnd;
        }
    }

    public void Initialize(Node fromNode)
    {
        ConnectedNodes = new List<NodeConnectionPoint>();

        if (connectionPointList.Count == 0)
        {
            Debug.LogWarning($"{name} is missing at least one connection point");
            return;
        }

        foreach (var connectionPoint in connectionPointList)
        {
            connectionPoint.OnConnectionStart += ConnectionPoint_OnConnectionStart;
            connectionPoint.OnConnectionEnd += ConnectionPoint_OnConnectionEnd;
            connectionPoint.Initialize(fromNode);
        }
    }

    private void ConnectionPoint_OnConnectionEnd(NodeConnectionPoint connectionPoint)
    {
        ConnectedNodes.Remove(connectionPoint);
    }

    private void ConnectionPoint_OnConnectionStart(NodeConnectionPoint connectionPoint)
    {
        ConnectedNodes.Add(connectionPoint);
    }

    public void CheckAllPointsForConnection()
    {
        foreach (var connectionPoint in connectionPointList)
        {
            connectionPoint.CheckForConnection();
        }
    }

    public void DisconnectAllPoints()
    {
        foreach (var connectionPoint in connectionPointList)
        {
            connectionPoint.DisconnectConnection();
        }
    }
}