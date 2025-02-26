using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    float HP { get; set; }
    float Speed { get; set; }
    float Damage { get; set; }
    void Attack();
    void TakeDamage(float damage);
}

public abstract class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] protected float hp;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected int gold;
    [HideInInspector] public Transform player; // 플레이어 추적
    protected AnimationHandler animationHandler;
    
    public float HP { get => hp; set => hp = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Damage { get => damage; set => damage = value; }

    protected virtual void Awake()
    {
        animationHandler = GetComponent<AnimationHandler>();
    }
    
    protected virtual void Start()
    {
        EnemyManager.Instance?.AddEnemy(this);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public virtual void TakeDamage(float damage)
    {
        HP -= PlayerManager.Instance.Damage;
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

    protected void FlipSprite()
    {
        Vector3 currentScale = transform.localScale; // 현재 스케일 저장
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z); // x축 방향을 양수로 설정
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z); // x축 방향을 음수로 설정
        }
    }
}
