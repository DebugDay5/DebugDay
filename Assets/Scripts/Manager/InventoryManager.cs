using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour   // �κ��丮 ȭ�� ����
{
    public static InventoryManager Instance;
    public List<Item> displayedItems = new List<Item>();
    public InventoryUI inventoryUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RefreshInventory()
    {
        displayedItems = PlayerInventoryManager.Instance.GetOwnedItems();   // �÷��̾� ������ ���� �����ͼ�
        SortInventoryByRarity();    // ��� ������ ����
    }

    public void SortInventoryByRarity()
    {
        displayedItems.Sort((a, b) => CompareRarity(b.rarity, a.rarity));
        inventoryUI.RefreshInventory(displayedItems);
    }

    private int CompareRarity(string rarityA, string rarityB)
    {
        Dictionary<string, int> rarityOrder = new Dictionary<string, int>
        {
            { "common", 1}, { "rare", 2}, { "unique", 3}, { "legendary", 4}
        };
        return rarityOrder[rarityA].CompareTo(rarityOrder[rarityB]);
    }
}
