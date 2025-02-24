using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 투사체 속도
    [SerializeField] private int damage = 40; // 투사체 데미지
    [SerializeField] private float slowDownRate = 100f;
    private Vector3 direction;
    private Animator animator;
     private bool isSlowingDown = false;

    private void Awake()
    {
      animator = GetComponent<Animator>();  
    }

    private void Start()
    {
        Invoke("DestroyProjectile", 5f); // 10초 후 자동 삭제
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

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
