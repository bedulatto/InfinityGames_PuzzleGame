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
    [SerializeField] bool randomRotationAtStart;
    [SerializeField] List<float> possibleStartRotationList;

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
        if (randomRotationAtStart)
        {
            int randomAngleIndex = UnityEngine.Random.Range(0, possibleStartRotationList.Count);
            float randomAngle = possibleStartRotationList[randomAngleIndex];
            rotationPivot.transform.Rotate(Vector3.forward, randomAngle);
        }

        nodeConnection.Initialize(this);
        isBusy = false;
    }

    public void InteractWithNode()
    {
        OnNodeInteraction?.Invoke();
        float newAngle = rotationPivot.transform.rotation.eulerAngles.z + rotationAngle;
        RotateNode(newAngle);
    }

    public void RotateNode(float newAngle)
    {
        if (isBusy)
        {
            return;
        }

        isBusy = true;
        OnNodeRotationStart?.Invoke();

        if (!isLocked)
        {
            nodeConnection.DisconnectAllPoints();
            LeanTween.rotateZ(rotationPivot, newAngle, duration)
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

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }
}