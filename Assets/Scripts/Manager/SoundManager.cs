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
            DontDestroyOnLoad(gameObject); //  씬 전환 시 object 유지
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
    /// 사용방법
    /// 1. 프리팹에 AudioSource컴포넌트를 할당한다
    /// 2. AudioSource 변수를 만든 후, Awake/Start에서 Audio GetComponent 
    /// 3. SoundManager.Instance.PlayPlayerSound(오디오소스, enum);
    /// </summary>

    // 플레이어 사운드
    public void PlaySounds(AudioSource audioSource , PlayerSound sound) 
    {
        // play : source에 미리 clip이 할당되어있어야 함, 재생중인 소리 중지 시킨 후 재생

        // PlayOnShot : 재생시킬 clip 매개변수로, 재생중지 하지 않고 같이 재생
        try
        {
            audioSource.PlayOneShot(playerClip[(int)sound]);
        }
        catch (Exception ex) { Debug.Log($"SoundManager 오류 : {ex}"); }
    }

    // 배경 사운드
    public void PlaySounds(AudioSource audioSource, BGM sound) 
    {
        try
        {
            audioSource.PlayOneShot(backGroundClip[(int)sound]);
        }
        catch (Exception ex) { Debug.Log($"SoundManager 오류 : {ex}"); }
    }

    // 던전 내부 사운드
    public void PlaySounds(AudioSource audioSource, DungeonSound sound)
    {
        try
        {
            audioSource.PlayOneShot(dungeonSound[(int)sound]);
        }
        catch (Exception ex) { Debug.Log($"SoundManager 오류 : {ex}"); }
    }

    // 사운드가 본인 Manager에서 사용하고 있다면
    public void PlaySounds(AudioSource audioSource, AudioClip sound) 
    {
        try
        {
            audioSource.PlayOneShot(sound);
        }
        catch (Exception ex) { Debug.Log($"SoundManager 오류 : {ex}"); }
    }

}
