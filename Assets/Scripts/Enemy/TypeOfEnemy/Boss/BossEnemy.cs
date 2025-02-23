using System.Collections;
using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private Transform attackPoint;
    
    private bool isSecondPhase = false;
    private BossState currentState;
    private BossAnimatorController animatorController;

    protected override void Awake()
    {
        base.Awake();
        animatorController = GetComponent<BossAnimatorController>();
        currentState = new BossFirstPhase(this);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(BossAttackPattern());
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public override void Attack()
    {
        currentState.Attack();
    }

    private IEnumerator BossAttackPattern()
    {
        while (HP > 0)
        {
            if (HP <= 1000 * 0.5f && !isSecondPhase)
            {
                TransitionToSecondPhase();
            }
            yield return new WaitForSeconds(attackCooldown);
            currentState.Attack();
        }
    }

    private void TransitionToSecondPhase()
    {
        isSecondPhase = true;
        animatorController.SetTrigger("IsBreakArmor");
        currentState = new BossSecondPhase(this);
    }
}