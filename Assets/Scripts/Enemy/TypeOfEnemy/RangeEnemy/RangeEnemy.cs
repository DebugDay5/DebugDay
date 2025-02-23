using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : BaseEnemy
{
    [SerializeField] private float attackRange = 5f; // 공격 범위
    [SerializeField] private float attackCooldown = 2.0f; // 공격 속도
    [SerializeField] private GameObject projectilePrefab; // 투사체

    private float lastAttackTime; // 공격 시간
    private bool isAttacking = false; // 공격 여부

    private void Awake()
    {
        base.Awake();
        hp = 70;
        speed = 0.5f;
        attackPower = 0;
        gold = 5;
    }
    
    private void Update()
    {
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
        lastAttackTime = Time.time;
        isAttacking = true;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity); // 현재 위치에 투사체 생성
        projectile.GetComponent<Projectile>().SetDirection((player.position - transform.position).normalized); // 투사체 방향 할당
        animationHandler.Attack(OnAttackComplete);
    }

    private void OnAttackComplete()
    {
        isAttacking = false;
    }

    private void FlipSprite()
    {
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
