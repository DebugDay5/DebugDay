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
        string path = Path.Combine(Application.dataPath, "Scripts/Data/ItemData.json");
        if (!File.Exists(path))
        {
            Debug.LogError("ItemData.json ������ �������� �ʽ��ϴ�");
            return;
        }

        string json = File.ReadAllText(path);
        ItemList itemList = JsonUtility.FromJson<ItemList>(json);
        
        Debug.Log($"{itemDatabase.Count}���� ������ �ε��");
    }

    [System.Serializable]
    public class ItemList
    {
        public List<ItemData> items;
    }
}
