using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollider : MonoBehaviour
{
    // Enemy�� �� ���� �̻� �׿��� �� GateCollider Ȱ��ȭ -> EnemyManaager���� ����
    // ���� ���� �� ���� �� �����Ŵ������� ����� �ϳ�??

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
                Debug.LogError("DungeonManager �ν��Ͻ��� �������� ����");
            }

            gameObject.SetActive(false);  // ���� ����Ʈ ��Ȱ��ȭ �Ǵ� �ı�
        }
    }
}
