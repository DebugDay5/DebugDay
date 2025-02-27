using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDismantle : MonoBehaviour  // ������ ����
{
    public Button dismantleButton;
    private List<Item> selectedItems = new List<Item>();
    private bool isDismantling = false;
    private Dictionary<string, int> reinforcementMaterials = new Dictionary<string, int>();

    public void Start()
    {
        dismantleButton.onClick.AddListener(ToggleDismantleMode);
    }

    private void ToggleDismantleMode()
    {
        isDismantling = !isDismantling;
        dismantleButton.GetComponentInChildren<Text>().text = isDismantling ? "Confirm" : "Dismantle";

        if (!isDismantling)
        {
            ClearSelection();
        }
    }

    public void ToggleItemSelection(Item item, ItemSlot slot)
    {
        if (!isDismantling || item == null) return;

        if (selectedItems.Contains(item))
        {
            selectedItems.Remove(item);
            slot.Deselect();  // UI ȿ�� ����
        }
        else
        {
            selectedItems.Add(item);
            slot.Select();  // UI ȿ�� �߰�
        }

        dismantleButton.interactable = selectedItems.Count > 0;
    }

    public void DismantleItems()
    {
        if (!isDismantling || selectedItems.Count == 0) return;

        int totalSellPrice = 0;
        foreach (Item item in selectedItems)
        {
            totalSellPrice += item.sellPrice;
            PlayerInventoryManager.Instance.RemoveItem(item);

            string itemType = item.type;
            int reinforcementAmount = Mathf.Max(1, item.sellPrice / 100);

            if (reinforcementMaterials.ContainsKey(itemType))
                reinforcementMaterials[itemType] += reinforcementAmount;
            else
                reinforcementMaterials[itemType] = reinforcementAmount;
        }
        GameManager.Instance.UpdateGold(totalSellPrice);

        selectedItems.Clear();
        dismantleButton.interactable = false;
        InventoryManager.Instance.RefreshInventory();

        Debug.Log($"�������� ������. ȹ�� ���: {totalSellPrice}, ��ȭ ���: {reinforcementMaterials}");

        // ���� ��� ����
        ToggleDismantleMode();
    }

    private void ClearSelection()
    {
        foreach (var slot in FindObjectsOfType<ItemSlot>())
        {
            slot.Deselect();
        }
        selectedItems.Clear();
        dismantleButton.interactable = false;
    }
}
