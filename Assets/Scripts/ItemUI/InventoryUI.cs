using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour    //�κ��丮 ȭ�� UI
{
    public Transform inventorySlotGrid;
    public GameObject itemSlotPrefab;
    public ScrollRect inventoryScrollView;
    public GameObject itemInfoPanel;

    private List<GameObject> itemSlots = new List<GameObject>();

    private const int maxVisibleRows = 4;
    private const int itemsPerRow = 5;

    private void Start()
    {
        if (inventoryScrollView == null)
            inventoryScrollView = GetComponentInChildren<ScrollRect>();

        if (inventorySlotGrid == null)
            inventorySlotGrid = inventoryScrollView.content;

        RefreshInventory(PlayerInventoryManager.Instance.GetOwnedItems());
    }

    public void RefreshInventory(List<Item> items)
    {
        foreach (var slot in itemSlots)
            Destroy(slot);
        itemSlots.Clear();

        foreach (Item item in items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, inventorySlotGrid);
            Debug.Log($"�κ��丮�� ������ �߰���: {item.name}");
            ItemSlot itemSlot = slot.GetComponent<ItemSlot>();

            itemSlot.itemInfoPanel = itemInfoPanel;
            itemSlot.Setup(item);

            itemSlots.Add(slot);
        }
        Debug.Log("�κ��丮 UI ���ŵ�.");

        // Scroll View ����
        int totalRows = Mathf.CeilToInt((float)items.Count / itemsPerRow);
        totalRows = Mathf.Clamp(totalRows, 1, maxVisibleRows);

        float slotHeight = itemSlots.Count > 0 ? itemSlots[0].GetComponent<RectTransform>().sizeDelta.y : 100f;
        inventorySlotGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
            inventorySlotGrid.GetComponent<RectTransform>().sizeDelta.x,
            totalRows * slotHeight
        );

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            Debug.Log("UI �Է� �ٽ� Ȱ��ȭ�� (RefreshInventory ����)");
        }
    }
}
