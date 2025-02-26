using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayeSound 
{
    Walk,
    Shoot,
    Die
}

public class SoundManager : MonoBehaviour
{
    [Header("===Player Sound===")]
    [SerializeField] AudioClip[] playerClip;

    [Header("===Background Sound===")]
    [SerializeField] AudioClip[] backGroundClip;

    [Header("===Enemy Move Sound==")]
    [SerializeField] AudioClip[] enemyMoveClip;

    public void PlayPlayerSound(AudioSource playerSource , PlayeSound sound) 
    {
        // play : source에 미리 clip이 할당되어있어야 함, 재생중인 소리 중지 시킨 후 재생

        // PlayOnShot : 재생시킬 clip 매개변수로, 재생중지 하지 않고 같이 재생
        playerSource.PlayOneShot(playerClip[(int)sound]);
    }

}
