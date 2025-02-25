using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemManager : MonoBehaviour    // ItemData.json 참조, 아이템 데이터 로드, 관리
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
            Debug.LogError("ItemData.json 파일이 존재하지 않습니다");
            return;
        }

        string json = File.ReadAllText(savePath);
        ItemList itemList = JsonUtility.FromJson<ItemList>(json);

        if (itemList == null || itemList.items == null)
        {
            Debug.LogError("LoadItemsFronJson에서 오류 발생 - 데이터 오류");
            return;
        }

        foreach (ItemData data in itemList.items)
            itemDatabase.Add(new Item(data));
        
        Debug.Log($"{itemDatabase.Count}개의 아이템 로드됨");
    }

    public Item GetItemById(int id)
    {
        Item item = itemDatabase.Find(i => i.id == id);
        if (item == null)
            Debug.Log($"ID : {id} 에 해당하는 아이템을 찾을 수 없음");
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