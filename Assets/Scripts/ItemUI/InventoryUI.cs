using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour    //인벤토리 화면 UI
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
            Debug.Log($"인벤토리에 아이템 추가됨: {item.name}");
            ItemSlot itemSlot = slot.GetComponent<ItemSlot>();

            itemSlot.itemInfoPanel = itemInfoPanel;

            itemSlot.Setup(item);
        }
    }
}
