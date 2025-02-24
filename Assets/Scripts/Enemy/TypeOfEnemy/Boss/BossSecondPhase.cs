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
                AttackPattern2();
                break;
            case 2:
                AttackPattern3();
                break;
            case 3:
                Heal();
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

    private void AttackPattern2()
    {
        Debug.Log("Attack2: Boss 주변 범위 공격");
        animatorController.SecondAttackPattern2(true);

        // AttackPattern2 로직

        animatorController.SecondAttackPattern2(false);
    }

    private void AttackPattern3()
    {
        Debug.Log("SecondPhase Attack3: 투사체 발사");
        animatorController.SecondAttackPattern3(true);

        // AttackPattern3 로직

        animatorController.SecondAttackPattern3(false);
    }

    private void Heal()
    {
        Debug.Log("SecondPhase: Boss 체력 5% 회복");
        animatorController.SecondHeal(true);
        boss.HP += boss.HP * 0.05f;
        animatorController.SecondHeal(false);
    }
}
