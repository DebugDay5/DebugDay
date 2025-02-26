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
        // play : source�� �̸� clip�� �Ҵ�Ǿ��־�� ��, ������� �Ҹ� ���� ��Ų �� ���

        // PlayOnShot : �����ų clip �Ű�������, ������� ���� �ʰ� ���� ���
        playerSource.PlayOneShot(playerClip[(int)sound]);
    }

}
