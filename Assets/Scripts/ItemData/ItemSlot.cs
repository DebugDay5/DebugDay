using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // �κ��丮 ȭ�� �����۽��Կ� ������ ��ġ
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
