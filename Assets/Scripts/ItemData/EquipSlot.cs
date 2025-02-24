using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour      // 장비 슬롯 관리
{
    public Image icon;
    private Item equippedItem;

    public void EquipItem(Item newItem)
    {
        if (equippedItem != null)
            UnequipItem();

        equippedItem = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
    }

    public void UnequipItem()
    {
        equippedItem = null;
        icon.enabled = false;
    }

    public Item GetEquippedItem()
    {
        return equippedItem;
    }
}
