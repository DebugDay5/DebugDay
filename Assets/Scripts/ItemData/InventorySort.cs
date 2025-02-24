using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySort : MonoBehaviour      // �κ��丮 ȭ�鿡�� ���� ������ ����, ����� ����
{
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
