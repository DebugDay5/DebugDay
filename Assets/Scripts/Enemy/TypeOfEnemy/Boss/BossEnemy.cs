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
    private bool isDead = false; // 보스 사망 여부 추가
    private BossState currentState; // 현재 페이즈
    private BossAnimatorController animatorController;
    public GameObject firstProjectilePrefab;
    public GameObject stone;
    public Transform attackPoint;

    protected override void Awake()
    {
        base.Awake();
        animatorController = GetComponentInChildren<BossAnimatorController>();
        currentState = new BossFirstPhase(this);
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(BossAttackPattern()); // 보스 공격 패턴
    }

    protected override void Update()
    {
        if (!IsPlayerAvailable()) return;
        
        if (isDead) return;
        FlipSprite();
        PhaseTransition(); // 체력이 50% 이하일 시 2페이즈
        CheckLowHealthPhase(); // 체력이 10% 이하일 때 BreakArmor 실행
    }

    public override void Attack()
    {
        if (isDead) return;
        currentState.Attack(); // 현재 페이즈의 공격
    }

    private IEnumerator BossAttackPattern() // 현재 페이즈의 공격 패턴
    {
        while (HP > 0)
        {
            if (isTransitioning || isArmorBroken || !canAttack) yield break;
            yield return new WaitForSeconds(attackCooldown);
            if (!isArmorBroken && canAttack) currentState.Attack();
        }
    }

    private void PhaseTransition() // 체력이 50% 이하일 시 2페이즈
    {
        if (HP <= 1000 * 0.5f && !isSecondPhase && !isTransitioning)
        {
            isTransitioning = true;
            StopCoroutine(BossAttackPattern());
            StartCoroutine(TransitionToSecondPhase());
        }
    }

    private IEnumerator TransitionToSecondPhase()
    {
        isTransitioning = true;
        animatorController.PhaseTransition(true);

        float animationLength = animatorController.GetAnimationLength("PhaseTransition");
        yield return new WaitForSeconds(animationLength);

        animatorController.PhaseTransition(false);
        isSecondPhase = true;
        isTransitioning = false;
        
        GetComponent<Animator>().runtimeAnimatorController = secondPhaseAnimator;
        currentState = new BossSecondPhase(this);
        StartCoroutine(BossAttackPattern());
    }

    private void CheckLowHealthPhase() // 체력이 10% 이하일 때 BreakArmor 실행
    {
        if (HP <= 1000 * 0.1f && isSecondPhase && !isArmorBroken)
        {
            canAttack = false;
            StartCoroutine(ExecuteBreakArmorAndLast());
        }
        
        if (HP <= 0 && !isDead)
        {
            StartCoroutine(BossDie());
        }
    }

    private IEnumerator ExecuteBreakArmorAndLast()
    {
        isArmorBroken = true;
        yield return StartCoroutine(BreakArmor()); // BreakArmor 실행 후 대기
        yield return StartCoroutine(Last()); // BreakArmor 완료 후 Last 실행
    }

    private IEnumerator BreakArmor()
    {
        animatorController.SecondBreakArmor(true);

        float animationLength = animatorController.GetAnimationLength("BreakArmor");
        yield return new WaitForSeconds(animationLength);

        animatorController.SecondBreakArmor(false);
    }

    private IEnumerator Last()
    {
        animatorController.SecondLast(true);

        float animationLength = animatorController.GetAnimationLength("Last");
        yield return new WaitForSeconds(animationLength);
    }

        private IEnumerator BossDie() // 보스 사망 애니메이션 실행
    {
        isDead = true; // 보스 사망 상태 설정
        animatorController.BossDie(true);

        float animationLength = animatorController.GetAnimationLength("BossDie");
        yield return new WaitForSeconds(animationLength);
        
        isDead = true;

        Destroy(gameObject);
    }
}
