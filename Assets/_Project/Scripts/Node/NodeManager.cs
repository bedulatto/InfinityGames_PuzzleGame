using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private List<Node> allNodes;
    private List<Node> pathStartList;

    private IEnumerator Start()
    {
        allNodes = GetComponentsInChildren<Node>().ToList();
        pathStartList = allNodes.Where(n => n.PathSegment == PathSegment.Start).ToList();

        yield return null;

        foreach (var node in allNodes)
        {
            node.OnNodeUpdate += Node_OnNodeUpdate;
            node.Initialize();
        }
    }

    private void OnDestroy()
    {
        foreach (var node in allNodes)
        {
            node.OnNodeUpdate -= Node_OnNodeUpdate;
        }
    }

    private void Node_OnNodeUpdate()
    {
        CheckPaths();
    }

    public void CheckPaths()
    {
        foreach (var pathStart in pathStartList)
        {
            if (CanReachEnd(pathStart))
            {
                Debug.Log("end reached");
            }
        }
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
