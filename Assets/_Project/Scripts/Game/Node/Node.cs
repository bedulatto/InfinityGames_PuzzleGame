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
    float toRotation;

    public int PathId => pathId;
    public PathSegment PathSegment => pathSegment;

    public event Action OnNodeInteraction;
    public event Action OnNodeRotationStart;
    public event Action OnNodeRotationEnd;
    public event Action OnNodePathDone;
    public event Action OnNodePathUndone;

    public void Initialize()
    {
        toRotation = rotationPivot.transform.rotation.eulerAngles.z;

        if (randomRotationAtStart)
        {
            int randomAngleIndex = UnityEngine.Random.Range(0, possibleStartRotationList.Count);
            toRotation = possibleStartRotationList[randomAngleIndex];
            rotationPivot.transform.rotation = Quaternion.Euler(Vector3.forward * toRotation);
        }

        nodeConnection.Initialize(this);
        isBusy = false;
    }

    public void InteractWithNode()
    {
        OnNodeInteraction?.Invoke();
        RotateNode();
    }

    public void RotateNode()
    {
        if (isLocked)
        {
            return;
        }

        if (isBusy)
        {
            LeanTween.cancel(rotationPivot);
            rotationPivot.transform.rotation = Quaternion.Euler(Vector3.forward * toRotation);
        }

        isBusy = true;
        toRotation += rotationAngle;
        
        if (toRotation > 360)
        {
            toRotation -= 360;
        }

        nodeConnection.DisconnectAllPoints();
        OnNodeRotationStart?.Invoke();
        LeanTween.rotateZ(rotationPivot, toRotation, duration)
            .setEase(easing)
            .setOnComplete(OnRotationComplete);
    }

    private void OnRotationComplete()
    {
        nodeConnection.CheckAllPointsForConnection();
        isBusy = false;
        OnNodeRotationEnd?.Invoke();
    }

    public List<Node> GetConnectedNodes()
    {
        return nodeConnection.ConnectedNodes
            .Select(n => n.FromNode)
            .ToList();
    }

    public void SetPathDone()
    {
        OnNodePathDone?.Invoke();
    }

    public void SetPathUndone()
    {
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