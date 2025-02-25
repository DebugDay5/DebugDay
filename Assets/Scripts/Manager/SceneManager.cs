using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;

    public static SceneManager Instance { get => instance; }

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

    public void ChangeLobbyScene() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void ChangeDungeonScene() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DungeonScene_KHY");
    }

}
