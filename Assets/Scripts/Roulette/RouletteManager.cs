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

        // ȿ�� �Լ� �߰� 
        this.abilityEffect += ac;
        this.abilityEffect += ac2;
        this.abilityEffect += ac3;
    }
    public void InvokeAction() 
    {
        //Debug.Log($"{abilityName}�� �ش��ϴ� ī���� invoke");
        abilityEffect.Invoke();
    }
}

public class RouletteManager : MonoBehaviour
{
    // �̱��� 
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
        // �̹� �ν��Ͻ��� �����ϰ� ���� ������Ʈ�� �ƴ϶��
        if (instance != null && instance != this)
        {
            // �ߺ��� �ν��Ͻ� ����
            Destroy(gameObject);
            return;
        }

        // �ν��Ͻ��� ������ ���� �ν��Ͻ� ����
        instance = this;
    }

    void Start()
    {
        // �����Ƽ Ƽ� Ŭ���� �ۼ� 
        stateToAbility = new Dictionary<AblityState, List<Ability>>();
        selectAbility = new Ability[3];
        selectAbilityState = new AblityState[3];

        stateToAbility[AblityState.Rare] = new List<Ability>();
        stateToAbility[AblityState.Epic] = new List<Ability>();
        stateToAbility[AblityState.Legendary] = new List<Ability>();

        // �� �ʱ�ȭ 
        SettingAbility();

        RandomAbility();
    }

    private void RandomAbility() 
    {
        // 3���� �ɷ��� ����
        for (int i = 0; i < 3; i++)
        {
            // 0���� 1 ������ ���� float �� ����
            float randomValue = UnityEngine.Random.Range(0f, 1f);

            // ���� ���� ���� �ɷ� ��� ����
            AblityState selectedState;

            if (randomValue <= LegendaryRate) // 0.05 ���ϸ� ���� ���
            {
                selectedState = AblityState.Legendary;
            }
            else if (randomValue <= EpicRate) // 0.05���� ũ�� 0.35 ���ϸ� ���� ���
            {
                selectedState = AblityState.Epic;
            }
            else // 0.35���� ũ�� 1 ���ϸ� ���� ���
            {
                selectedState = AblityState.Rare;
            }

            // ���õ� ��޿��� �����ϰ� �ɷ� ����
            List<Ability> availableAbilities = stateToAbility[selectedState];

            // �ش� ��޿� �ɷ��� �ִ��� Ȯ��
            if (availableAbilities.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableAbilities.Count);

                // ���õ� �ɷ°� ��� ����
                selectAbility[i] = availableAbilities[randomIndex];
                selectAbilityState[i] = selectedState;
            }
        }
    }

    private void SettingAbility() 
    {
        #region Rare 
        stateToAbility[AblityState.Rare].Add(
            new Ability("������ ����", "�������� ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Damage)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("�ִ� ü�� ����", "�ִ� ü���� ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.MaxHP)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("���� ����", "������ ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("ũ��Ƽ�� ���� ����", "ũ��Ƽ�� ������ ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.CritRate)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("ũ��Ƽ�� ������ ����", "ũ��Ƽ�� �������� ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.CritDamage)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("�̵��ӵ� ����", "�̵��ӵ��� ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.MoveSpeed)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("���ݼӵ� ����", "���ݼӵ��� ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.AttackSpeed)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("�߻�ü �ӵ� ����", "�߻�ü �ӵ��� ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.ShotSpeed)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("���� �߻� ���� ����", "���ÿ� �߻� ������ ����ü ������ ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(1f, PlayerManager.PlayerStat.NumOfOneShot)));
        stateToAbility[AblityState.Rare].Add(
            new Ability("���� �߻� ���� ����", "�������� �߻� ������ ����ü ������ ���� �����մϴ�", () => PlayerManager.Instance.UpdateStat(1f, PlayerManager.PlayerStat.NumOfShooting)));
        #endregion

        #region Epic
        stateToAbility[AblityState.Epic].Add(
            new Ability("����ó�� �ܴ��ϰ�!", "���°� �ִ�ü���� ���ÿ� �����մϴ�", 
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.MaxHP)));
        stateToAbility[AblityState.Epic].Add(
            new Ability("�ѹ��� ���", "ũ��Ƽ�� ������ �������� ���ÿ� �����մϴ�",
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.CritRate),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.CritDamage)));
        stateToAbility[AblityState.Epic].Add(
            new Ability("�� ���� ���� ���մ±�", "���� �߻� ������ ���� �߻� ������ ���ÿ� �����մϴ�",
                () => PlayerManager.Instance.UpdateStat(2f, PlayerManager.PlayerStat.NumOfShooting),
                () => PlayerManager.Instance.UpdateStat(2f, PlayerManager.PlayerStat.NumOfOneShot)));
        stateToAbility[AblityState.Epic].Add(
            new Ability("������ ��������", "���ݼӵ��� �߻�ü �ӵ��� ���ÿ� �����մϴ�",
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.AttackSpeed),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.ShotSpeed)));
        #endregion

        #region Legendary
        stateToAbility[AblityState.Legendary].Add(
            new Ability("���� ���ǳ������?", "���ݷ��� ���� �����մϴ�",
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.Defence)));
        stateToAbility[AblityState.Legendary].Add(
            new Ability("�����ϼ���", "�̵��ӵ�,���ݼӵ�,�߻�ü �ӵ� ��� �����մϴ�",
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.MoveSpeed),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.AttackSpeed),
                () => PlayerManager.Instance.UpdateStat(5f, PlayerManager.PlayerStat.ShotSpeed)));
        #endregion
    }

    // ī�� ���� �� 
    public void SelectCard(int idx) 
    {
        // �ε����� �´� Action����
        try
        {
            selectAbility[idx].InvokeAction();
        }
        catch (Exception e) { Debug.Log(e); }
    }
}
