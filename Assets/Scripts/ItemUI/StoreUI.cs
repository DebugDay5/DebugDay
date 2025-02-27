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
    public Image highGachaImage;
    public Image lowGachaImage;

    private ItemManager itemManager;
    private PlayerInventoryManager inventoryManager;

    private int PlayerGold => GameManager.Instance.Gold;

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

    private IEnumerator WaitForPlayerManager()
    {
        while (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager�� ã�� �� ����. ��� ��...");
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("GameManager �߰ߵ�!");
        UpdateGoldUI();
    }

    private void Start()
    {
        WaitForPlayerManager();

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

        ReducePlayerGold(cost);
        UpdateGoldUI();

        List<Item> obtainedItems = new List<Item>();
        for (int i = 0; i < count; i++)
        {
            Item item = GetRandomItem(gachaType, itemManager);
            obtainedItems.Add(item);
            PlayerInventoryManager.Instance.AddItem(item);
        }

        DisplayPulledItem(gachaType, obtainedItems[0]);
    }

    private Item GetRandomItem(string gachatype, ItemManager itemManager)
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

    private void DisplayPulledItem(string gachaType, Item item)
    {
        if (gachaType == "high")
            highGachaImage.sprite = item.icon;
        else
            lowGachaImage.sprite = item.icon;

        Debug.Log($"ȹ���� ������ : {item.name} ({item.rarity})");
    }

    private void ReducePlayerGold(int amount)
    {
        typeof(GameManager).GetProperty("Gold")?.SetValue(GameManager.Instance, PlayerGold - amount);   // reflection
    }
}
