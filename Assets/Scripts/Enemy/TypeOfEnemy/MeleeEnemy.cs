using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private float attackRange = 1f;

    private void Awake()
    {
        hp = 100;
        speed = 5;
        attackPower = 10;
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
        if (player != null)
        {
            // player.GetComponent<Player>().TakeDamage(attackPower);
        }
    }
}
