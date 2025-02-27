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
        if (boss.isDead) return;

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

    private IEnumerator AttackPattern1() // 투사체 발사
    {
        if (boss.isDead) yield break;
        animatorController.FirstAttackPattern1(true);

        float animationLength = animatorController.GetAnimationLength("Attack1");
        yield return new WaitForSeconds(animationLength * 0.8f);
        if (boss.isDead) yield break;

        GameObject projectile = Object.Instantiate(firstProjectilePrefab, boss.attackPoint.position, Quaternion.identity); // 투사체 생성
        FirstProjectile firstProjectile = projectile.GetComponent<FirstProjectile>(); // Projectile 스크립트 참조
        firstProjectile.GetComponent<FirstProjectile>().SetDirection((boss.player.position - boss.attackPoint.position).normalized); // 투사체 방향 설정

        yield return new WaitForSeconds(animationLength * 0.2f);
        if (boss.isDead) yield break;
        animatorController.FirstAttackPattern1(false);
    }

    private IEnumerator AttackPattern2() // Attack2: Boss 주변 범위 공격
    {
        if (boss.isDead) yield break;
        animatorController.FirstAttackPattern2(true);

        float animationLength = animatorController.GetAnimationLength("Attack2");
        yield return new WaitForSeconds(animationLength);
        if (boss.isDead) yield break;

        float attackRange = 4f;
        Collider2D[] attackPlayers = Physics2D.OverlapCircleAll(boss.transform.position, attackRange);

        foreach (Collider2D player in attackPlayers)
        {
            if (player.CompareTag("Player"))
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                playerController.TakeDamage(30f);
            }
        }

        animatorController.FirstAttackPattern2(false);
    }

    private IEnumerator AttackPattern3() // Attack3: 근접 공격
    {
        if (boss.isDead) yield break;
        animatorController.FirstAttackPattern3(true);

        float animationLength = animatorController.GetAnimationLength("Attack3");
        yield return new WaitForSeconds(animationLength);
        if (boss.isDead) yield break;

        float attackRange = 2f;
        Collider2D[] attackPlayers = Physics2D.OverlapCircleAll(boss.transform.position, attackRange);

        foreach (Collider2D player in attackPlayers)
        {
            if (boss.isDead) yield break;
            if (player.CompareTag("Player"))
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                playerController.TakeDamage(50f);
            }
        }

        animatorController.FirstAttackPattern3(false);
    }
}
