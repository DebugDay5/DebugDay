using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id { get; private set; }
    public string name { get; private set; }
    public string rarity { get; private set; }
    public string type { get; private set; }
    public Dictionary<int, float> stats { get; private set; }
    public int sellPrice { get; private set; }

    public Item(ItemData data)
    {
        id = data.itemId;
        name = data.itemName;
        rarity = data.itemRarity;
        type = data.itemType;
        sellPrice = data.itemSellPrice;

        stats = new Dictionary<int, float>();

        if (data.itemStatCode.Length > 0)
            stats[data.itemStatCode[0]] = data.itemStat1;
        if (data.itemStatCode.Length > 1 && data.itemStat2.HasValue)
            stats[data.itemStatCode[1]] = data.itemStat2.Value;
    }
}
