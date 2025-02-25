using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item  // ���� �� ������ �ν��Ͻ� 02.25 - ������ Ŭ������ Monobehavior �������ϴ�. ������Ʈ�� ��ߵ� ���� �ٽ� �߰��� ����
{
    public int id { get; private set; }
    public string name { get; private set; }
    public string rarity { get; private set; }
    public string type { get; private set; }
    public Dictionary<int, float> stats { get; private set; }
    public int sellPrice { get; private set; }
    public Sprite icon { get; private set; }

    public Item(ItemData data)
    {
        id = data.itemId;
        name = data.itemName;
        rarity = data.itemRarity;
        type = data.itemType;
        sellPrice = data.itemSellPrice;

        stats = new Dictionary<int, float>();

        if (data.itemStatCode.Length > 0)
            if (!stats.ContainsKey(data.itemStatCode[0]))
                stats[data.itemStatCode[0]] = data.itemStat1;
        if (data.itemStatCode.Length > 1 && data.itemStat2 != 0f)
            if(!stats.ContainsKey(data.itemStatCode[1]))
                stats[data.itemStatCode[1]] = data.itemStat2;

        icon = Resources.Load<Sprite>(data.iconPath);
        if (icon == null && !string.IsNullOrEmpty(data.iconPath))
            Debug.LogError($"������ �������� ã�� �� �����ϴ� : {data.iconPath}");
    }
}
