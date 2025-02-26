using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // 인벤토리 화면 아이템슬롯에 아이템 배치
{
    public Image itemIcon;
    public GameObject itemInfoPanel;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStats;
    public Button equipButton;
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
        Debug.Log($"Setup() 실행됨 itemData: {(item != null ? item.name : "NULL")}");
        itemData = item;

        if (itemData == null)
        {
            Debug.LogError("Setup()에서 itemData가 NULL - ItemManager에서 정상적으로 데이터가 전달되는지 확인 필요함");
            return;
        }

        Debug.Log($"Setup() 실행됨 itemData: {itemData.name}");

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
        Debug.Log($"OnClick() 호출 itemData: {(itemData != null ? itemData.name : "NULL")}");

        if (itemData == null)
        {
            Debug.LogError("OnClick()에서 itemData가 NULL입니다");
            return;
        }

        if (itemInfoPanel == null)
        {
            Debug.LogError("OnClick()에서 itemInfoPanel이 NULL입니다");
            return;
        }

        itemInfoPanel.SetActive(true);
        itemInfoPanel.transform.SetAsLastSibling();

        RectTransform infoPanelRect = itemInfoPanel.GetComponent<RectTransform>();
        infoPanelRect.anchoredPosition = Vector2.zero;

        itemName.text = itemData.name;
        itemStats.text = GetStatInfo();

        if (equipButton == null)
        {
            Debug.LogError("equipButton이 NULL입니다!", this);
            return;
        }

        if (enhanceButton == null)
        {
            Debug.LogError("enhanceButton이 NULL입니다!", this);
            return;
        }

        equipButton.onClick.RemoveAllListeners();
        enhanceButton.onClick.RemoveAllListeners(); // 중복 방지

        equipButton.onClick.AddListener(() => {
            Debug.Log("EquipButton 눌러짐");
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
            itemInfoPanel.SetActive(false);
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
            string statName = statManager.GetStatName(stat.Key);
            statInfo += $"{statName}: {stat.Value}\n";
        }
        return statInfo;
    }

    public void EquipItem()    // 장착버튼
    {
        Debug.Log($"EquipItem() 실행됨 itemData 상태: {(itemData != null ? itemData.name : "NULL")}");
        if (itemData == null)
        {
            Debug.LogError("EquipItem()에서 itemData가 NULL입니다 OnClick() 실행 후 itemData가 초기화되었는지 확인하세요.");
            return;
        }

        var inventoryManager = PlayerInventoryManager.Instance;
        if (inventoryManager == null) return;

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
            equipSlot.UnequipItem();
        }

        // 새 아이템 장착
        inventoryManager.EquipItem(itemType, itemData);
        Debug.Log($"{itemData.name} 장착됨");

        equipSlot.UpdateSlot(itemData);
    }

    public void EnhanceItem()  // 강화버튼
    {
        Debug.Log($"{itemData.name} 강화함");
        itemInfoPanel.SetActive(false);
    }

    private EquipSlot FindEquipSlot(string itemType)
    {
        Debug.Log($"{itemType} 장착 슬롯 찾는 중");
        EquipSlot[] equipSlots = FindObjectsOfType<EquipSlot>();
        foreach (var slot in equipSlots)
        {
            Debug.Log($"{slot.itemType} {slot.name} 발견함");
            if (slot.itemType == itemType)
                return slot;
        }
        Debug.LogError($"{itemType} 장착 슬롯을 찾는데 실패함");
        return null;
    }
}
