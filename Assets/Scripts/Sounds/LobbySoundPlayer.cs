using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SoundManager.Instance.PlaySounds(audioSource , BGM.Lobby);
    }

}
