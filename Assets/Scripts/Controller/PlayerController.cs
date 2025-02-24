using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerManager playerManager;

    private Rigidbody2D _rigidbody;

    private Vector2 moveDirection; //이동 방향
    private Vector2 lookDirection;  //발사 방향

    private GameObject target; //발사할 적

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
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
    }

    private void Movement(Vector2 moveDirection)
    {
        moveDirection = moveDirection * playerManager.MoveSpeed;

        _rigidbody.velocity = moveDirection;
    }

    private void AttackTarget()
    {
        if(target == null) return; //적이 없다면
        
        lookDirection = target.transform.position - transform.position;
        lookDirection = lookDirection.normalized;
    }
}
