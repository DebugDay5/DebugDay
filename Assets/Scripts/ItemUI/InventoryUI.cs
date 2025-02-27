using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour    //�κ��丮 ȭ�� UI
{
    public Transform inventorySlotGrid;
    public GameObject itemSlotPrefab;

    public GameObject itemInfoPanel;

    public void RefreshInventory(List<Item> items)
    {
        foreach (Transform child in inventorySlotGrid)
            Destroy(child.gameObject);

        foreach (Item item in items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, inventorySlotGrid);
            Debug.Log($"�κ��丮�� ������ �߰���: {item.name}");
            ItemSlot itemSlot = slot.GetComponent<ItemSlot>();

            itemSlot.itemInfoPanel = itemInfoPanel;

            itemSlot.Setup(item);
        }
        Debug.Log("�κ��丮 UI ���ŵ�.");
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            Debug.Log("UI �Է� �ٽ� Ȱ��ȭ�� (RefreshInventory ����)");
        }
    }
}
