using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimatorController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAttackAnimation(bool isAttack)
    {
        animator.SetBool("IsAttack", isAttack);
    }

    public void SetTrigger(string IsPhaseTransition)
    {
        animator.SetTrigger(IsPhaseTransition);
    }
}
