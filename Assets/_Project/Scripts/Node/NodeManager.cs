using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private List<Node> allNodes;
    private List<Node> pathStartList;
    private List<int> completedPaths;

    public static event Action<int> OnPathCompleted;
    public static event Action OnAllPathCompleted;

    private IEnumerator Start()
    {
        completedPaths = new List<int>();
        allNodes = GetComponentsInChildren<Node>().ToList();
        pathStartList = allNodes.Where(n => n.PathSegment == PathSegment.Start).ToList();

        yield return null;

        foreach (var node in allNodes)
        {
            node.OnNodeRotationEnd += Node_OnNodeRotationEnd;
            node.Initialize();
        }

        CheckPaths();
    }

    private void OnDestroy()
    {
        foreach (var node in allNodes)
        {
            node.OnNodeRotationEnd -= Node_OnNodeRotationEnd;
        }
    }

    private void Node_OnNodeRotationEnd()
    {
        CheckPaths();
    }

    public void CheckPaths()
    {
        foreach (var pathStart in pathStartList)
        {
            var nodesInPath = GetNodesInPath(pathStart.PathId);
            bool isPathComplete = CanReachEnd(pathStart);

            foreach (var node in nodesInPath)
            {
                if (isPathComplete)
                {
                    if (!completedPaths.Contains(node.PathId))
                    {
                        OnPathCompleted?.Invoke(node.PathId);
                        completedPaths.Add(node.PathId);
                    }

                    node.SetPathDone();
                }
                else
                {
                    node.SetPathUndone();
                }
            }
        }

        if (pathStartList.Count == completedPaths.Count)
        {
            foreach (var node in allNodes)
            {
                node.Lock();
            }

            OnAllPathCompleted?.Invoke();
        }
    }

    public List<Node> GetNodesInPath(int pathId)
    {
        return allNodes.Where(n => n.PathId == pathId).ToList();
    }

    private bool CanReachEnd(Node start)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        openList.Add(start);

        while (openList.Count > 0)
        {
            var node = openList[0];
            openList.Remove(node);

            if (node.PathSegment == PathSegment.End && start.PathId == node.PathId)
            {
                return true;
            }

            if (!closedList.Contains(node))
            {
                foreach (var connectedNode in node.GetConnectedNodes())
                {
                    openList.Add(connectedNode);
                }

                closedList.Add(node);
            }
        }

        return false;
    }
}
