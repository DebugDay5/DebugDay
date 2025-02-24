using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // 인벤토리 화면 아이템슬롯에 아이템 배치
{
    public Image itemIcon;
    private Item itemData;

    public void Setup(Item item)
    {
        itemData = item;
        itemIcon.sprite = item.icon;
        itemIcon.enabled = true;
    }
}
