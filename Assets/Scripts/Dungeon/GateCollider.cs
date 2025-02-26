using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GateCollider : MonoBehaviour
{
    public static GateCollider Instance {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("Player"))
        {
            if(DungeonManager.Instance != null)
            {
                DungeonManager.Instance.passedNum++;  // 플레이어가 게이트를 통과한 횟수 증가
                DungeonManager.Instance.CheckDungeonClear();
            }
            else
            {
                Debug.LogError("DungeonManager 인스턴스가 존재하지 않음");
            }

            gameObject.SetActive(false);  // 현재 게이트 비활성화
        }
    }
}
