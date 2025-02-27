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
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStats;
    public Button unEquipButton;
    public Button equipButton;
    public Button enhanceButton;
    public Button closeButton;

    private Item equippedItem;  // ���� ������ ������

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
            Debug.LogError("UpdateSlot()���� newItem�� NULL�Դϴ�!");
            return;
        }

        equippedItem = newItem;
        itemIcon.sprite = newItem.icon;
        itemIcon.enabled = true;
        Debug.Log($"EquipSlot ������Ʈ �Ϸ�: {newItem.name}");
    }

    public void OnClick()
    {
        if (equippedItem == null) return; // ������ �������� ������ �ƹ� ���� ����

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

        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI�� ã�� �� ���� - InventoryUI�� ������ ã��");
            inventoryUI = GameObject.Find("InventoryPanel")?.GetComponent<InventoryUI>();
        }

        if (inventoryUI != null)
        {
            inventoryUI.RefreshInventory(inventoryManager.GetOwnedItems());
            Debug.Log("�κ��丮 UI ���ŵ�.");
        }
        else
        {
            Debug.LogError("InventoryUI ã�� ���� - UI ���� �Ұ���.");
        }

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
