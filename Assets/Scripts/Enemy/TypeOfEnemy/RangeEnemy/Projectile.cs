using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 투사체 속도
    [SerializeField] private int damage = 15; // 투사체 데미지
    private float slowDownRate = 50f;
    private Vector3 direction;
    private Animator animator;
     private bool isSlowingDown = false;

    private void Awake()
    {
      animator = GetComponent<Animator>();  
    }

    public void SetDirection(Vector3 newDirection) // 투사체 방향 설정
    {
        direction = newDirection.normalized;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime; // 투사체의 독립적인 이동

        if (isSlowingDown)
        {
            speed = Mathf.Max(speed - slowDownRate * Time.deltaTime, 0); // 속도 점진적 감소
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // collision.GetComponent<Player>().TakeDamage(damage); // Player에게 데미지를 입힘
            PlayDestroyAnimation();
            isSlowingDown = true;
        }
        // else if (collision.CompareTag("Wall"))
        // {
        //     Destroy(gameObject);
        // }
    }

    private void PlayDestroyAnimation()
    {
        animator.SetBool("IsDestroy", true);
        StartCoroutine(DestroyAfterAnimation()); // 애니메이션이 끝난 후 오브젝트 파괴
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject); // 애니메이션이 끝난 후 오브젝트 파괴
    }
}
