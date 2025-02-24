using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecondPhase : BossState
{
    private BossAnimatorController animatorController;
    public BossSecondPhase(BossEnemy boss) : base(boss) { }
    public override void UpdateState()
    {
        // AI 업데이트 로직
    }
    
    public override void Attack()
    {
        int pattern = Random.Range(0, 4);
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
            case 3:
                boss.StartCoroutine(Heal());
                break;
        }
    }

    private IEnumerator AttackPattern1()
    {
        Debug.Log("SecondPhase Attack1: 랜덤한 5곳에 범위 공격");
        animatorController.SecondAttackPattern1(true);

        yield return new WaitForSeconds(2f);

        // AttackPattern1 로직

        animatorController.SecondAttackPattern1(false);
    }

    private IEnumerator AttackPattern2()
    {
        Debug.Log("Attack2: Boss 주변 범위 공격");
        animatorController.SecondAttackPattern2(true);

        yield return new WaitForSeconds(2f);

        // AttackPattern2 로직

        animatorController.SecondAttackPattern2(false);
    }

    private IEnumerator AttackPattern3()
    {
        Debug.Log("SecondPhase Attack3: 투사체 발사");
        animatorController.SecondAttackPattern3(true);

        yield return new WaitForSeconds(2f);

        // AttackPattern3 로직

        animatorController.SecondAttackPattern3(false);
    }

    private IEnumerator Heal()
    {
        Debug.Log("SecondPhase: Boss 체력 5% 회복");
        animatorController.SecondHeal(true);

        yield return new WaitForSeconds(2f);

        boss.HP += boss.HP * 0.05f;
        
        animatorController.SecondHeal(false);
    }
}
