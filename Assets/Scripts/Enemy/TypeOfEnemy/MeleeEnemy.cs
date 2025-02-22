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
        float distance = Vector3.Distance(transform.position, player.position); // 상호 간 거리 계산

        FlipSprite(); // 스프라이트 방향 전환

        if (isAttacking) // true 공격 x, false 공격 o
            return;

        if (distance > attackRange) // 공격 범위보다 거리가 멀면 이동
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime); // 현재 위치에서 플레이어 위치로 일정한 속도로 이동
            animationHandler.Move(true);
        }
        else if (Time.time >= lastAttackTime + attackCooldown) // 공격 쿨타임 계산
        {
            animationHandler.Move(false);
            Attack();
        }
    }

    public override void Attack() // BaseEnemy의 Attack을 Override
    {
        lastAttackTime = Time.time; // 쿨타임 계산
        isAttacking = true; // 공격
        animationHandler.Attack(OnAttackComplete); // 공격 애니메이션을 호출하고 OnAttackComplete 콜백
        // player.GetComponent<Player>().TakeDamage(attackPower); // Player에게 데미지를 입힘
    }

    private void OnAttackComplete()
    {
        isAttacking = false; // 다시 공격할 수 있도록 함
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
