using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDismantle : MonoBehaviour  // ������ ����
{
    public Button dismantleButton;
    private List<Item> selectedItems = new List<Item>();

    public void ToggleItemSelection(Item item)
    {
        if (selectedItems.Contains(item))
            selectedItems.Remove(item);
        else
            selectedItems.Add(item);

        dismantleButton.interactable = selectedItems.Count > 0;
    }

    public void DismantleItems()
    {
        int totalSellPrice = 0;
        foreach (Item item in selectedItems)
        {
            totalSellPrice += item.sellPrice;
            PlayerInventoryManager.Instance.RemoveItem(item);
        }
        selectedItems.Clear();
        dismantleButton.interactable = false;
        Debug.Log($"�������� ������. ȹ�� ��� {totalSellPrice}");
    }
}
