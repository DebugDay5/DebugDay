using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;  // 싱글톤 던전매니저가 전체 1개

    public DungeonSO currentDungeonData;  // ScriptableObject를 불러와 사용

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

    // 랜덤으로 맵 구성하는 기능

}



    
