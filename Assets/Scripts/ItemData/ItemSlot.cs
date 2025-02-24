using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour   // �κ��丮 ȭ�� �����۽��Կ� ������ ��ġ
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

    public void OnClick()   // �κ��丮�� ������ ���� Ŭ�� �� ������ ����â�� ������ ��ȭ, ������ ����
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

    public void CloseItemInfoPanel()    // ������ ����â �ٱ� ������ ����â ��������
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

    private void EquipItem()    // ������ư
    {
        Debug.Log($"{itemData.name} ������");
        itemInfoPanel.SetActive(false);
    }

    private void EnhanceItem()  // ��ȭ��ư
    {
        Debug.Log($"{itemData.name} ��ȭ��");
        itemInfoPanel.SetActive(false);
    }

}
