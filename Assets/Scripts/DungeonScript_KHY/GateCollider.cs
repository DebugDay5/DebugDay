using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollider : MonoBehaviour
{
    public int nextStage = 0; // 다음 스테이지 번호

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(DungeonManager.Instance != null)
            {
                DungeonManager.Instance.passedNum++;
                DungeonManager.Instance.AdvanceToNextStage();

            }
            else
            {
                Debug.LogError("DungeonManager 인스턴스가 존재하지 않음");
            }

            gameObject.SetActive(false);  // 현재 게이트 비활성화 또는 파괴
        }
    }
}
