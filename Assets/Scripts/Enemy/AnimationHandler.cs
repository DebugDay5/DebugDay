using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");
    private static readonly int IsHit = Animator.StringToHash("IsHit");
    private static readonly int IsDie = Animator.StringToHash("IsDie");

    protected Animator animator;
    private System.Action attackCompleteCallback; // 공격 애니메이션 완료 후 콜백 함수 호출

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>(); // 애니메이터 변수 할당
    }

    public void Move(bool isMoving) // 값을 할당 받아옴
    {
        animator.SetBool(IsMove, isMoving); // 받아온 값을 IsMove에 설정
    }

    public void Attack(System.Action onComplete)
    {
        animator.SetBool(IsAttack, true);
        attackCompleteCallback = onComplete; // 콜백 함수 저장
        StartCoroutine(DisableAttackBool()); // 애니메이션이 끝난 후 상태 초기화
    }

    public void Hit()
    {
        animator.SetBool(IsHit, true);
    }

    public void Die()
    {
        animator.SetBool(IsDie, true);
    }

    private IEnumerator DisableAttackBool()
    {
        yield return new WaitForSeconds(0.8f); // 애니메이션의 길이에 맞춘 값
        animator.SetBool(IsAttack, false);
        attackCompleteCallback?.Invoke(); // 공격 완료 후 콜백 함수 호출
    }
}
