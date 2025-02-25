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
        yield return new WaitForSeconds(animationLength * 0.8f);

        GameObject projectile = Object.Instantiate(firstProjectilePrefab, boss.attackPoint.position, Quaternion.identity); // 투사체 생성
        FirstProjectile firstProjectile = projectile.GetComponent<FirstProjectile>(); // Projectile 스크립트 참조
        firstProjectile.GetComponent<FirstProjectile>().SetDirection((boss.player.position - boss.attackPoint.position).normalized); // 투사체 방향 설정

        yield return new WaitForSeconds(animationLength * 0.2f);
        animatorController.FirstAttackPattern1(false);
    }

    private IEnumerator AttackPattern2()
    {
        Debug.Log("Attack2: Boss 주변 범위 공격");
        animatorController.FirstAttackPattern2(true);

        float animationLength = animatorController.GetAnimationLength("Attack2");
        yield return new WaitForSeconds(animationLength);

        float attackRadius = 4f;
        Collider2D[] attackPlayers = Physics2D.OverlapCircleAll(boss.transform.position, attackRadius);

        foreach (Collider2D player in attackPlayers)
        {
            if (player.CompareTag("Player"))
            {
                // player.GetComponent<Player>().TakeDamage(30);
            }
        }

        animatorController.FirstAttackPattern2(false);
    }

    private IEnumerator AttackPattern3()
    {
        Debug.Log("Attack3: 근접 공격");
        animatorController.FirstAttackPattern3(true);

        float animationLength = animatorController.GetAnimationLength("Attack3");
        yield return new WaitForSeconds(animationLength);

        float attackRadius = 2f;
        Collider2D[] attackPlayers = Physics2D.OverlapCircleAll(boss.transform.position, attackRadius);

        foreach (Collider2D player in attackPlayers)
        {
            if (player.CompareTag("Player"))
            {
                // player.GetComponent<Player>().TakeDamage(50);
            }
        }

        animatorController.FirstAttackPattern3(false);
    }
}
