using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollider : MonoBehaviour
{
    // Enemy를 몇 마리 이상 죽였을 때 GateCollider 활성화 -> EnemyManaager에서 관리
    // 몬스터 마리 수 세는 건 던전매니저에서 해줘야 하나??

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
