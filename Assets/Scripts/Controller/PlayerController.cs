using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PlayerManager playerManager;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    [SerializeField] private Transform shootingPosition;
    [SerializeField] private Transform pivot;

    private Vector2 moveDirection; //이동 방향
    private Vector2 lookDirection;  //발사 방향

    public GameObject projectile;//테스트용

    private float shootTime; //남은 발사 시간
    private int shootNum; //발사 횟수
    private bool isShooting = false;
    private bool isMoving = false;
    private bool isDead = false;
    float rotZ;

    private bool isInvincible = false;
    GameObject target;

    public GameObject HealthBar;
    public TextMeshProUGUI healthBarText;

    public Vector2 minBounds;  // 이동 가능한 최소 x,y값
    public Vector2 maxBounds;  // 이동 가능한 최대 x,y값

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        

        moveDirection = Vector2.zero;
        lookDirection = Vector2.zero;
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerManager = PlayerManager.Instance;
        shootTime = playerManager.AttackSpeed;
        shootNum = playerManager.NumOfShooting;
        healthBarText.text = playerManager.Hp.ToString();
    }


    float hitTime = 10f;
    float curr = 10f;
    // Update is called once per frame
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontal, vertical).normalized;

        curr -= Time.deltaTime;
        if(curr < 0f)
        {
            TakeDamage(10f);
            curr = hitTime;
        }
    }

    private void FixedUpdate()
    {
        if(isDead) return;
        Movement(moveDirection);
        LookTarget();
        Shoot();
    }

    private void Movement(Vector2 moveDirection)
    {
        moveDirection = moveDirection * playerManager.MoveSpeed;
        if (moveDirection != Vector2.zero)
        {
            _animator.SetBool("IsMove", true);
            isMoving = true;
        }
        else
        {
            isMoving = false;
            _animator.SetBool("IsMove", false);
        }

        _rigidbody.velocity = moveDirection;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);

        transform.position = pos;

        HealthBar.transform.position = pos + new Vector3(0f, 0.6f, 0f);
    }

    private void LookTarget()
    {
        if (_animator.GetBool("IsMove")) //이동시 이동방향으로
        {
            lookDirection = _rigidbody.velocity.normalized;
        }
        else //멈췄을 경우 타겟방향 및 공격
        {
            target = playerManager.GetTarget();
            if (target == null) return;

            lookDirection = target.transform.position - transform.position;
            lookDirection = lookDirection.normalized;
        }
        rotZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        if (pivot != null)
        {
            pivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        bool isLeft = Mathf.Abs(rotZ) > 90f;
        _spriteRenderer.flipX = isLeft;
    }

    
    private void Shoot()
    {
        
        if (target == null) return;
        if (projectile == null) return;
        if (isShooting) return;
        
        shootTime -= Time.deltaTime;
        if (isMoving)
        {
            if (shootTime < 0.1f)
                shootTime = 0.1f;
            
            return;
        }
        if (shootTime < 0)
        {
            isShooting = true;

            MakeProjectile();
        }
    }

    private void MakeProjectile()
    {
        if(shootNum == 0)
        {
            isShooting = false;
            shootTime = playerManager.AttackSpeed;
            shootNum = playerManager.NumOfShooting;
            return;
        }

        for(int i = -(playerManager.NumOfOneShot/2); i <= (playerManager.NumOfOneShot/2); i++)
        {
            if (i == 0 && playerManager.NumOfOneShot % 2 == 0)
                continue;
            Vector3 addictionPosition = Quaternion.Euler(0, 0, rotZ) * new Vector3(0, 0.1f * i, 0);
            GameObject obj = Instantiate(projectile, shootingPosition.position + addictionPosition, Quaternion.Euler(0f, 0f, 0f));
            obj.transform.right = lookDirection;
            
            Rigidbody2D objRigid = obj.GetComponent<Rigidbody2D>();
            obj.GetComponent<ProjectileController>().Init(playerManager.Damage, playerManager.CritRate, playerManager.CritDamage, false);

            objRigid.velocity = lookDirection * playerManager.ShotSpeed;
        }

        shootNum--;
        Invoke("MakeProjectile", 0.1f);
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return; //피격 무적
        
        playerManager.Hp -= damage;
        HealthBar.GetComponent<Slider>().value = playerManager.Hp / playerManager.MaxHp;
        healthBarText.text = playerManager.Hp.ToString();

        if (playerManager.Hp <= 0f)
        {
            playerManager.PlayerDead();
            isDead = true;
            Invoke("DeletePlayer", 1f); //1초 뒤 삭제
            return;
        }

        isInvincible = true;
        _animator.SetBool("IsInvincible", true);
        Invoke("InvincibleChange", 0.5f); //0.5초뒤 무적 해제
    }

    private void InvincibleChange()
    {
        _animator.SetBool("IsInvincible", false);
        isInvincible = false;
    }

    private void DeletePlayer()
    {
        Destroy(gameObject);
    }

    public void UpdateBounds()
    {
        if (DungeonManager.Instance != null && DungeonManager.Instance.currentDungeonData != null)
        {
            minBounds = DungeonManager.Instance.currentDungeonData.minPlayerBounds;
            maxBounds = DungeonManager.Instance.currentDungeonData.maxPlayerBounds;
        }
    }
}
