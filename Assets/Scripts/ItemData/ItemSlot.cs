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
        if (itemData == null)
        {
            Debug.LogError("OnClick()���� itemData�� NULL�Դϴ�. Setup()�� ���������� ����Ǿ����� Ȯ���ϼ���.");

            // itemData�� ������ �ʱ�ȭ�ϴ� ���� �߰�
            if (itemIcon != null && inventoryUI != null)
            {
                Debug.Log("OnClick()���� itemData ���� �ʱ�ȭ �õ�");
                List<Item> ownedItems = PlayerInventoryManager.Instance.GetOwnedItems();
                foreach (var item in ownedItems)
                {
                    if (item.icon == itemIcon.sprite) // ���� ���԰� ��ġ�ϴ� �������� ã��
                    {
                        itemData = item;
                        Debug.Log($"OnClick()���� itemData ���� �ʱ�ȭ �Ϸ�: {itemData.name}");
                        break;
                    }
                }
            }

            if (itemData == null)
            {
                Debug.LogError("OnClick()���� itemData�� ���� �ʱ�ȭ�� �� ����.");
                return;
            }
        }

        Debug.Log($"OnClick() ȣ�� itemData: {itemData.name}");

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

        if (equipButton == null || enhanceButton == null)
        {
            Debug.LogError("OnClick()���� ��ư �Ҵ��� �߸���.");
            return;
        }

        equipButton.onClick.RemoveAllListeners();
        enhanceButton.onClick.RemoveAllListeners();

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
                canvasGroup.blocksRaycasts = true;  // UI �Է� Ȱ��ȭ
                canvasGroup.interactable = true;
                Debug.Log("ItemInfoPanel ����, UI �Է� �ٽ� Ȱ��ȭ��.");
            }

            /*Debug.Log("EventSystem.current.SetSelectedGameObject(null) ����");
            EventSystem.current.SetSelectedGameObject(null); // UI �Է� �ʱ�ȭ*/ // ��� ������ ģ��

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
            Debug.LogError("PlayerInventoryManager �ν��Ͻ��� NULL");
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
            Debug.Log($"{itemType} ���Կ��� ���� ���� ������ ���� ��...");
            equipSlot.UnequipItem();
        }

        // ������ ����
        inventoryManager.EquipItem(itemType, itemData);
        equipSlot.UpdateSlot(itemData);
        Debug.Log($"{itemData.name} ������");

        // ������ ������ �κ��丮���� ����
        inventoryManager.RemoveItem(itemData);
        Debug.Log($"{itemData.name} �κ��丮���� ���ŵ�");

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

        Debug.Log("CloseItemInfoPanel ����");
        CloseItemInfoPanel();

        /*Debug.Log("EventSystem.current.SetSelectedGameObject(null) ����");
        EventSystem.current.SetSelectedGameObject(null);*/  // ��� ������
    }

    public void EnhanceItem()  // ��ȭ��ư
    {
        if (itemData == null)
        {
            Debug.LogError("EnhanceItem()���� itemData�� NULL�Դϴ�.");
            return;
        }

        Debug.Log($"{itemData.name} ��ȭ��");

        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("EnhanceItem()���� itemInfoPanel�� NULL�Դϴ�.");
        }
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
