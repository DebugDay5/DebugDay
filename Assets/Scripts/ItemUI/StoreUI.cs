using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreUI : MonoBehaviour
{
    public static StoreUI Instance;

    [Header("UI Elements")]
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

    private int playerGold = 10000; // 플레이어의 소지골드 가져오게 되면 이거 지워야됨

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Start()
    {

        UpdateGoldUI();

        highGachaOnePull.onClick.AddListener(() => PullItem("high", 1));
        highGachaTenPull.onClick.AddListener(() => PullItem("high", 10));
        lowGachaOnePull.onClick.AddListener(() => PullItem("low", 1));
        lowGachaTenPull.onClick.AddListener(() => PullItem("low", 10));
    }

    private void UpdateGoldUI()
    {
        goldText.text = $"GOLD {playerGold}";
    }

    private void PullItem(string gachaType, int count)
    {
        int cost = (gachaType == "high") ? 2000 : 500;
        cost *= count;

        if (playerGold < cost)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        playerGold -= cost;
        UpdateGoldUI();

        ItemManager itemManager = ItemManager.Instance;
        PlayerInventoryManager inventoryManager = PlayerInventoryManager.Instance;

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
        else
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
}
