using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // 인벤토리 화면 아이템슬롯에 아이템 배치
{
    public Image itemIcon;
    public GameObject itemInfoPanel;
    public GameObject closePanel;

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
        closePanel.SetActive(false);
    }

    public void OnClick()   // 인벤토리의 아이템 슬롯 클릭 시 아이템 정보창이 나오고 강화, 장착을 선택
    {
        if (itemInfoPanel.activeSelf)
            CloseItemInfoPanel();
        else
        {
            itemInfoPanel.SetActive(true);
            closePanel.SetActive(true);

            itemInfoPanel.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            itemName.text = itemData.name;
            itemStats.text = GetStatInfo();
            equipButton.onClick.AddListener(() => EquipItem());
            enhanceButton.onClick.AddListener(() => EnhanceItem());
        }
    }

    public void CloseItemInfoPanel()    // 아이템 정보창 바깥 누르면 정보창 닫히도록
    {
        itemInfoPanel.SetActive(false);
        closePanel.SetActive(false);
    }

    private string GetStatInfo()
    {
        string statInfo = "";
        foreach (var stat in itemData.stats)
        {
            string statName = ItemStatManager.Instance.GetStatName(stat.Key);
            statInfo += $"{statName}: {stat.Value}\n";
        }
        return statInfo;
    }

    private void EquipItem()    // 장착버튼
    {
        Debug.Log($"{itemData.name} 장착함");
        itemInfoPanel.SetActive(false);
    }

    private void EnhanceItem()  // 강화버튼
    {
        Debug.Log($"{itemData.name} 강화함");
        itemInfoPanel.SetActive(false);
    }

}
