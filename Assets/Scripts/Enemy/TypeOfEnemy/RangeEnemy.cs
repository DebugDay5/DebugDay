using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : BaseEnemy
{
    [SerializeField] private float attackRange = 8f; // 공격 범위
    [SerializeField] private float attackCooldown = 1.0f; // 연사 속도
    [SerializeField] private GameObject projectilePrefab; // 투사체

    private float lastAttackTime; // 공격 시간

    private void Awake()
    {
        hp = 70;
        speed = 2;
        attackPower = 0;
        gold = 5;
    }
    
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
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
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetDirection((player.position - transform.position).normalized);
    }
}
