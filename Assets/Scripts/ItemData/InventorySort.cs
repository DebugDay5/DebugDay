using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySort : MonoBehaviour      // 인벤토리 화면에서 장착 부위별 정렬, 레어도별 정렬
{
    public static InventorySort Instance;

    public Button sortButton;
    public TextMeshProUGUI sortButtonText;

    private enum SortMode { Type, Rarity}
    private SortMode currentSortMode = SortMode.Rarity;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (sortButton != null)
        {
            sortButton.onClick.AddListener(ToggleSortMode);
            UpdateSortButtonText();
        }
    }

    public void ToggleSortMode()
    {
        if (currentSortMode == SortMode.Type)
            currentSortMode = SortMode.Rarity;
        else
            currentSortMode = SortMode.Type;

        ApplySort();
        UpdateSortButtonText();
    }

    private void ApplySort()
    {
        List<Item> items = PlayerInventoryManager.Instance?.GetOwnedItems();

        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("정렬할 아이템이 없습니다");
            return;
        }

        if (currentSortMode == SortMode.Type)
            SortByType(items);
        else
            SortByRarity(items);

        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
            inventoryUI.RefreshInventory(items);
    }

    private void UpdateSortButtonText()
    {
        if (sortButtonText != null)
            sortButtonText.text = (currentSortMode == SortMode.Type) ? "Sorted By Type" : "Sorted By Rarity";
    }

    public void SortByType(List<Item> items)
    {
        items.Sort((a, b) => a.type.CompareTo(b.type));
    }

    public void SortByRarity(List<Item> items)
    {
        Dictionary<string, int> rarityOrder = new Dictionary<string, int>
        {
            { "common", 1}, { "rare", 2}, { "unique", 3}, { "legendary", 4}
        };
        items.Sort((a, b) => rarityOrder[b.rarity].CompareTo(rarityOrder[a.rarity]));
    }
}
