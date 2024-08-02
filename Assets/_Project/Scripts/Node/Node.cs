using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Node : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] GameObject rotationPivot;
    [SerializeField] float rotationAngle = 45;
    [SerializeField] float duration = 0.2f;
    [SerializeField] LeanTweenType easing = LeanTweenType.easeInOutSine;
    [SerializeField] bool isLocked;

    [Header("Path")]
    [SerializeField] NodeConnection nodeConnection;
    [SerializeField] int pathId;
    [SerializeField] PathSegment pathSegment = PathSegment.Pathway;

    bool isBusy;
    bool isDone;

    public int PathId => pathId;
    public PathSegment PathSegment => pathSegment;

    public event Action OnNodeInteraction;
    public event Action OnNodeRotationStart;
    public event Action OnNodeRotationEnd;
    public event Action OnNodePathDone;
    public event Action OnNodePathUndone;

    public void Initialize()
    {
        nodeConnection.Initialize(this);
        isBusy = false;
    }

    public void RotateNode()
    {
        OnNodeInteraction?.Invoke();

        if (isBusy)
        {
            return;
        }

        isBusy = true;
        OnNodeRotationStart?.Invoke();

        if (!isLocked)
        {
            nodeConnection.DisconnectAllPoints();
            float angle = rotationPivot.transform.rotation.eulerAngles.z + rotationAngle;
            LeanTween.rotateZ(rotationPivot, angle, duration)
                .setEase(easing)
                .setOnComplete(() =>
                {
                    nodeConnection.CheckAllPointsForConnection();
                    isBusy = false;
                    OnNodeRotationEnd?.Invoke();
                });
        }
    }

    public List<Node> GetConnectedNodes()
    {
        return nodeConnection.ConnectedNodes
            .Select(n => n.FromNode)
            .ToList();
    }

    public void SetPathDone()
    {
        isDone = true;
        OnNodePathDone?.Invoke();
    }

    public void SetPathUndone()
    {
        isDone = false;
        OnNodePathUndone?.Invoke();
    }
}