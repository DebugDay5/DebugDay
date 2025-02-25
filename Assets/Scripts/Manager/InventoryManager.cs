using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour   // �κ��丮 ȭ�� ����
{
    public static InventoryManager Instance;
    public InventoryUI inventoryUI;
    public Transform inventorySlotGrid;
    public GameObject itemSlotPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RefreshInventory()
    {
        List<Item> items = PlayerInventoryManager.Instance.GetOwnedItems();
        InventorySort.Instance.SortByRarity(items);

        // ���� ���� ����
        foreach (Transform child in inventorySlotGrid)
        {
            Destroy(child.gameObject);
        }

        // �� ���� ����
        foreach (Item item in items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, inventorySlotGrid);
            slot.GetComponent<ItemSlot>().Setup(item);
        }
    }
    public void OnSortByTypeButtonClicked()
    {
        List<Item> items = PlayerInventoryManager.Instance.GetOwnedItems();
        InventorySort.Instance.SortByType(items);
        inventoryUI.RefreshInventory(items);
    }

    public void OnSortByRarityButtonClicked()
    {
        List<Item> items = PlayerInventoryManager.Instance.GetOwnedItems();
        InventorySort.Instance.SortByRarity(items);
        inventoryUI.RefreshInventory(items);
    }
}
