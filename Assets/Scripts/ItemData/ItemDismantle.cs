using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDismantle : MonoBehaviour  // 아이템 분해
{
    public Button dismantleButton;
    private List<Item> selectedItems = new List<Item>();

    public void ToggleItemSelection(Item item)
    {
        if (item == null) return;

        if (selectedItems.Contains(item))
            selectedItems.Remove(item);
        else
            selectedItems.Add(item);

        dismantleButton.interactable = selectedItems.Count > 0;
    }

    public void DismantleItems()
    {
        if (selectedItems.Count == 0) return;

        int totalSellPrice = 0;
        foreach (Item item in selectedItems)
        {
            totalSellPrice += item.sellPrice;
            PlayerInventoryManager.Instance.RemoveItem(item);
        }
        // 나중에 충돌위험 없을때 GameManager.cs와 UIManager.cs에 골드를 획득하고 그 골드수치를 업데이트해주도록 작업해야함

        selectedItems.Clear();
        dismantleButton.interactable = false;

        InventoryManager.Instance.RefreshInventory();
        Debug.Log($"아이템을 분해함. 획득 골드 {totalSellPrice}");
    }
}
