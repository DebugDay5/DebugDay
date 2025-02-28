using System.Collections;
using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private RuntimeAnimatorController firstPhaseAnimator;
    [SerializeField] private RuntimeAnimatorController secondPhaseAnimator;

    private bool isTransitioning = false;
    private bool isSecondPhase = false; // 2페이즈
    private bool isArmorBroken = false; // BreakArmor 실행 여부
    private bool canAttack = true;
    public bool isDead = false; // 보스 처치 여부
    private BossState currentState; // 현재 페이즈
    private BossAnimatorController animatorController;
    public GameObject firstProjectilePrefab;
    public GameObject stone;
    public Transform attackPoint;

    private bool isInvulnerable = false;

    protected override void Awake()
    {
        base.Awake();
        animatorController = GetComponentInChildren<BossAnimatorController>();
        currentState = new BossFirstPhase(this);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(BossAttackPattern()); // 공격 패턴
    }

    protected override void Update()
    {
        if (!IsPlayerAvailable() || isDead) return;
        FlipSprite();
        PhaseTransition(); // 체력 50% 이하
        CheckLowHealthPhase(); // 체력 10% 이하
    }

    public override void Attack()
    {
        if (isDead) return;
        currentState.Attack(); // 현재 페이즈 공격
    }

    public override void TakeDamage(float damage)
    {
        if (isDead || isInvulnerable) return;
        base.TakeDamage(damage);

        if (HP <= 0)
        {
            isDead = true;
            StartCoroutine(BossDie());
        }
    }

    private IEnumerator BossAttackPattern() // 현재 페이즈 공격 패턴
    {
        while (!isDead && HP > 0)
        {
            if (isTransitioning || isArmorBroken || !canAttack) yield break;
            yield return new WaitForSeconds(attackCooldown);
            if (isDead) yield break;
            if (!isDead && !isArmorBroken && canAttack) currentState.Attack();
        }
    }

    private void PhaseTransition() // 체력 50% 이하 실행
    {
        if (HP <= 1000 * 0.5f && !isSecondPhase && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionToSecondPhase());
        }
    }

    private IEnumerator TransitionToSecondPhase()
    {
        isTransitioning = true;
        isInvulnerable = true;
        animatorController.PhaseTransition(true);

        float animationLength = animatorController.GetAnimationLength("PhaseTransition");
        yield return new WaitForSeconds(animationLength);

        animatorController.PhaseTransition(false);
        isSecondPhase = true;
        isTransitioning = false;

        GetComponent<Animator>().runtimeAnimatorController = secondPhaseAnimator;
        currentState = new BossSecondPhase(this);
        
        isInvulnerable = false;
        StartCoroutine(BossAttackPattern());

        if (HP <= 0)
        {
            StartCoroutine(BossDie());
            yield break;
        }
    }

    private void CheckLowHealthPhase() // 체력 10% 이하
    {
        if (HP <= 1000 * 0.1f && isSecondPhase && !isArmorBroken)
        {
            canAttack = false;
            StartCoroutine(ExecuteBreakArmorAndLast());
        }
        
        if (HP <= 0)
        {
            StartCoroutine(BossDie());
        }
    }

    private IEnumerator ExecuteBreakArmorAndLast()
    {
        isInvulnerable = true;
        isArmorBroken = true;
        yield return StartCoroutine(BreakArmor());
        yield return StartCoroutine(Last());
        isInvulnerable = false;
    }

    private IEnumerator BreakArmor()
    {
        animatorController.SecondBreakArmor(true);

        float animationLength = animatorController.GetAnimationLength("BreakArmor");
        yield return new WaitForSeconds(animationLength);

        animatorController.SecondBreakArmor(false);
            if (HP <= 0)
    {
        StartCoroutine(BossDie());
    }
    }

    private IEnumerator Last()
    {
        animatorController.SecondLast(true);

        float animationLength = animatorController.GetAnimationLength("Last");
        yield return new WaitForSeconds(animationLength);

        animatorController.SecondLast(false);
            if (HP <= 0)
    {
        StartCoroutine(BossDie());
    }
    }

private IEnumerator BossDie()
{
    isDead = true;
    animatorController.BossDie(true);

    float animationLength = animatorController.GetAnimationLength("BossDie");
    yield return new WaitForSeconds(animationLength);

    Destroy(gameObject);
}

}
