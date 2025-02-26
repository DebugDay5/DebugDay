using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : BaseEnemy
{
    [SerializeField] private float attackRange = 5f; // 공격 범위
    [SerializeField] private float attackCooldown = 2.0f; // 공격 속도
    [SerializeField] private GameObject projectilePrefab; // 투사체
    private float lastAttackTime; // 공격 시간
    private bool isAttacking = true;

    protected override void Awake()
    {
        base.Awake();
        hp = 70;
        speed = 2f;
        damage = 0; // 지워도 됨
        gold = 5;
    }
    
    private void Update()
    {
        if (!IsPlayerAvailable()) return;

        float distance = Vector3.Distance(transform.position, player.position);

        FlipSprite();

        if (distance > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }
    
    public override void Attack()
    {
        if (isAttacking)
        {
            lastAttackTime = Time.time;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity); // 현재 위치에 투사체 생성
            projectile.GetComponent<Projectile>().SetDirection((player.position - transform.position).normalized); // 투사체 방향 할당
            animationHandler.Attack(true);
            isAttacking = false;
            StartCoroutine(AttackDelay(0.65f));
        }
    }

    private IEnumerator AttackDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animationHandler.Attack(false);
        isAttacking = true;
    }
}
