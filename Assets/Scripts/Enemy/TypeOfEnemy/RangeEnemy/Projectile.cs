using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public float speed = 10f; // 투사체 속도
    [SerializeField] public float damage = 15f; // 투사체 데미지
    private Vector3 direction;
    private Animator animator;
    private bool isDestroyed;

    private void Awake()
    {
      animator = GetComponent<Animator>();  
    }

    private void Start()
    {
        Invoke("DestroyProjectile", 3f);
    }

    private void Update()
    {
        if (isDestroyed) return;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDirection)
    {
        if (!isDestroyed)
        {
            direction = newDirection.normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed || !collision.CompareTag("Player")) return;

        PlayerController playerController = collision.GetComponent<PlayerController>();
        playerController.TakeDamage(damage);

        isDestroyed = true;
        speed = 0f;
        direction = Vector3.zero;

        if (animator != null)
        {
            animator.SetBool("IsDestroy", true);
            StartCoroutine(DestroyAfterAnimation(animator.GetCurrentAnimatorStateInfo(0).length));
        }
        else
        {
            DestroyProjectile();
        }
    }

    private IEnumerator DestroyAfterAnimation(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);
        Destroy(gameObject);
    }

    private void DestroyProjectile()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
