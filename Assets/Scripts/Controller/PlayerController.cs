using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerManager playerManager;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private Vector2 moveDirection; //이동 방향
    private Vector2 lookDirection;  //발사 방향

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();


        moveDirection = Vector2.zero;
        lookDirection = Vector2.zero;
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerManager = PlayerManager.Instance;
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
    }

    private void Movement(Vector2 moveDirection)
    {
        moveDirection = moveDirection * playerManager.MoveSpeed;

        _rigidbody.velocity = moveDirection;
    }

    private void LookTarget()
    {
        Vector2 target = playerManager.GetTarget();
        lookDirection = target - (Vector2)transform.position;
        lookDirection = lookDirection.normalized;

        float rotZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;
        _spriteRenderer.flipX = isLeft;
    }
}
