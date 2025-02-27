using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDismantle : MonoBehaviour  // 아이템 분해
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
            slot.Deselect();  // UI 효과 제거
        }
        else
        {
            selectedItems.Add(item);
            slot.Select();  // UI 효과 추가
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

        Debug.Log($"아이템을 분해함. 획득 골드: {totalSellPrice}, 강화 재료: {reinforcementMaterials}");

        // 분해 모드 해제
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
