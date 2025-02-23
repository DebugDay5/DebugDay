using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstPhase : BossState
{
    private BossAnimatorController animatorController;
    public BossFirstPhase(BossEnemy boss) : base(boss) { }
    public override void UpdateState()
    {
        // AI 업데이트 로직
    }
    
    public override void Attack()
    {
        int pattern = Random.Range(0, 3);
        switch (pattern)
        {
            case 0:
                boss.StartCoroutine(AttackPattern1());
                break;
            case 1:
                AttackPattern2();
                break;
            case 2:
                AttackPattern3();
                break;
        }
    }

    private IEnumerator AttackPattern1()
    {
        Debug.Log("Attack1: 2초 후 범위 공격");
        animatorController.SetAttackAnimation(true);

        yield return new WaitForSeconds(2f);

        // AttackPattern1 로직

        animatorController.SetAttackAnimation(false);
    }

    private void AttackPattern2()
    {
        Debug.Log("Attack2: Boss 주변 범위 공격");
        animatorController.SetAttackAnimation(true);

        // AttackPattern2 로직

        animatorController.SetAttackAnimation(false);
    }

    private void AttackPattern3()
    {
        Debug.Log("Attack3: 근접 공격");
        animatorController.SetAttackAnimation(true);

        // AttackPattern3 로직

        animatorController.SetAttackAnimation(false);
    }
}
