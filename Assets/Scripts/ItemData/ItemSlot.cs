using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // 인벤토리 화면 아이템슬롯에 아이템 배치
{
    public Image itemIcon;
    public GameObject itemInfoPanel;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStats;
    public Button equipButton;
    public Button unEquipButton;
    public Button enhanceButton;
    public Button closeButton;

    private Item itemData;
    private InventoryUI inventoryUI;

    public void Start()
    {
        if (inventoryUI == null)
            inventoryUI = FindObjectOfType<InventoryUI>();

        if (inventoryUI != null)
        {
            itemInfoPanel = inventoryUI.itemInfoPanel;
        }

        if (itemInfoPanel != null)
        {
            itemName = itemInfoPanel.transform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();
            itemStats = itemInfoPanel.transform.Find("ItemStats")?.GetComponent<TextMeshProUGUI>();
            equipButton = itemInfoPanel.transform.Find("EquipButton")?.GetComponent<Button>();
            unEquipButton = itemInfoPanel.transform.Find("UnequipButton")?.GetComponent<Button>();
            enhanceButton = itemInfoPanel.transform.Find("EnhanceButton")?.GetComponent<Button>();
            closeButton = itemInfoPanel.transform.Find("CloseButton")?.GetComponent<Button>();

            Debug.Log($"EquipButton 할당됨: {equipButton != null}");
            Debug.Log($"EnhanceButton 할당됨: {enhanceButton != null}");
            Debug.Log($"CloseButton 할당됨: {closeButton != null}");

            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(CloseItemInfoPanel);
                Debug.Log("CloseButton 클릭 리스너 추가됨");
            }
            else
            {
                Debug.LogError("CloseButton을 찾을 수 없습니다!", this);
            }
        }
        else
        {
            Debug.LogError("ItemInfoPanel을 찾을 수 없습니다!", this);
        }
    }

    public void Setup(Item item)
    {
        Debug.Log($"Setup() 실행됨 itemData: {(item != null ? item.name : "NULL")}");  // Setup() 실행됨 itemData: 정교한 활
        itemData = item;

        if (itemData == null)
        {
            Debug.LogError("Setup()에서 itemData가 NULL - ItemManager에서 정상적으로 데이터가 전달되는지 확인 필요함"); // 이 메세지는 안뜨고있음
            return;
        }

        Debug.Log($"Setup() 실행됨 itemData: {itemData.name}");    // 디버그 아이템 이름 들어간걸로 두번뜨는게 정상임

        if (item.icon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;
        }
        else
            itemIcon.enabled = false;

        if (itemInfoPanel != null) itemInfoPanel.SetActive(false);
    }

    public void OnClick()   // 인벤토리의 아이템 슬롯 클릭 시 아이템 정보창이 나오고 강화, 장착을 선택
    {
        if (itemData == null)
        {
            Debug.LogError("OnClick()에서 itemData가 NULL입니다. Setup()이 정상적으로 실행되었는지 확인하세요.");

            // itemData를 강제로 초기화하는 로직 추가 - 강제 초기화 시도 로그도 뜬적 없음
            if (itemIcon != null && inventoryUI != null)
            {
                Debug.Log("OnClick()에서 itemData 강제 초기화 시도");
                List<Item> ownedItems = PlayerInventoryManager.Instance.GetOwnedItems();
                foreach (var item in ownedItems)
                {
                    if (item.icon == itemIcon.sprite) // 현재 슬롯과 일치하는 아이템을 찾음
                    {
                        itemData = item;
                        Debug.Log($"OnClick()에서 itemData 강제 초기화 완료: {itemData.name}");
                        break;
                    }
                }
            }

            if (itemData == null)
            {
                Debug.LogError("OnClick()에서 itemData를 강제 초기화할 수 없음.");
                return;
            }
        }

        Debug.Log($"OnClick() 호출 itemData: {itemData.name}");

        if (itemInfoPanel == null)
        {
            Debug.LogError("OnClick()에서 itemInfoPanel이 NULL입니다");
            return;
        }

        itemInfoPanel.SetActive(true);
        itemInfoPanel.transform.SetAsLastSibling();

        equipButton.gameObject.SetActive(true);
        unEquipButton.gameObject.SetActive(false);

        RectTransform infoPanelRect = itemInfoPanel.GetComponent<RectTransform>();
        infoPanelRect.anchoredPosition = Vector2.zero;

        itemName.text = itemData.name;
        itemStats.text = GetStatInfo();

        if (equipButton == null || enhanceButton == null)
        {
            Debug.LogError("OnClick()에서 버튼 할당이 잘못됨.");
            return;
        }

        equipButton.onClick.RemoveAllListeners();
        enhanceButton.onClick.RemoveAllListeners();

        equipButton.onClick.AddListener(() => {
            Debug.Log($"EquipButton 눌러짐 - itemData 상태: {(itemData != null ? itemData.name : "NULL")}"); // EquipButton 눌러짐 - itemData 상태: 일반 활
            EquipItem();
        });

        enhanceButton.onClick.AddListener(() => {
            Debug.Log("EnhanceButton 눌러짐");
            EnhanceItem();
        });

        Debug.Log("EquipButton & EnhanceButton 클릭 리스너 추가됨");
    }

    public void CloseItemInfoPanel()    // 아이템 정보창 바깥 누르면 정보창 닫히도록 - 버튼식으로 바꿈
    {
        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);

            CanvasGroup canvasGroup = itemInfoPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = true;  // UI 입력 활성화
                canvasGroup.interactable = true;
                Debug.Log("ItemInfoPanel 닫힘, UI 입력 다시 활성화됨.");
            }

            /*Debug.Log("EventSystem.current.SetSelectedGameObject(null) 실행");
            EventSystem.current.SetSelectedGameObject(null); // UI 입력 초기화*/ // 없어도 무방한 친구

            Debug.Log("ItemInfoPanel 닫힘, UI 입력 다시 활성화됨.");
        }
    }

    private string GetStatInfo()
    {
        var statManager = ItemStatManager.Instance;
        if (statManager == null)
        {
            Debug.LogError("ItemStatManager의 인스턴스가 NULL");
            return "";
        }
        string statInfo = "";
        foreach (var stat in itemData.stats)
        {
            string statName = statManager.GetStatName(stat.Key);    // 이부분 충돌날 가능성 매우높음
            statInfo += $"{statName}: {stat.Value}\n";
        }
        return statInfo;
    }

    public void EquipItem()    // 장착버튼
    {
        Debug.Log($"EquipItem() 실행됨 itemData 상태: {(itemData != null ? itemData.name : "NULL")}");     // 여기서 NULL이었음 이 이전시점에 NULL인지 확인필요
        if (itemData == null)
        {
            Debug.LogError("EquipItem()에서 itemData가 NULL입니다 OnClick() 실행 후 itemData가 초기화되었는지 확인하세요.");
            return;
        }

        Debug.Log($"EquipItem() 실행됨 itemData 상태: {itemData.name}"); // 여기선 갑자기 데이터 있음 - itemData는 null인데 itemData.name는 null이 아닐 수가 있나?

        var inventoryManager = PlayerInventoryManager.Instance;
        if (inventoryManager == null)
        {
            Debug.LogError("PlayerInventoryManager 인스턴스가 NULL");
            return;
        }
        var gameManager = GameManager.Instance;

        string itemType = itemData.type;

        EquipSlot equipSlot = FindEquipSlot(itemType);

        if (equipSlot == null)
        {
            Debug.LogError($"EquipSlot을 찾을 수 없음: {itemType}", this);
            return;
        }

        // 기존 장착 아이템 해제(존재한다면)
        if (inventoryManager.IsEquipped(itemType))
        {
            Debug.Log($"{itemType} 슬롯에서 기존 장착 아이템 해제 중...");
            equipSlot.UnequipItem();
        }

        foreach (var stat in itemData.stats)
        {
            int statCode = stat.Key;
            float statValue = stat.Value;
            gameManager.UpdateStat(statValue, (PlayerManager.PlayerStat)statCode);
            Debug.Log($"스탯 증가: {statCode} + {statValue}");
        }

        // 아이템 장착
        inventoryManager.EquipItem(itemType, itemData);
        equipSlot.UpdateSlot(itemData);
        Debug.Log($"{itemData.name} 장착됨");

        // 장착한 아이템 인벤토리에서 제거
        inventoryManager.RemoveItem(itemData);
        Debug.Log($"{itemData.name} 인벤토리에서 제거됨");

        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI를 찾을 수 없음 - InventoryUI를 강제로 찾음");
            inventoryUI = GameObject.Find("InventoryPanel")?.GetComponent<InventoryUI>();
        }

        if (inventoryUI != null)
        {
            inventoryUI.RefreshInventory(inventoryManager.GetOwnedItems());
            Debug.Log("인벤토리 UI 갱신됨.");
        }
        else
        {
            Debug.LogError("InventoryUI 찾기 실패 - UI 갱신 불가능.");
        }

        Debug.Log("CloseItemInfoPanel 실행");
        CloseItemInfoPanel();

    }

    public void EnhanceItem()  // 강화버튼
    {
        if (itemData == null)
        {
            Debug.LogError("EnhanceItem()에서 itemData가 NULL입니다.");
            return;
        }

        Debug.Log($"{itemData.name} 강화함");

        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("EnhanceItem()에서 itemInfoPanel이 NULL입니다.");
        }
    }

    private EquipSlot FindEquipSlot(string itemType)
    {
        Debug.Log($"{itemType} 장착 슬롯 찾는 중");
        EquipSlot[] equipSlots = FindObjectsOfType<EquipSlot>();
        foreach (var slot in equipSlots)
        {
            Debug.Log($"{slot.itemType} {slot.name} 발견함");
            if (slot.itemType.Equals(itemType, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"{itemType} 장착 슬롯 찾음: {slot.name}");
                return slot;
            }
        }
        Debug.LogError($"{itemType} 장착 슬롯을 찾는데 실패함");
        return null;
    }
}
