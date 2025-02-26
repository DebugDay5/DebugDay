using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GateCollider : MonoBehaviour
{
    //public static GateCollider Instance {  get; private set; }
    [SerializeField]
    private Animator animator;
    public bool isStartMap = false;
    private Collider2D coll;

    private void Start()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        DungeonManager.Instance.currentGate = this;
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("Player"))
        {
            if (!isStartMap)
            {
                if (DungeonManager.Instance != null && DungeonManager.Instance.passedNum > 0)
                {
                    PlayStageTransition(); // 애니메이션 실행 후 다음 스테이지로 이동
                }
                else
                {
                    Debug.LogError("DungeonManager 인스턴스가 존재하지 않거나 던전 미클리어");
                }
            }
            else
            {
                if (DungeonManager.Instance != null && DungeonManager.Instance.passedNum > 0)
                {
                    PlayStageTransition(); // 애니메이션 실행 후 다음 스테이지로 이동
                }
            }
        }
    }

    public void OpenGate()
    {
        Debug.Log("문열어");
        gameObject.SetActive(true);
        animator.SetTrigger("Open");  // 문이 열리는 애니메이션
        coll.enabled = true;
    }

    private void PlayStageTransition()  // 애니메이션 실행 후 다음 스테이지로 이동
    {
        Debug.Log("넥스트스테이지");
        DungeonManager.Instance.AdvanceToNextStage(); // 다음 스테이지로 이동
        gameObject.SetActive(false);  // 문 비활성화
    
    }

}
