using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // 인벤토리 화면 아이템슬롯에 아이템 배치
{
    public Image itemIcon;
    public GameObject itemInfoPanel;
    public GameObject closePanel;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStats;
    public Button equipButton;
    public Button enhanceButton;

    private Item itemData;
    private InventoryUI inventoryUI;

    public void Start()
    {
        if (inventoryUI == null)
            inventoryUI = FindObjectOfType<InventoryUI>();
        if (itemInfoPanel == null && inventoryUI != null)
            itemInfoPanel = inventoryUI.itemInfoPanel;
        if (closePanel == null && inventoryUI != null)
            closePanel = inventoryUI.closePanel;
    }

    public void Setup(Item item)
    {
        itemData = item;

        if (item.icon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;
        }
        else
            itemIcon.enabled = false;

        if (itemInfoPanel == null)
            Debug.LogError("ItemSlot.cs : itemInfoPanel이 할당되지 않음 - 발생위치 Setup(Item item)");

        itemInfoPanel.SetActive(false);
        closePanel.SetActive(false);
    }

    public void OnClick()   // 인벤토리의 아이템 슬롯 클릭 시 아이템 정보창이 나오고 강화, 장착을 선택
    {
        if (itemInfoPanel.activeSelf)
            CloseItemInfoPanel();
        else
        {
            itemInfoPanel.SetActive(true);
            closePanel.SetActive(true);

            RectTransform infoPanelRect = itemInfoPanel.GetComponent<RectTransform>();
            infoPanelRect.anchoredPosition = Vector2.zero;

            itemName.text = itemData.name;
            itemStats.text = GetStatInfo();

            equipButton.onClick.RemoveAllListeners();
            enhanceButton.onClick.RemoveAllListeners(); // 중복 방지
            equipButton.onClick.AddListener(EquipItem);
            enhanceButton.onClick.AddListener(EnhanceItem);
        }
    }

    public void CloseItemInfoPanel()    // 아이템 정보창 바깥 누르면 정보창 닫히도록
    {
        itemInfoPanel.SetActive(false);
        closePanel.SetActive(false);
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

    private void EquipItem()    // 장착버튼
    {
        var inventoryManager = PlayerInventoryManager.Instance;
        var equipslot = EquipSlot.Instance;
        if (inventoryManager == null) return;

        string itemType = itemData.type;

        if (inventoryManager.IsEquipped(itemType))
        {
            equipslot.UnequipItem();
        }

        inventoryManager.EquipItem(itemType, itemData);
        Debug.Log($"{itemData.name} 장착됨");

        equipslot.UpdateSlot(itemData);
    }

    private void EnhanceItem()  // 강화버튼
    {
        Debug.Log($"{itemData.name} 강화함");
        itemInfoPanel.SetActive(false);
    }
}
