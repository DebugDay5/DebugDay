using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;  // �̱��� �����Ŵ����� ��ü 1��

    public DungeonSO currentDungeonData;  // ScriptableObject�� �ҷ��� ���

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentMap(DungeonSO dungeonData)
    {
        currentDungeonData = dungeonData;
    }

    // �������� �� �����ϴ� ���

}



    
