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

        Debug.Log("StoreUI.cs의 Awake() 실행됨");
    }

    private IEnumerator WaitForPlayerManager()
    {
        while (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager를 찾을 수 없음. 대기 중...");
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("GameManager 발견됨!");
        UpdateGoldUI();
    }

    private void Start()
    {
        WaitForPlayerManager();

        itemManager = ItemManager.Instance;
        inventoryManager = PlayerInventoryManager.Instance;

        if (itemManager == null)
            Debug.LogError("StoreUI.cs의 Start()에서 ItemManager가 NULL");
        if (inventoryManager == null)
            Debug.LogError("StoreUI.cs의 Start()에서 PlayerInventoryManager가 NULL");

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
            Debug.Log("골드가 부족합니다.");
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

        if (gachatype == "high")    // 고급가챠 레어60% 유니크30% 레전더리10%
        {
            if (rand < 60) return itemManager.GetRandomItemByRarity("rare");
            else if (rand < 90) return itemManager.GetRandomItemByRarity("unique");
            else return itemManager.GetRandomItemByRarity("legendary");
        }
        else    // 일반가챠 커먼70% 레어25% 유니크5%
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

        Debug.Log($"획득한 아이템 : {item.name} ({item.rarity})");
    }

    private void ReducePlayerGold(int amount)
    {
        typeof(GameManager).GetProperty("Gold")?.SetValue(GameManager.Instance, PlayerGold - amount);   // reflection
    }
}
