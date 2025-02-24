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
            Debug.LogError("ItemData.json 파일이 존재하지 않습니다");
            return;
        }

        ItemList itemList = JsonUtility.FromJson<ItemList>(jsonFile.text);
        foreach (ItemData data in itemList.items)
        {
            itemDatabase.Add(new Item(data));
        }

        Debug.Log($"{itemDatabase.Count}개의 아이템 로드됨");
    }

    [System.Serializable]
    public class ItemList
    {
        public List<ItemData> items;
    }
}
