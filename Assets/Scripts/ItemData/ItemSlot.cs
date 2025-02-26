using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // �κ��丮 ȭ�� �����۽��Կ� ������ ��ġ
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

            Debug.Log($"EquipButton �Ҵ��: {equipButton != null}");
            Debug.Log($"EnhanceButton �Ҵ��: {enhanceButton != null}");
            Debug.Log($"CloseButton �Ҵ��: {closeButton != null}");

            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(CloseItemInfoPanel);
                Debug.Log("CloseButton Ŭ�� ������ �߰���");
            }
            else
            {
                Debug.LogError("CloseButton�� ã�� �� �����ϴ�!", this);
            }
        }
        else
        {
            Debug.LogError("ItemInfoPanel�� ã�� �� �����ϴ�!", this);
        }
    }

    public void Setup(Item item)
    {
        Debug.Log($"Setup() ����� itemData: {(item != null ? item.name : "NULL")}");
        itemData = item;

        if (itemData == null)
        {
            Debug.LogError("Setup()���� itemData�� NULL - ItemManager���� ���������� �����Ͱ� ���޵Ǵ��� Ȯ�� �ʿ���");
            return;
        }

        Debug.Log($"Setup() ����� itemData: {itemData.name}");

        if (item.icon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;
        }
        else
            itemIcon.enabled = false;

        if (itemInfoPanel != null) itemInfoPanel.SetActive(false);
    }

    public void OnClick()   // �κ��丮�� ������ ���� Ŭ�� �� ������ ����â�� ������ ��ȭ, ������ ����
    {
        Debug.Log($"OnClick() ȣ�� itemData: {(itemData != null ? itemData.name : "NULL")}");

        if (itemData == null)
        {
            Debug.LogError("OnClick()���� itemData�� NULL�Դϴ�");
            return;
        }

        if (itemInfoPanel == null)
        {
            Debug.LogError("OnClick()���� itemInfoPanel�� NULL�Դϴ�");
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
            Debug.LogError("equipButton�� NULL�Դϴ�!", this);
            return;
        }

        if (enhanceButton == null)
        {
            Debug.LogError("enhanceButton�� NULL�Դϴ�!", this);
            return;
        }

        equipButton.onClick.RemoveAllListeners();
        enhanceButton.onClick.RemoveAllListeners(); // �ߺ� ����

        equipButton.onClick.AddListener(() => {
            Debug.Log("EquipButton ������");
            EquipItem();
        });

        enhanceButton.onClick.AddListener(() => {
            Debug.Log("EnhanceButton ������");
            EnhanceItem();
        });

        Debug.Log("EquipButton & EnhanceButton Ŭ�� ������ �߰���");
    }

    public void CloseItemInfoPanel()    // ������ ����â �ٱ� ������ ����â �������� - ��ư������ �ٲ�
    {
        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);

            CanvasGroup canvasGroup = itemInfoPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.blocksRaycasts = true;  // UI �Է� Ȱ��ȭ
                canvasGroup.interactable = true;
                Debug.Log("ItemInfoPanel ����, UI �Է� �ٽ� Ȱ��ȭ��.");
            }

            EventSystem.current.SetSelectedGameObject(null); // UI �Է� �ʱ�ȭ

            Debug.Log("ItemInfoPanel ����, UI �Է� �ٽ� Ȱ��ȭ��.");
        }
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

    public void EquipItem()    // ������ư
    {
        Debug.Log($"EquipItem() ����� itemData ����: {(itemData != null ? itemData.name : "NULL")}");
        if (itemData == null)
        {
            Debug.LogError("EquipItem()���� itemData�� NULL�Դϴ� OnClick() ���� �� itemData�� �ʱ�ȭ�Ǿ����� Ȯ���ϼ���.");
            return;
        }

        Debug.Log($"EquipItem() ����� itemData ����: {itemData.name}");

        var inventoryManager = PlayerInventoryManager.Instance;
        if (inventoryManager == null)
        {
            Debug.LogError("PlayerInventoryManager �ν��Ͻ��� ã�� �� ����");
            return;
        }

        string itemType = itemData.type;

        EquipSlot equipSlot = FindEquipSlot(itemType);

        if (equipSlot == null)
        {
            Debug.LogError($"EquipSlot�� ã�� �� ����: {itemType}", this);
            return;
        }

        // ���� ���� ������ ����(�����Ѵٸ�)
        if (inventoryManager.IsEquipped(itemType))
        {
            Item unequippedItem = inventoryManager.GetEquippedItem(itemType);
            equipSlot.UnequipItem();

            // ������ �������� �κ��丮�� �߰�
            if (unequippedItem != null)
            {
                inventoryManager.AddItem(unequippedItem);
                Debug.Log($"���� ������ {unequippedItem.name} ���� �� �κ��丮�� �߰���");
            }
        }

        // �� ������ ����
        inventoryManager.EquipItem(itemType, itemData);
        Debug.Log($"{itemData.name} ������");

        equipSlot.UpdateSlot(itemData);

        inventoryManager.RemoveItem(itemData);
        Debug.Log($"{itemData.name} �κ��丮���� ���ŵ�");

        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.RefreshInventory(inventoryManager.GetOwnedItems());
            Debug.Log("�κ��丮 UI ���ŵ�.");
        }
        else
        {
            Debug.LogError("InventoryUI�� ã�� �� ����.");
        }

        CloseItemInfoPanel();
    }

    public void EnhanceItem()  // ��ȭ��ư
    {
        Debug.Log($"{itemData.name} ��ȭ��");
        itemInfoPanel.SetActive(false);
    }

    private EquipSlot FindEquipSlot(string itemType)
    {
        Debug.Log($"{itemType} ���� ���� ã�� ��");
        EquipSlot[] equipSlots = FindObjectsOfType<EquipSlot>();
        foreach (var slot in equipSlots)
        {
            Debug.Log($"{slot.itemType} {slot.name} �߰���");
            if (slot.itemType.Equals(itemType, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"{itemType} ���� ���� ã��: {slot.name}");
                return slot;
            }
        }
        Debug.LogError($"{itemType} ���� ������ ã�µ� ������");
        return null;
    }
}
