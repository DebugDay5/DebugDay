using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour    //�κ��丮 ȭ�� UI
{
    public Transform inventoryPanel;
    public GameObject itemSlotPrefab;

    public void RefreshInventory(List<Item> items)
    {
        foreach (Transform child in inventoryPanel)
            Destroy(child.gameObject);

        foreach (Item item in items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, inventoryPanel);
            slot.GetComponent<ItemSlot>().Setup(item);
        }
    }
}
