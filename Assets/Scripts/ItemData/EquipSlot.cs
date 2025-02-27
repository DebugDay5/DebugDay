using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour      // ��� ���� ����
{
    public string itemType; // �� �������Կ� ���������� ������Ÿ��

    public Image itemIcon;
    public GameObject itemInfoPanel; // ������ ���� �г� (UI)
    public GameObject closePanel;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStats;
    public Button unequipButton;
    public Button equipButton;
    public Button enhanceButton;

    private Item equippedItem;  // ���� ������ ������

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
        if (equippedItem == null) return; // ������ �������� ������ �ƹ� ���� ����

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

        Debug.Log($"{equippedItem.name} ���� ����");

        foreach (var stat in equippedItem.stats)
        {
            int statCode = stat.Key;
            float statValue = stat.Value;
            gameManager.UpdateStat(-statValue, (PlayerManager.PlayerStat)statCode);
            Debug.Log($"���� ����: {statCode} - {statValue}");
        }

        inventoryManager.AddItem(equippedItem); // ���� ������ �������� �κ��丮�� �̵�
        inventoryManager.UnequipItem(itemType);

        equippedItem = null;
        itemIcon.enabled = false;
        CloseItemInfoPanel();
    }

    private void EnhanceItem()
    {
        Debug.Log($"{equippedItem.name} ��ȭ");
        CloseItemInfoPanel();
    }

    private void ApplyItemStats(Item item, bool isEquip)
    {
        float multiplier = isEquip ? 1f : -1f; // ���� �� ���� ����, ���� �� ���� ����

        foreach (var stat in item.stats)
        {
            PlayerManager.Instance.UpdateStat(stat.Value * multiplier, (PlayerManager.PlayerStat)stat.Key);
        }
    }
}
