using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // 인벤토리 화면 아이템슬롯에 아이템 배치
{
    public Image itemIcon;
    public GameObject itemInfoPanel;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemStats;
    public Button equipButton;
    public Button enhanceButton;

    private Item itemData;

    public void Setup(Item item)
    {
        itemData = item;
        itemIcon.sprite = item.icon;
        itemIcon.enabled = true;
        itemInfoPanel.SetActive(false);
    }

    public void OnClick()
    {
        if (itemInfoPanel.activeSelf)
            itemInfoPanel.SetActive(false);
        else
        {
            itemInfoPanel.SetActive(true);
            itemName.text = itemData.name;

            string statInfo = "";
            foreach (var stat in itemData.stats)
            {
                string statName = ItemStatManager.Instance.GetStatName(stat.key);
                statInfo += $"{statName} : {stat.Value}\n";
            }

            itemStats.text = statInfo;
            equipButton.onClick.AddListener(() => EquipItem());
            enhanceButton.onClick.AddListener(() => EnhanceItem());
        }
    }

}
