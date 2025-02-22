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
    private System.Action attackCompleteCallback;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(bool isMoving)
    {
        animator.SetBool(IsMove, isMoving);
    }

    public void Attack(System.Action onComplete)
    {
        animator.SetBool(IsAttack, true);
        attackCompleteCallback = onComplete;
        StartCoroutine(DisableAttackBool());
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
        yield return new WaitForSeconds(0.8f);
        animator.SetBool(IsAttack, false);
        attackCompleteCallback?.Invoke();
    }
}
