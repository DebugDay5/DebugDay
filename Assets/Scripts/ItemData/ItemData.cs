using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData   // ItemData.json 데이터모델
{
    public int itemId;
    public string itemName;
    public string itemRarity;
    public string itemType;
    public int[] itemStatCode;  // 레어리티가 높은 아이템은 스탯 두개까지
    public float itemStat1;
    public float itemStat2 = 0f;    // 레어리티 낮은 템 0f
    public int itemSellPrice;
    public string iconPath;
}