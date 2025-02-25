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
        // ���߿� �浹���� ������ GameManager.cs�� UIManager.cs�� ��带 ȹ���ϰ� �� ����ġ�� ������Ʈ���ֵ��� �۾��ؾ���

        selectedItems.Clear();
        dismantleButton.interactable = false;

        InventoryManager.Instance.RefreshInventory();
        Debug.Log($"�������� ������. ȹ�� ��� {totalSellPrice}");
    }
}
