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

    public int PathId => pathId;
    public PathSegment PathSegment => pathSegment;

    public event Action OnNodeUpdate;

    public void Initialize()
    {
        nodeConnection.Initialize(this);
        isBusy = false;
    }

    public void RotateNode()
    {
        if (isBusy)
        {
            return;
        }

        isBusy = true;

        if (isLocked)
        {
            rotationPivot.transform.localScale = Vector3.one * .9f;
            LeanTween.scale(rotationPivot, Vector3.one, duration)
           .setEase(easing)
           .setOnComplete(() =>
           {
               isBusy = false;
           });
        }
        else
        {
            nodeConnection.DisconnectAllPoints();
            rotationPivot.transform.localScale = Vector3.one * 1.2f;
            LeanTween.scale(rotationPivot, Vector3.one, duration)
                .setEase(easing);

            float angle = rotationPivot.transform.rotation.eulerAngles.z + rotationAngle;
            LeanTween.rotateZ(rotationPivot, angle, duration)
                .setEase(easing)
                .setOnComplete(() =>
                {
                    nodeConnection.CheckAllPointsForConnection();
                    isBusy = false;
                    OnNodeUpdate?.Invoke();
                });
        }
    }

    public List<Node> GetConnectedNodes()
    {
        return nodeConnection.ConnectedNodes
            .Select(n => n.FromNode)
            .ToList();
    }
}