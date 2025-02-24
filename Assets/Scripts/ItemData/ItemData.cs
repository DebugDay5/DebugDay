using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour   // ItemData.json �����͸�
{
    public int itemId;
    public string itemName;
    public string itemRarity;
    public string itemType;
    public int[] itemStatCode;  // ���Ƽ�� ���� �������� �ΰ�����
    public float itemStat1;
    public float? itemStat2;    // ���Ƽ ���� �� null
    public int itemSellPrice;
    public string iconPath;
}
