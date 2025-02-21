using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    int HP { get; set; }
    float Speed { get; set; }
    int AttackPower { get; set; }
    void Attack();
    void TakeDamage(int damage);
}

public abstract class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] protected int hp;
    [SerializeField] protected float speed;
    [SerializeField] protected int attackPower;
    [SerializeField] protected int gold;
    protected Transform player;
    
    public int HP { get => hp; set => hp = value; }
    public float Speed { get => speed; set => speed = value; }
    public int AttackPower { get => attackPower; set => attackPower = value; }
    
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        EnemyManager.Instance?.AddEnemy(this);
    }
    
    public abstract void Attack();
    
    public virtual void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
    }
    
    protected void Die()
    {
        EnemyManager.Instance?.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
