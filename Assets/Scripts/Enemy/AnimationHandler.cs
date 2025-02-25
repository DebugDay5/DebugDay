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

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>(); // 애니메이터 변수 할당
    }

    public void Move(bool isMoving) // 값을 할당 받아옴
    {
        animator.SetBool(IsMove, isMoving); // 받아온 값을 IsMove에 설정
    }

    public void Attack(bool isAttacking)
    {
        animator.SetBool(IsAttack, isAttacking);
    }

    public void Hit()
    {
        animator.SetBool(IsHit, true);
    }

    public void Die()
    {
        animator.SetBool(IsDie, true);
    }

    public float GetAnimationLength(string animationName)
    {
        RuntimeAnimatorController ac = GetComponent<Animator>().runtimeAnimatorController;

        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0.5f;
    }
}
