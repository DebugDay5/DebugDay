using System;
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
    public GameObject closePanel;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStats;
    public Button unequipButton;
    public Button equipButton;
    public Button enhanceButton;

    private Item equippedItem;  // 현재 장착된 아이템

    public void Setup(Item item)
    {
        UpdateSlot(item);

        if (item != null)
            ApplyItemStats(item, true);
    }

    public void UpdateSlot(Item newItem)
    {
        equippedItem = newItem;
        if (equippedItem != null)
        {
            itemIcon.sprite = equippedItem.icon;
            itemIcon.enabled = true;
        }
        else
        {
            itemIcon.enabled = false;
        }
    }

    public void OnClick()
    {
        if (equippedItem == null) return; // 장착된 아이템이 없으면 아무 동작 안함

        if (itemInfoPanel.activeSelf)
            CloseItemInfoPanel();
        else
        {
            itemInfoPanel.SetActive(true);
            closePanel.SetActive(true);

            itemName.text = equippedItem.name;
            itemStats.text = GetStatInfo();

            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(true);
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
        GameManager gameManager = GameManager.Instance;
        PlayerManager playerManager = PlayerManager.Instance;

        if (gameManager == null || playerManager == null || inventoryManager == null)
        {
            Debug.LogError("UnequipItem() 실행 실패 - GameManager, PlayerManager, 또는 PlayerInventoryManager 인스턴스가 NULL입니다.");
            return;
        }

        Debug.Log($"{equippedItem.name} 장착 해제");

        foreach (var stat in equippedItem.stats)
        {
            int statCode = stat.Key;
            float statValue = stat.Value;

            if (Enum.IsDefined(typeof(PlayerManager.PlayerStat), statCode))
            {
                var playerStat = (PlayerManager.PlayerStat)statCode;

                // GameManager와 PlayerManager에 스탯 감소 적용
                gameManager.UpdateStat(-statValue, playerStat);
                playerManager.UpdateStat(-statValue, playerStat);

                Debug.Log($"{playerStat} 스탯 감소: {-statValue}");
            }
            else
            {
                Debug.LogError($"UnequipItem() 오류 - {statCode}는 PlayerStat에 정의되지 않은 값입니다.");
            }
        }

        inventoryManager.AddItem(equippedItem); // 장착 해제한 아이템은 인벤토리로 이동
        inventoryManager.UnequipItem(itemType);

        equippedItem = null;
        itemIcon.enabled = false;

        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.RefreshInventory(inventoryManager.GetOwnedItems());
            Debug.Log("인벤토리 UI 갱신됨.");
        }
        else
        {
            Debug.LogError("InventoryUI 찾기 실패 - UI 갱신 불가능.");
        }

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
