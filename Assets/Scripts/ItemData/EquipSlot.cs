using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour      // 장비 슬롯 관리
{
    public string itemType; // 각 장착슬롯에 장착가능한 아이템타입

    public Image itemIcon;
    public GameObject itemInfoPanel; // 아이템 정보 패널 (UI)
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStats;
    public Button unEquipButton;
    public Button equipButton;
    public Button enhanceButton;
    public Button closeButton;

    private Item equippedItem;  // 현재 장착된 아이템

    public void Setup(Item item)
    {
        UpdateSlot(item);

        if (item != null)
            ApplyItemStats(item, true);
    }

    public void UpdateSlot(Item newItem)
    {
        if (newItem == null)
        {
            Debug.LogError("UpdateSlot()에서 newItem이 NULL입니다!");
            return;
        }

        equippedItem = newItem;
        itemIcon.sprite = newItem.icon;
        itemIcon.enabled = true;
        Debug.Log($"EquipSlot 업데이트 완료: {newItem.name}");
    }

    public void OnClick()
    {
        if (equippedItem == null) return; // 장착된 아이템이 없으면 아무 동작 안함

        if (itemInfoPanel.activeSelf)
            CloseItemInfoPanel();
        else
        {
            itemInfoPanel.SetActive(true);

            itemName.text = equippedItem.name;
            itemStats.text = GetStatInfo();

            equipButton.gameObject.SetActive(false);
            unEquipButton.gameObject.SetActive(true);
            unEquipButton.onClick.RemoveAllListeners();
            enhanceButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
            unEquipButton.onClick.AddListener(UnequipItem);
            enhanceButton.onClick.AddListener(EnhanceItem);
            closeButton.onClick.AddListener(CloseItemInfoPanel);
        }
    }

    public void CloseItemInfoPanel()
    {
        itemInfoPanel.SetActive(false);
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
        GameManager gameManager = GameManager.Instance;

        Debug.Log($"{equippedItem.name} 장착 해제");

        foreach (var stat in equippedItem.stats)
        {
            int statCode = stat.Key;
            float statValue = stat.Value;
            gameManager.UpdateStat(-statValue, (PlayerManager.PlayerStat)statCode);
            Debug.Log($"스탯 감소: {statCode} - {statValue}");
        }

        inventoryManager.AddItem(equippedItem); // 장착 해제한 아이템은 인벤토리로 이동
        inventoryManager.UnequipItem(itemType);

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

        equippedItem = null;
        itemIcon.enabled = false;
        CloseItemInfoPanel();
    }

    private void EnhanceItem()
    {
        Debug.Log($"{equippedItem.name} 강화");
        CloseItemInfoPanel();
    }

    private void ApplyItemStats(Item item, bool isEquip)
    {
        float multiplier = isEquip ? 1f : -1f; // 장착 시 스탯 증가, 해제 시 스탯 감소

        foreach (var stat in item.stats)
        {
            PlayerManager.Instance.UpdateStat(stat.Value * multiplier, (PlayerManager.PlayerStat)stat.Key);
        }
    }
}
