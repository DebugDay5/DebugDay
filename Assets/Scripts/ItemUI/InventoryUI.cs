using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour    //�κ��丮 ȭ�� UI
{
    public Transform inventorySlotGrid;
    public GameObject itemSlotPrefab;

    public GameObject itemInfoPanel;
    public GameObject closePanel;

    public void RefreshInventory(List<Item> items)
    {
        foreach (Transform child in inventorySlotGrid)
            Destroy(child.gameObject);

        foreach (Item item in items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, inventorySlotGrid);
            slot.GetComponent<ItemSlot>().Setup(item);
        }
    }
}
