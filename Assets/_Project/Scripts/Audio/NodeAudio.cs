using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NodeAudio : MonoBehaviour
{
    [SerializeField] Node node;
    [SerializeField] AudioClip interactionAudioClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        node.OnNodeInteraction += Node_OnNodeInteraction;
    }

    private void Node_OnNodeInteraction()
    {
        audioSource.PlayOneShot(interactionAudioClip);
    }
}
