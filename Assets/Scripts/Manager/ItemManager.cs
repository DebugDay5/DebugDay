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
}

[System.Serializable]
public class ItemList
{
    public List<ItemData> items;
}