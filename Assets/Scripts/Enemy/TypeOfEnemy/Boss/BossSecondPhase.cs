using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecondPhase : BossState
{
    private BossAnimatorController animatorController;
    public BossSecondPhase(BossEnemy boss) : base(boss)
    {
        animatorController = boss.GetComponent<BossAnimatorController>();
    }

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

    private IEnumerator AttackPattern1() // 랜덤한 5곳에 범위 공격
    {
        animatorController.SecondAttackPattern1(true);

        float animationLength = animatorController.GetAnimationLength("SecondAttack1");
        yield return new WaitForSeconds(animationLength * 0.5f);

        SpawnRandomStones(5);

        yield return new WaitForSeconds(animationLength * 0.5f);

        animatorController.SecondAttackPattern1(false);
    }

    private IEnumerator AttackPattern2() // Boss 주변 범위 공격
    {
        animatorController.SecondAttackPattern2(true);

        float animationLength = animatorController.GetAnimationLength("SecondAttack2");
        yield return new WaitForSeconds(animationLength);

        float attackRadius = 4f;
        Collider2D[] attackPlayers = Physics2D.OverlapCircleAll(boss.transform.position, attackRadius);

        foreach (Collider2D player in attackPlayers)
        {
            if (player.CompareTag("Player"))
            {
                // playerController.TakeDamage(30f);
            }
        }

        animatorController.SecondAttackPattern2(false);
    }

    private IEnumerator AttackPattern3() // 투사체 발사
    {
        animatorController.SecondAttackPattern3(true);

        float animationLength = animatorController.GetAnimationLength("SecondAttack3");

        yield return new WaitForSeconds(animationLength * 0.5f);

        GameObject projectile = Object.Instantiate(firstProjectilePrefab, boss.attackPoint.position, Quaternion.identity); // 투사체 생성
        FirstProjectile firstProjectile = projectile.GetComponent<FirstProjectile>(); // Projectile 스크립트 참조
        firstProjectile.GetComponent<FirstProjectile>().SetDirection((boss.player.position - boss.attackPoint.position).normalized); // 투사체 방향 설정
        
        firstProjectile.speed += 5;
        firstProjectile.damage += 10;

        yield return new WaitForSeconds(animationLength * 0.5f);

        animatorController.SecondAttackPattern3(false);
    }

    private IEnumerator Heal() // 체력 5% 회복
    {
        animatorController.SecondHeal(true);

        float animationLength = animatorController.GetAnimationLength("Heal");
        yield return new WaitForSeconds(animationLength);

        boss.HP += boss.HP * 0.05f;
        
        animatorController.SecondHeal(false);
    }

    private void SpawnRandomStones(int spawnStoneCount)
    {
        float minX = -2f, maxX = 2f;
        float minY = -7f, maxY = 3f;

        for (int i = 0; i < spawnStoneCount; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            Object.Instantiate(stone, spawnPosition, Quaternion.identity); // 스톤 생성
        }
    }
}
