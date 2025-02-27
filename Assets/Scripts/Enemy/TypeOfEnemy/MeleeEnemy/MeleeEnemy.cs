using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private float attackRange = 1.5f; // 공격 범위
    [SerializeField] private float attackCooldown = 3.0f; // 공격 속도
    private float lastAttackTime; // 공격 시간
    private bool isAttacking = true;

    protected override void Awake()
    {
        base.Awake();
        hp = 30;
        speed = 1f;
        damage = 10;
        gold = 5;
    }

    protected override void Update()
    {
        if (!IsPlayerAvailable()) return;

        float distance = Vector3.Distance(transform.position, player.position); // 상호 간 거리 계산

        FlipSprite(); // 스프라이트 방향 전환

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
        if (isAttacking)
        {
            lastAttackTime = Time.time; // 쿨타임 계산

            animationHandler.Attack(true); // 공격 애니메이션을 호출하고 OnAttackComplete 콜백
            
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.TakeDamage(damage);

            isAttacking = false; // 공격 후에는 공격 불가능 상태로 변경

            StartCoroutine(AttackDelay(0.7f));
        }
    }

    private IEnumerator AttackDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 애니메이션 길이만큼 대기
        animationHandler.Attack(false); // 애니메이션 중지
        isAttacking = true; // 쿨타임이 끝나면 공격 가능 상태로 변경
    }
}
