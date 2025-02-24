using System.Collections;
using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float passiveHealthDecayRate = 5f; // 페이즈 테스트 코드
    
    [SerializeField] private RuntimeAnimatorController firstPhaseAnimator;
    [SerializeField] private RuntimeAnimatorController secondPhaseAnimator;

    private bool isSecondPhase = false; // 2페이즈
    private bool isTransitioning = false; // IsPhaseTransition이 한 번만 실행되게 하는 코드
    private bool isArmorBroken = false; // BreakArmor 실행 여부
    private BossState currentState; // 현재 페이즈
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
        StartCoroutine(BossAttackPattern()); // 보스 공격 패턴
        StartCoroutine(PassiveHealthDecay()); // 페이즈 테스트 코드
    }

    private void Update()
    {
        currentState.UpdateState(); // 추후 추가
        PhaseTransition(); // 체력이 50% 이하일 시 2페이즈
        CheckLowHealthPhase(); // 체력이 10% 이하일 때 BreakArmor 실행
    }

    public override void Attack()
    {
        currentState.Attack(); // 현재 페이즈의 공격
    }

    private IEnumerator BossAttackPattern() // 현재 페이즈의 공격 패턴
    {
        while (HP > 0)
        {
            yield return new WaitForSeconds(attackCooldown);
            currentState.Attack();
        }
    }

    private IEnumerator PassiveHealthDecay() // 페이즈 테스트 코드
    {
        while (HP > 0)
        {
            HP -= passiveHealthDecayRate * Time.deltaTime;
            yield return null;
        }
    }

    private void PhaseTransition() // 체력이 50% 이하일 시 2페이즈
    {
        if (HP <= 1000 * 0.5f && !isSecondPhase && !isTransitioning)
        {
            StartCoroutine(TransitionToSecondPhase());
        }
    }

    private IEnumerator TransitionToSecondPhase()
    {
        isTransitioning = true;
        animatorController.PhaseTransition(true);

        float animationLength = animatorController.GetAnimationLength("PhaseTransition");
        yield return new WaitForSeconds(animationLength); // 애니메이션의 정확한 길이

        animatorController.PhaseTransition(false);
        isSecondPhase = true;
        
        GetComponent<Animator>().runtimeAnimatorController = secondPhaseAnimator;

        currentState = new BossSecondPhase(this);
    }

    private void CheckLowHealthPhase() // 체력이 10% 이하일 때 BreakArmor 실행
    {
        if (HP <= 1000 * 0.1f && isSecondPhase && !isArmorBroken)
        {
            StartCoroutine(ExecuteBreakArmorAndLast());
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
        Debug.Log("Boss: 갑옷 파괴");
        animatorController.SecondBreakArmor(true);

        float animationLength = animatorController.GetAnimationLength("BreakArmor");
        yield return new WaitForSeconds(animationLength);

        animatorController.SecondBreakArmor(false);
    }

    private IEnumerator Last()
    {
        Debug.Log("Boss: 최후의 발악");
        animatorController.SecondLast(true);

        float animationLength = animatorController.GetAnimationLength("Last");
        yield return new WaitForSeconds(animationLength);

        animatorController.SecondLast(false);
    }
}
