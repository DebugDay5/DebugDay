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
        // �̹� �ν��Ͻ��� �����ϰ� ���� ������Ʈ�� �ƴ϶��
        if (instance != null && instance != this)
        {
            // �ߺ��� �ν��Ͻ� ����
            Destroy(gameObject);
            return;
        }

        // �ν��Ͻ��� ������ ���� �ν��Ͻ� ����
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
