using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] public int damage = 40;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (animator != null)
        {
            animator.Play("StoneAnimation"); // 애니메이션 한 번만 재생
            StartCoroutine(WaitForAnimation());
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            // PlayerManager.Instance.Hp -= damage;
        }
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
