using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour   // 인벤토리 화면 관리
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

        // 기존 슬롯 삭제
        foreach (Transform child in inventorySlotGrid)
        {
            Destroy(child.gameObject);
        }

        // 새 슬롯 생성
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
