using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData   // ItemData.json �����͸�
{
    public int itemId;
    public string itemName;
    public string itemRarity;
    public string itemType;
    public int[] itemStatCode;  // ���Ƽ�� ���� �������� ���� �ΰ�����
    public float itemStat1;
    public float itemStat2 = 0f;    // ���Ƽ ���� �� 0f
    public int itemSellPrice;
    public string iconPath;
}