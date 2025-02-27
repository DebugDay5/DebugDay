using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;

    public static SceneManager Instance { get => instance; }

    //public int TestNum = 0;

    private void Awake()
    {
        // 이미 인스턴스가 존재하고 현재 오브젝트가 아니라면
        if (instance != null && instance != this)
        {
            // 중복된 인스턴스 제거
            Destroy(gameObject);
            return;
        }

        // 인스턴스가 없으면 현재 인스턴스 설정
        instance = this;
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.L))
        {
            TestNum++;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ChangeLobbyScene();
        }
        */
    }

    public void ChangeLobbyScene() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void ChangeDungeonScene() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DungeonScene");
    }

}
