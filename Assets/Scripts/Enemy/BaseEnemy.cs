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
    protected Transform player; // 플레이어 추적
    protected AnimationHandler animationHandler;
    
    public int HP { get => hp; set => hp = value; }
    public float Speed { get => speed; set => speed = value; }
    public int AttackPower { get => attackPower; set => attackPower = value; }

    protected virtual void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
    }
    
    protected virtual void Start()
    {
        EnemyManager.Instance?.AddEnemy(this);
        player = GameObject.FindGameObjectWithTag("Player").transform; // player에 Player Tag 할당
    }
    
    public virtual void TakeDamage(int damage)
    {
        HP -= damage;
        animationHandler?.Hit();
        if (HP <= 0)
        {
            Die();
        }
    }
    
    protected void Die()
    {
        animationHandler?.Die();
        EnemyManager.Instance?.RemoveEnemy(this);
        Destroy(gameObject, 4f);
    }
    
    public abstract void Attack(); // abstract를 사용하여 각 Enemy마다 공격 방식 정의
}
