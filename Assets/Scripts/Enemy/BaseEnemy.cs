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
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Transform player;

    [SerializeField] protected float hp;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected int gold;

    [SerializeField] private GameObject enemyHPBarPrefab;
    private EnemyHPBar enemyHPBar;
    private float maxHP;

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
        maxHP = hp;

        if (enemyHPBarPrefab != null)
        {
            GameObject hpBarObj = Instantiate(enemyHPBarPrefab);
            enemyHPBar = hpBarObj.GetComponentInChildren<EnemyHPBar>();
            enemyHPBar.Initialize(transform);
        }
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
            yield return new WaitForSeconds(1f);
        }
    }

    protected bool IsPlayerAvailable()
    {
        return player != null && player.gameObject != null;
    }

    protected virtual void Update()
    {
        if (!IsPlayerAvailable()) return;
    }

    public virtual void TakeDamage(float damage)
    {
        HP -= PlayerManager.Instance.Damage;
        animationHandler.Hit(true);
        StartCoroutine(HitMotion(0.6f));

        if (enemyHPBar != null)
        {
            enemyHPBar.UpdateHP(HP, maxHP);
        }

        if (HP <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitMotion(float delay)
    {
        yield return new WaitForSeconds(delay); // 애니메이션 길이만큼 대기
        animationHandler.Hit(false); // 애니메이션 중지
    }

    protected void Die()
    {
        DungeonManager.Instance.OnEnemyDead();
        animationHandler.Die();
        EnemyManager.Instance?.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public abstract void Attack(); // abstract를 사용하여 각 Enemy마다 공격 방식 정의

    protected void FlipSprite()
    {
        Vector3 currentScale = transform.localScale; // 현재 스케일 저장
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }

        if (enemyHPBar != null)
        {
            Transform hpBarTransform = enemyHPBar.transform;
            hpBarTransform.localScale = new Vector3(Mathf.Abs(hpBarTransform.localScale.x), hpBarTransform.localScale.y, hpBarTransform.localScale.z);
        }
    }
}
