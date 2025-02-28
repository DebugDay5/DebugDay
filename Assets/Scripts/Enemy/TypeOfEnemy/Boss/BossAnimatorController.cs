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

    public void FirstAttackPattern1(bool isAttack)
    {
        animator.SetBool("IsAttack1", isAttack);
    }

    public void FirstAttackPattern2(bool isAttack)
    {
        animator.SetBool("IsAttack2", isAttack);
    }

    public void FirstAttackPattern3(bool isAttack)
    {
        animator.SetBool("IsAttack3", isAttack);
    }

    public void PhaseTransition(bool isPhaseTransition)
    {
        animator.SetBool("IsPhaseTransition",isPhaseTransition);
    }

    public void SecondAttackPattern1(bool isAttack)
    {
        animator.SetBool("IsSecondAttack1", isAttack);
    }

    public void SecondAttackPattern2(bool isAttack)
    {
        animator.SetBool("IsSecondAttack2", isAttack);
    }

    public void SecondAttackPattern3(bool isAttack)
    {
        animator.SetBool("IsSecondAttack3", isAttack);
    }

    public void SecondHeal(bool isHeal)
    {
        animator.SetBool("IsHeal", isHeal);
    }

    public void SecondBreakArmor(bool isBreakArmor)
    {
        animator.SetBool("IsBreakArmor", isBreakArmor);
    }

    public void SecondLast(bool isLast)
    {
        animator.SetBool("IsLast", isLast);
    }
    
    public void BossDie(bool isDie)
    {
        animator.SetBool("IsDie", isDie);
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
