using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private float attackRange = 2f; // 공격 범위
    [SerializeField] private float attackCooldown = 2.0f; // 공격 속도
    private float lastAttackTime; // 공격 시간
    private bool isAttacking = false; // 공격 여부

    private void Awake()
    {
        base.Awake();
        hp = 100;
        speed = 1.5f;
        attackPower = 10;
        gold = 5;
    }
    
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        FlipSprite();

        // 공격 중이면 움직이거나 다시 공격하지 않음
        if (isAttacking)
            return;

        if (distance > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            animationHandler.Move(true);
        }
        else if (Time.time >= lastAttackTime + attackCooldown)
        {
            animationHandler.Move(false);
            Attack();
        }
    }
    
    public override void Attack()
    {
        lastAttackTime = Time.time;
        isAttacking = true;
        animationHandler.Attack(OnAttackComplete);
        // player.GetComponent<Player>().TakeDamage(attackPower);
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
