using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] GameObject objectReference;
    [SerializeField] float rotationAngle = 45;
    [SerializeField] float duration = 0.2f;
    [SerializeField] LeanTweenType easing = LeanTweenType.easeInOutSine;

    [Header("Connection")]
    [SerializeField] List<NodeConnectionPoint> nodeConnectionPointList;

    bool isBusy;

    private void Start()
    {
        foreach (var connection in nodeConnectionPointList)
        {
            connection.InitializeConnectionPoint(this);
            connection.OnConnectionEstablished += Connection_OnConnectionEstablished;
            connection.OnConnectionCeased += Connection_OnConnectionCeased;
        }

        isBusy = false;
    }

    private void OnDestroy()
    {
        foreach (var connection in nodeConnectionPointList)
        {
            connection.OnConnectionEstablished -= Connection_OnConnectionEstablished;
            connection.OnConnectionCeased -= Connection_OnConnectionCeased;
        }
    }

    private void Connection_OnConnectionEstablished(NodeConnectionPoint from, Node to)
    {
        Debug.Log($"{from.name} -> {to.name}");
    }

    private void Connection_OnConnectionCeased(NodeConnectionPoint from, Node to)
    {
        Debug.Log($"{from.name} x {to.name}");
    }

    public void RotateNode()
    {
        if (isBusy)
        {
            return;
        }

        isBusy = true;
        float angle = objectReference.transform.rotation.eulerAngles.z + rotationAngle;
        LeanTween.rotateZ(objectReference, angle, duration)
            .setEase(easing)
            .setOnComplete(() =>
            {
                isBusy = false;
            });
    }
}