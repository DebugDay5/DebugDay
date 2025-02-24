using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySort : MonoBehaviour      // 인벤토리 화면에서 장착 부위별 정렬, 레어도별 정렬
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
