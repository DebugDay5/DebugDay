using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstPhase : BossState
{
    private BossAnimatorController animatorController;
    public BossFirstPhase(BossEnemy boss) : base(boss)
    {
        animatorController = boss.GetComponent<BossAnimatorController>();
    }
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
                boss.StartCoroutine(AttackPattern2());
                break;
            case 2:
                boss.StartCoroutine(AttackPattern3());
                break;
        }
    }

    private IEnumerator AttackPattern1()
    {
        Debug.Log("Attack1: 2초 후 범위 공격");
        animatorController.FirstAttackPattern1(true);

        yield return new WaitForSeconds(2.2f); // 공격 지연 시간

        // AttackPattern1 실행 로직

        animatorController.FirstAttackPattern1(false);
    }

    private IEnumerator AttackPattern2()
    {
        Debug.Log("Attack2: Boss 주변 범위 공격");
        animatorController.FirstAttackPattern2(true);

        yield return new WaitForSeconds(0.5f); // 애니메이션 지속 시간 고려

        // AttackPattern2 실행 로직

        animatorController.FirstAttackPattern2(false);
    }

    private IEnumerator AttackPattern3()
    {
        Debug.Log("Attack3: 근접 공격");
        animatorController.FirstAttackPattern3(true);

        yield return new WaitForSeconds(0.5f); // 애니메이션 지속 시간 고려

        // AttackPattern3 실행 로직

        animatorController.FirstAttackPattern3(false);
    }
}
