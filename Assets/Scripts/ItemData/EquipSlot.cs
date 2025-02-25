using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour      // 장비 슬롯 관리
{
    public static EquipSlot Instance;

    public Image itemIcon;
    public GameObject itemInfoPanel; // 아이템 정보 패널 (UI)
    public GameObject closePanel;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStats;
    public Button unequipButton;
    public Button enhanceButton;

    private Item equippedItem;  // 현재 장착된 아이템

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Setup(Item item)
    {
        equippedItem = item;
        itemIcon.sprite = item.icon;
        itemIcon.enabled = true;
    }

    public void OnClick()
    {
        if (equippedItem == null) return; // 장착된 아이템이 없으면 아무 동작 안함

        if (itemInfoPanel.activeSelf)
        {
            CloseItemInfoPanel();
        }
        else
        {
            itemInfoPanel.SetActive(true);
            closePanel.SetActive(true);
            itemName.text = equippedItem.name;
            itemStats.text = GetStatInfo();
            unequipButton.onClick.RemoveAllListeners();
            enhanceButton.onClick.RemoveAllListeners();
            unequipButton.onClick.AddListener(UnequipItem);
            enhanceButton.onClick.AddListener(EnhanceItem);
        }
    }

    public void CloseItemInfoPanel()
    {
        itemInfoPanel.SetActive(false);
        closePanel.SetActive(false);
    }

    private string GetStatInfo()
    {
        string statInfo = "";
        foreach (var stat in equippedItem.stats)
        {
            string statName = ItemStatManager.Instance.GetStatName(stat.Key);
            statInfo += $"{statName}: {stat.Value}\n";
        }
        return statInfo;
    }

    public void UnequipItem()
    {
        if (equippedItem == null) return;

        PlayerInventoryManager inventoryManager = PlayerInventoryManager.Instance;
        Debug.Log($"{equippedItem.name} 장착 해제");

        inventoryManager.AddItem(equippedItem); // 장착 해제한 아이템은 인벤토리로 이동

        inventoryManager.UnequipItem(equippedItem.type);

        equippedItem = null;
        itemIcon.enabled = false;
        CloseItemInfoPanel();
    }

    private void EnhanceItem()
    {
        Debug.Log($"{equippedItem.name} 강화");
        CloseItemInfoPanel();
    }

    public void UpdateSlot(Item newItem)
    {
        equippedItem = newItem;
        itemIcon.sprite = newItem.icon;
        itemIcon.enabled = true;
    }
}
