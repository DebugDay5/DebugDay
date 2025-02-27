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
    public GameObject gachaResultPanel;          // 가챠 결과 전체 패널 (비활성화 상태)
    public Transform resultItemContainer;          // GridLayoutGroup이 적용된 컨테이너
    public GameObject gachaItemPrefab;             // GachaItem 프리팹 (아이콘 + 이름)
    public Button retryButton;                     // "한 번 더" 버튼
    public Button closeResultButton;               // 결과창 닫기 버튼

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

        Debug.Log("StoreUI.cs의 Awake() 실행됨");
    }

    

    private void Start()
    {
        UpdateGoldUI();

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
            Debug.Log("골드가 부족합니다.");
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

    private void ShowGachaResult(List<Item> items)
    {
        // 결과창 UI 초기화
        foreach (Transform child in resultItemContainer)
        {
            Destroy(child.gameObject);
        }

        // 아이템 표시 (아이템 아이콘과 이름)
        foreach (Item item in items)
        {
            GameObject slot = Instantiate(gachaItemPrefab, resultItemContainer);
            // GachaItem 프리팹에서 "ItemIcon"과 "ItemName" 오브젝트가 있어야 함.
            Image iconImage = slot.transform.Find("ItemIcon")?.GetComponent<Image>();
            TextMeshProUGUI nameText = slot.transform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();

            if (iconImage != null) iconImage.sprite = item.icon;
            if (nameText != null) nameText.text = item.name;
        }

        // 1pull인 경우 중앙 배치, 10pull인 경우 한 줄에 5개씩 2줄 표시
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
        GameManager.Instance.UpdateGold(-amount); // 골드 차감
        UpdateGoldUI(); // UI 업데이트
    }
}
