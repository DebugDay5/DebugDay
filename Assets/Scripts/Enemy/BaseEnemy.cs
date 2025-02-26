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
    [HideInInspector] public Transform player;
    [HideInInspector] public PlayerController playerController;
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
        StartCoroutine(FindPlayer()); // 플레이어 자동 검색 시작
    }

    private IEnumerator FindPlayer()
    {
        while (true)
        {
            if (!IsPlayerAvailable())
            {
                GameObject playerObject = GameObject.FindWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }
            yield return new WaitForSeconds(1f); // 1초마다 확인
        }
    }

    protected bool IsPlayerAvailable()
    {
        return player != null && player.gameObject != null;
    }

    protected virtual void Update()
    {
        if (!IsPlayerAvailable()) return; // 플레이어가 없으면 동작 중지
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
        Destroy(gameObject);
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
