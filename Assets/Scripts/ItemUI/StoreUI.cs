using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreUI : MonoBehaviour
{
    public static StoreUI Instance;

    [Header("UI")]
    public TextMeshProUGUI goldText;
    public Button highGachaOnePull;
    public Button highGachaTenPull;
    public Button lowGachaOnePull;
    public Button lowGachaTenPull;

    [Header("Gacha Panel")]
    public GameObject highGachaPanel;
    public GameObject lowGachaPanel;

    [Header("Gacha Result Panel")]
    public GameObject gachaResultPanel;          // ��í ��� ��ü �г� (��Ȱ��ȭ ����)
    public Transform resultItemContainer;          // GridLayoutGroup�� ����� �����̳�
    public GameObject gachaItemPrefab;             // GachaItem ������ (������ + �̸�)
    public Button retryButton;                     // "�� �� ��" ��ư
    public Button closeResultButton;               // ���â �ݱ� ��ư

    private ItemManager itemManager;
    private PlayerInventoryManager inventoryManager;

    private int PlayerGold => GameManager.Instance.Gold;
    private string lastGachaType = "";


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("StoreUI.cs�� Awake() �����");
    }

    

    private void Start()
    {
        UpdateGoldUI();

        itemManager = ItemManager.Instance;
        inventoryManager = PlayerInventoryManager.Instance;

        if (itemManager == null)
            Debug.LogError("StoreUI.cs�� Start()���� ItemManager�� NULL");
        if (inventoryManager == null)
            Debug.LogError("StoreUI.cs�� Start()���� PlayerInventoryManager�� NULL");

        UpdateGoldUI();

        highGachaOnePull.onClick.AddListener(() => PullItem("high", 1));
        highGachaTenPull.onClick.AddListener(() => PullItem("high", 10));
        lowGachaOnePull.onClick.AddListener(() => PullItem("low", 1));
        lowGachaTenPull.onClick.AddListener(() => PullItem("low", 10));

        retryButton.onClick.AddListener(() => PullItem(lastGachaType, 10));
        closeResultButton.onClick.AddListener(CloseGachaResult);
    }

    private void UpdateGoldUI()
    {
        goldText.text = $"GOLD {PlayerGold}";
    }

    public void PullItemHighOne() => PullItem("high", 1);
    public void PullItemHighTen() => PullItem("high", 10);
    public void PullItemLowOne() => PullItem("low", 1);
    public void PullItemLowTen() => PullItem("low", 10);

    private void PullItem(string gachaType, int count)
    {
        int cost = (gachaType == "high") ? 2000 : 500;
        cost *= count;

        if (PlayerGold < cost)
        {
            Debug.Log("��尡 �����մϴ�.");
            return;
        }

        lastGachaType = gachaType;
        ReducePlayerGold(cost);

        List<Item> obtainedItems = new List<Item>();
        for (int i = 0; i < count; i++)
        {
            Item item = GetRandomItem(gachaType);
            obtainedItems.Add(item);
            PlayerInventoryManager.Instance.AddItem(item);
        }

        ShowGachaResult(obtainedItems);
    }

    private Item GetRandomItem(string gachatype)
    {
        float rand = Random.value * 100;

        if (gachatype == "high")    // ��ް�í ����60% ����ũ30% ��������10%
        {
            if (rand < 60) return itemManager.GetRandomItemByRarity("rare");
            else if (rand < 90) return itemManager.GetRandomItemByRarity("unique");
            else return itemManager.GetRandomItemByRarity("legendary");
        }
        else    // �Ϲݰ�í Ŀ��70% ����25% ����ũ5%
        {
            if (rand < 70) return itemManager.GetRandomItemByRarity("common");
            else if (rand < 95) return itemManager.GetRandomItemByRarity("rare");
            else return itemManager.GetRandomItemByRarity("unique");
        }
    }

    private void ShowGachaResult(List<Item> items)
    {
        // ���â UI �ʱ�ȭ
        foreach (Transform child in resultItemContainer)
        {
            Destroy(child.gameObject);
        }

        // ������ ǥ�� (������ �����ܰ� �̸�)
        foreach (Item item in items)
        {
            GameObject slot = Instantiate(gachaItemPrefab, resultItemContainer);
            // GachaItem �����տ��� "ItemIcon"�� "ItemName" ������Ʈ�� �־�� ��.
            Image iconImage = slot.transform.Find("ItemIcon")?.GetComponent<Image>();
            TextMeshProUGUI nameText = slot.transform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();

            if (iconImage != null) iconImage.sprite = item.icon;
            if (nameText != null) nameText.text = item.name;
        }

        // 1pull�� ��� �߾� ��ġ, 10pull�� ��� �� �ٿ� 5���� 2�� ǥ��
        GridLayoutGroup grid = resultItemContainer.GetComponent<GridLayoutGroup>();
        if (items.Count == 1)
        {
            grid.constraintCount = 1;
        }
        else
        {
            grid.constraintCount = 5;
        }

        gachaResultPanel.SetActive(true);
    }

    private void CloseGachaResult()
    {
        gachaResultPanel.SetActive(false);
    }

    private void ReducePlayerGold(int amount)
    {
        GameManager.Instance.UpdateGold(-amount); // ��� ����
        UpdateGoldUI(); // UI ������Ʈ
    }
}
