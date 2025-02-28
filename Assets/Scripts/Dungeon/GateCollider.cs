using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GateCollider : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public bool isStartMap = false;
    private Collider2D coll;

    private void Start()
    {
        DungeonManager.Instance.currentGate = this;
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if (DungeonManager.Instance != null && DungeonManager.Instance.passedNum > 0)
            {
                PlayStageTransition(); // �ִϸ��̼� ���� �� ���� ���������� �̵�
            }
            else
            {
            Debug.LogError("DungeonManager �ν��Ͻ��� �������� �ʰų� ���� ��Ŭ����");
            }

        }
    }

    public void OpenGate()
    {
        Debug.Log("������");
        gameObject.SetActive(true);
        animator.SetTrigger("Open");  // ���� ������ �ִϸ��̼�
        coll.enabled = true;
    }

    private void PlayStageTransition()  // �ִϸ��̼� ���� �� ���� ���������� �̵�
    {
        Debug.Log("�ؽ�Ʈ��������");
        DungeonManager.Instance.AdvanceToNextStage(); // ���� ���������� �̵�
        gameObject.SetActive(false);  // �� ��Ȱ��ȭ
    
    }

}
