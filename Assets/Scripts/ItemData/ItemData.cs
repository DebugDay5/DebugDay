using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour   // ItemData.json 데이터모델
{
    public int itemId;
    public string itemName;
    public string itemRarity;
    public string itemType;
    public int[] itemStatCode;  // 레어리티가 높은 아이템은 두개까지
    public float itemStat1;
    public float? itemStat2;    // 레어리티 낮은 템 null
    public int itemSellPrice;
    public string iconPath;
}
