using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AblityState 
{
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class Ability 
{
    [SerializeField] private string abilityName;
    [SerializeField] private string abilityDescription;
    [SerializeField] private Sprite abilityicon;
    [SerializeField] private Action abilityEffect;

    public string AbilityName { get => abilityName; set => abilityName = value; }
    public string AbilityDescription { get => abilityDescription; set => abilityDescription = value; }
    public Sprite Abilityicon { get => abilityicon; set => abilityicon = value; }

    public Ability(string n, string d, Action ac , Action ac2 = null , Action ac3 = null) 
    {
        this.abilityName = n;
        this.abilityDescription = d;

        // 효과 함수 추가 
        this.abilityEffect += ac;
        this.abilityEffect += ac2;
        this.abilityEffect += ac3;
    }
    public void InvokeAction() 
    {
        //Debug.Log($"{abilityName}에 해당하는 카드의 invoke");
        abilityEffect.Invoke();
    }
}

public class RouletteManager : MonoBehaviour
{
    // 싱글톤 
    private static RouletteManager instance;

    public static RouletteManager Instance { get => instance; }

    private Dictionary<AblityState, List<Ability>> stateToAbility;
    [SerializeField] private Ability[] selectAbility;
    [SerializeField] private AblityState[] selectAbilityState;

    private float RareRate = 1f;
    private float EpicRate = 0.35f;
    private float LegendaryRate = 0.05f;

    private void Awake()
    {
        // 이미 인스턴스가 존재하고 현재 오브젝트가 아니라면
        if (instance != null && instance != this)
        {
            // 중복된 인스턴스 제거
            Destroy(gameObject);
            return;
        }

        // 인스턴스가 없으면 현재 인스턴스 설정
        instance = this;
    }

    void Start()
    {
        // 어빌리티 티어별 클래스 작성 
        stateToAbility = new Dictionary<AblityState, List<Ability>>();
        selectAbility = new Ability[3];
        selectAbilityState = new AblityState[3];

        stateToAbility[AblityState.Rare] = new List<Ability>();
        stateToAbility[AblityState.Epic] = new List<Ability>();
        stateToAbility[AblityState.Legendary] = new List<Ability>();

        // 값 초기화 
        SettingAbility();

        RandomAbility();
    }

    private void RandomAbility() 
    {
        // 3개의 능력을 선택
        for (int i = 0; i < 3; i++)
        {
            // 0부터 1 사이의 랜덤 float 값 생성
            float randomValue = UnityEngine.Random.Range(0f, 1f);

            // 랜덤 값에 따라 능력 등급 결정
            AblityState selectedState;

            if (randomValue <= LegendaryRate) // 0.05 이하면 전설 등급
            {
                selectedState = AblityState.Legendary;
            }
            else if (randomValue <= EpicRate) // 0.05보다 크고 0.35 이하면 에픽 등급
            {
                selectedState = AblityState.Epic;
            }
            else // 0.35보다 크고 1 이하면 레어 등급
            {
                selectedState = AblityState.Rare;
            }

            // 선택된 등급에서 랜덤하게 능력 선택
            List<Ability> availableAbilities = stateToAbility[selectedState];

            // 해당 등급에 능력이 있는지 확인
            if (availableAbilities.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableAbilities.Count);

                // 선택된 능력과 등급 저장
                selectAbility[i] = availableAbilities[randomIndex];
                selectAbilityState[i] = selectedState;
            }
        }
    }

    private void SettingAbility() 
    {
        #region Rare 
        stateToAbility[AblityState.Rare].Add(
            new Ability("데미지 증가", "데미지가 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Damage)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("최대 체력 증가", "최대 체력이 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.MaxHP)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("방어력 증가", "방어력이 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("크리티컬 비율 증가", "크리티컬 비율이 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.CritRate)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("크리티컬 데미지 증가", "크리티컬 데미지가 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.CritDamage)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("이동속도 증가", "이동속도가 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.MoveSpeed)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("공격속도 증가", "공격속도가 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.AttackSpeed)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("발사체 속도 증가", "발사체 속도가 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.ShotSpeed)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("동시 발사 갯수 증가", "동시에 발사 가능한 투사체 갯수가 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(1f, PlayerManager.PlayerStat.NumOfOneShot)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("연속 발사 갯수 증가", "연속으로 발사 가능한 투사체 갯수가 소폭 증가합니다", () => PlayerManager.Instance.UpdateStat(1f, PlayerManager.PlayerStat.NumOfShooting)));
        #endregion

        #region Epic
        stateToAbility[AblityState.Epic].Add(
            new Ability("바위처럼 단단하게!", "방어력과 최대체력이 동시에 증가합니다", 
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.MaxHP)));
        stateToAbility[AblityState.Epic].Add(
            new Ability("한방을 노려", "크리티컬 비율과 데미지가 동시에 증가합니다",
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.CritRate),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.CritDamage)));
        stateToAbility[AblityState.Epic].Add(
            new Ability("내 총이 불을 내뿜는군", "동시 발사 갯수와 연속 발사 갯수가 동시에 증가합니다",
                () => PlayerManager.Instance.UpdateStat(2f, PlayerManager.PlayerStat.NumOfShooting),
                () => PlayerManager.Instance.UpdateStat(2f, PlayerManager.PlayerStat.NumOfOneShot)));
        stateToAbility[AblityState.Epic].Add(
            new Ability("빠르게 없애주지", "공격속도와 발사체 속도가 동시에 증가합니다",
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.AttackSpeed),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.ShotSpeed)));
        #endregion

        #region Legendary
        stateToAbility[AblityState.Legendary].Add(
            new Ability("이제 결판내볼까요?", "공격력이 대폭 증가합니다",
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence)));
        stateToAbility[AblityState.Legendary].Add(
            new Ability("방출하세요", "이동속도,공격속도,발사체 속도 모두 증가합니다",
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.MoveSpeed),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.AttackSpeed),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.ShotSpeed)));
        #endregion
    }

    // 카드 선택 시 
    public void SelectCard(int idx) 
    {
        // 인덱스에 맞는 Action실행
        try
        {
            selectAbility[idx].InvokeAction();
        }
        catch (Exception e) { Debug.Log(e); }
    }
}
