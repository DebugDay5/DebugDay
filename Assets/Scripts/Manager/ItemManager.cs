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

        savePath = Path.Combine(Application.persistentDataPath, "ItemData.json");
        LoadItemsFromJson();
    }

    void LoadItemsFromJson()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("ItemData.json ������ �������� �ʽ��ϴ�");
            return;
        }

        string json = File.ReadAllText(savePath);
        ItemList itemList = JsonUtility.FromJson<ItemList>(json);

        if (itemList == null || itemList.items == null)
        {
            Debug.LogError("LoadItemsFronJson���� ���� �߻� - ������ ����");
            return;
        }

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
    public List<ItemData> items;
}