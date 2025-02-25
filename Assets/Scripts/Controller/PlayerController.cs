using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float rotZ;


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
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontal, vertical).normalized;
    }

    private void FixedUpdate()
    {
        Movement(moveDirection);
        LookTarget();
        Shoot();//테스트용
    }

    private void Movement(Vector2 moveDirection)
    {
        moveDirection = moveDirection * playerManager.MoveSpeed;
        if (moveDirection != Vector2.zero)
            _animator.SetBool("IsMove", true);
        else
            _animator.SetBool("IsMove", false);

        _rigidbody.velocity = moveDirection;
    }

    private void LookTarget()
    {
        GameObject target = playerManager.GetTarget();
        if (target == null) return;

        lookDirection = target.transform.position - transform.position;
        lookDirection = lookDirection.normalized;
        rotZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        if(pivot != null)
        {
            pivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        bool isLeft = Mathf.Abs(rotZ) > 90f;
        _spriteRenderer.flipX = isLeft;
    }

    
    private void Shoot() //테스트용
    {
        if(projectile == null) return;
        if (isShooting) return;

        shootTime -= Time.deltaTime;
        if (shootTime < 0)
        {
            isShooting = true;

            //StartCoroutine("MakeProjectile",playerManager.NumOfShooting);
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

        /*
        for (int i = 0; i < playerManager.NumOfOneShot; i++)
        {
            GameObject obj = Instantiate(projectile, shootingPosition.position, Quaternion.Euler(0f, 0f, 0f));
            obj.transform.right = lookDirection;
            Rigidbody2D objRigid = obj.GetComponent<Rigidbody2D>();
            objRigid.velocity = lookDirection * playerManager.ShotSpeed;
        }
        */

        for(int i = -(playerManager.NumOfOneShot/2); i <= (playerManager.NumOfOneShot/2); i++)
        {
            if (i == 0 && playerManager.NumOfOneShot % 2 == 0)
                continue;
            Vector3 addictionPosition = Quaternion.Euler(0, 0, rotZ) * new Vector3(0, 0.1f * i, 0);
            GameObject obj = Instantiate(projectile, shootingPosition.position + addictionPosition, Quaternion.Euler(0f, 0f, 0f));
            obj.transform.right = lookDirection;
            
            Rigidbody2D objRigid = obj.GetComponent<Rigidbody2D>();
            objRigid.velocity = lookDirection * playerManager.ShotSpeed;
        }

        shootNum--;
        Invoke("MakeProjectile", 0.1f);
    }
    
}
