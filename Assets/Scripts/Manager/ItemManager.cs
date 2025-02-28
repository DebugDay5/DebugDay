using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemManager : MonoBehaviour    // ItemData.json ����, ������ ������ �ε�, ����
{
    public static ItemManager Instance;
    private List<Item> itemDatabase = new List<Item>();

    private string savePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadItemsFromJson();
    }

    void LoadItemsFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Item/ItemData");
        if (jsonFile == null)
        {
            Debug.LogError("Resources ������ Item/ItemData.json �������� ����");
            return;
        }

        string json = jsonFile.text;

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("ItemData.json�� �������");
            return;
        }

        Debug.Log($"�ҷ��� JSON ������: {json}");

        ItemList itemList = new ItemList();
        JsonUtility.FromJsonOverwrite(json, itemList);

        if (itemList == null)
        {
            Debug.LogError("JsonUtility.FromJson<ItemList>(json)���� NULL ��ȯ");
            return;
        }

        // itemList.items�� NULL���� Ȯ��
        if (itemList.items == null)
        {
            Debug.LogError("itemList.items�� NULL");
            return;
        }

        itemDatabase.Clear();
        foreach (ItemData data in itemList.items)
            itemDatabase.Add(new Item(data));
        
        Debug.Log($"{itemDatabase.Count}���� ������ �ε��");
    }

    public Item GetItemById(int id)
    {
        Item item = itemDatabase.Find(i => i.id == id);
        if (item == null)
            Debug.Log($"ID : {id} �� �ش��ϴ� �������� ã�� �� ����");
        return item;
    }

    public List<Item> GetItemsByRarity(string rarity)
    {
        return itemDatabase.FindAll(item => item.rarity == rarity);
    }

    public List<Item> GetItemsByType(string type)
    {
        return itemDatabase.FindAll(item => item.type == type);
    }

    public Item GetRandomItemByRarity(string rarity)
    {
        int minId = 0, maxId = 0;

        switch (rarity)
        {
            case "common":
                {
                    minId = 10000;
                    maxId = 19999;
                    break;
                }
            case "rare":
                {
                    minId = 20000;
                    maxId = 29999;
                    break;
                }
            case "unique":
                {
                    minId = 30000;
                    maxId = 39999;
                    break;
                }
            case "legendary":
                {
                    minId = 40000;
                    maxId = 49999;
                    break;
                }
            default:
                Debug.LogWarning($"�߸��� ��͵�: {rarity}");
                return null;
        }

        List<Item> items = itemDatabase.FindAll(item => item.id >= minId && item.id <= maxId);

        if (items.Count == 0)
        {
            Debug.LogWarning($"'{rarity}' ����� �������� �������� �ʽ��ϴ�.");
            return null;
        }

        return items[Random.Range(0, items.Count)];
    }
}

[System.Serializable]
public class ItemList
{
    public List<ItemData> items = new List<ItemData>();
}