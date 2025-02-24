using System.Collections;
using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float passiveHealthDecayRate = 10f;
    
    private bool isSecondPhase = false;
    private BossState currentState;
    private BossAnimatorController animatorController;

    protected override void Awake()
    {
        base.Awake();
        animatorController = GetComponentInChildren<BossAnimatorController>();
        currentState = new BossFirstPhase(this);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(BossAttackPattern());
        StartCoroutine(PassiveHealthDecay());
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

    private IEnumerator PassiveHealthDecay()
    {
        while (HP > 0)
        {
            HP -= passiveHealthDecayRate * Time.deltaTime;
            yield return null;
        }
    }

    private void TransitionToSecondPhase()
    {
        isSecondPhase = true;
        animatorController.SetTransitionTrigger("IsPhaseTransition");
        currentState = new BossSecondPhase(this);
    }
}
