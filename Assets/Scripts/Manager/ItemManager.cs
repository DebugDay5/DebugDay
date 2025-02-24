using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    private List<Item> itemDatabase = new List<Item>();

    private string savePath;

    void Awake()
    {
        if (Instance == null) Instance = this;
        savePath = Path.Combine(Application.dataPath, "Scripts/Data/ItemData.json");
        LoadItemsFromJson();
    }

    void LoadItemsFromJson()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("ItemData.json 파일이 존재하지 않습니다");
            return;
        }

        string json = File.ReadAllText(savePath);
        ItemList itemList = JsonUtility.FromJson<ItemList>(json);

        foreach (ItemData data in itemList.items)
            itemDatabase.Add(new Item(data));
        
        Debug.Log($"{itemDatabase.Count}개의 아이템 로드됨");
    }

    public Item GetItemById(int id)
    {
        return itemDatabase.Find(item => item.id == id);
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