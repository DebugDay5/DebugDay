using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // �κ��丮 ȭ�� �����۽��Կ� ������ ��ġ
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
            Debug.LogError("ItemSlot.cs : itemInfoPanel�� �Ҵ���� ���� - �߻���ġ Setup(Item item)");

        itemInfoPanel.SetActive(false);
        closePanel.SetActive(false);
    }

    public void OnClick()   // �κ��丮�� ������ ���� Ŭ�� �� ������ ����â�� ������ ��ȭ, ������ ����
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
            enhanceButton.onClick.RemoveAllListeners(); // �ߺ� ����
            equipButton.onClick.AddListener(EquipItem);
            enhanceButton.onClick.AddListener(EnhanceItem);
        }
    }

    public void CloseItemInfoPanel()    // ������ ����â �ٱ� ������ ����â ��������
    {
        itemInfoPanel.SetActive(false);
        closePanel.SetActive(false);
    }

    private string GetStatInfo()
    {
        var statManager = ItemStatManager.Instance;
        if (statManager == null)
        {
            Debug.LogError("ItemStatManager�� �ν��Ͻ��� NULL");
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

    private void EquipItem()    // ������ư
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
        Debug.Log($"{itemData.name} ������");

        equipslot.UpdateSlot(itemData);
    }

    private void EnhanceItem()  // ��ȭ��ư
    {
        Debug.Log($"{itemData.name} ��ȭ��");
        itemInfoPanel.SetActive(false);
    }
}
