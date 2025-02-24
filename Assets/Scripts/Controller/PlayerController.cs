using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerManager playerManager;

    private Rigidbody2D _rigidbody;

    private Vector2 moveDirection; //�̵� ����
    private Vector2 lookDirection;  //�߻� ����

    private GameObject target; //�߻��� ��

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
        if(target == null) return; //���� ���ٸ�
        
        lookDirection = target.transform.position - transform.position;
        lookDirection = lookDirection.normalized;
    }
}
