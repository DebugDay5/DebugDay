using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    private List<Item> itemDatabase = new List<Item>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        LoadItemsFromJson();
    }

    void LoadItemsFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("ItemData");
        if (jsonFile == null)
        {
            Debug.LogError("ItemData.json ������ �������� �ʽ��ϴ�");
            return;
        }

        ItemList itemList = JsonUtility.FromJson<ItemList>(jsonFile.text);
        foreach (ItemData data in itemList.items)
        {
            itemDatabase.Add(new Item(data));
        }

        Debug.Log($"{itemDatabase.Count}���� ������ �ε��");
    }

    [System.Serializable]
    public class ItemList
    {
        public List<ItemData> items;
    }
}
