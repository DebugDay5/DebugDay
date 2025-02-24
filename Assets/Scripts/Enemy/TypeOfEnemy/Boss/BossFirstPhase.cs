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
        Debug.Log("Attack1: Player를 향한 투사체");
        animatorController.FirstAttackPattern1(true);

        float animationLength = animatorController.GetAnimationLength("Attack1");
        yield return new WaitForSeconds(animationLength);

        GameObject projectile = Object.Instantiate(projectilePrefab, boss.attackPoint.position, Quaternion.identity); // 투사체 생성
        SecondProjectile secondProjectile = projectile.GetComponent<SecondProjectile>(); // Projectile 스크립트 참조
        secondProjectile.GetComponent<SecondProjectile>().SetDirection((boss.player.position - boss.attackPoint.position).normalized); // 투사체 방향설정

        animatorController.FirstAttackPattern1(false);
    }

    private IEnumerator AttackPattern2()
    {
        Debug.Log("Attack2: Boss 주변 범위 공격");
        animatorController.FirstAttackPattern2(true);

        float animationLength = animatorController.GetAnimationLength("Attack2");
        yield return new WaitForSeconds(animationLength);

        // AttackPattern2 실행 로직

        animatorController.FirstAttackPattern2(false);
    }

    private IEnumerator AttackPattern3()
    {
        Debug.Log("Attack3: 근접 공격");
        animatorController.FirstAttackPattern3(true);

        float animationLength = animatorController.GetAnimationLength("Attack3");
        yield return new WaitForSeconds(animationLength);

        // AttackPattern3 실행 로직

        animatorController.FirstAttackPattern3(false);
    }
}
