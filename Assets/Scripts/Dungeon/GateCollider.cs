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
                DungeonManager.Instance.passedNum++;  // �÷��̾ ����Ʈ�� ����� Ƚ�� ����
                DungeonManager.Instance.CheckDungeonClear();
            }
            else
            {
                Debug.LogError("DungeonManager �ν��Ͻ��� �������� ����");
            }

            gameObject.SetActive(false);  // ���� ����Ʈ ��Ȱ��ȭ
        }
    }
}
