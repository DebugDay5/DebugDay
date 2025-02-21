using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : BaseEnemy
{
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private GameObject projectilePrefab;

    private void Awake()
    {
        hp = 70;
        speed = 2;
        attackPower = 15;
        gold = 5;
    }
    
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            Attack();
        }
    }
    
    public override void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetDirection((player.position - transform.position).normalized);
    }
}
