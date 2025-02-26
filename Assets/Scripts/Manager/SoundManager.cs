using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSound 
{
    Shoot,
    Hit,
    Die
}

public enum BGM 
{ 
    Lobby,
    DungeonRoom,
    BossRoom
}

public enum DungeonSound 
{
    EnterDoor,
    Success,
    Fail
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //  �� ��ȯ �� object ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("===Player Sound===")]
    [SerializeField] AudioClip[] playerClip;

    [Header("===Background Sound===")]
    [SerializeField] AudioClip[] backGroundClip;

    [Header("===Dungeon Sound===")]
    [SerializeField] AudioClip[] dungeonSound;

    /// <summary>
    /// �����
    /// 1. �����տ� AudioSource������Ʈ�� �Ҵ��Ѵ�
    /// 2. AudioSource ������ ���� ��, Awake/Start���� Audio GetComponent 
    /// 3. SoundManager.Instance.PlayPlayerSound(������ҽ�, enum);
    /// </summary>

    // �÷��̾� ����
    public void PlaySounds(AudioSource audioSource , PlayerSound sound) 
    {
        // play : source�� �̸� clip�� �Ҵ�Ǿ��־�� ��, ������� �Ҹ� ���� ��Ų �� ���

        // PlayOnShot : �����ų clip �Ű�������, ������� ���� �ʰ� ���� ���
        try
        {
            audioSource.PlayOneShot(playerClip[(int)sound]);
        }
        catch (Exception ex) { Debug.Log($"SoundManager ���� : {ex}"); }
    }

    // ��� ����
    public void PlaySounds(AudioSource audioSource, BGM sound) 
    {
        try
        {
            audioSource.PlayOneShot(backGroundClip[(int)sound]);
        }
        catch (Exception ex) { Debug.Log($"SoundManager ���� : {ex}"); }
    }

    // ���� ���� ����
    public void PlaySounds(AudioSource audioSource, DungeonSound sound)
    {
        try
        {
            audioSource.PlayOneShot(dungeonSound[(int)sound]);
        }
        catch (Exception ex) { Debug.Log($"SoundManager ���� : {ex}"); }
    }

    // ���尡 ���� Manager���� ����ϰ� �ִٸ�
    public void PlaySounds(AudioSource audioSource, AudioClip sound) 
    {
        try
        {
            audioSource.PlayOneShot(sound);
        }
        catch (Exception ex) { Debug.Log($"SoundManager ���� : {ex}"); }
    }

}
