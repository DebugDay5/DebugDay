using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager Instance;
    private List<Item> ownedItems = new List<Item>();
    private string savePath;
    private const int MAX_INVERTORY_SIZE = 100;

    void Awake()
    {
        if (Instance == null) Instance = this;
        savePath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");
        LoadInventory();
    }

    public bool AddItem(Item item)
    {
        if (ownedItems.Count >= MAX_INVERTORY_SIZE)
        {
            Debug.Log("�κ��丮�� ���� �� �� �̻� �������� ȹ���� �� ����");
            return false;
        }

        ownedItems.Add(item);
        SortInventory();
        SaveInventory();
        Debug.Log($"{item.name} ȹ�� �Ϸ�. �����");
        return true;
    }

    public void RemoveItem(Item item)
    {
        if (ownedItems.Contains(item))
        {
            ownedItems.Remove(item);
            SaveInventory();
            Debug.Log($"{item.name} ���� �Ϸ�. �����");
        }
    }

    private void LoadInventory()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerInventoryData data = JsonUtility.FromJson<PlayerInventoryData>(json);
            ownedItems.Clear();
            foreach (int itemId in data.ownedItemIds)
            {
                Item item = ItemManager.Instance.GetItemById(itemId);
                if (item != null)
                    ownedItems.Add(item);
            }
        }
    }

    private void CreateDefaultInventoryFile()
    {
        PlayerInventoryData defaultData = new PlayerInventoryData { ownedItemIds = new List<int>() };
        string json = JsonUtility.ToJson(defaultData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("�� �κ��丮 ���� ������");
    }

    private void SaveInventory()
    {
        PlayerInventoryData data = new PlayerInventoryData { ownedItemIds = new List<int>() };
        foreach (Item item in ownedItems)
            data.ownedItemIds.Add(item.id);
        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
    }

    private void SortInventory()
    {
        ownedItems.Sort((a, b) => CompareRarity(b.rarity, a.rarity));
    }

    private int CompareRarity(string rarityA, string rarityB)
    {
        Dictionary<string, int> rarityOrder = new Dictionary<string, int>
        {
            { "common", 1}, { "rare", 2}, { "unique", 3}, { "legendary", 4}
        };
        return rarityOrder[rarityA].CompareTo(rarityOrder[rarityB]);
    }

    public List<Item> GetOwnedItems() => ownedItems;
}

[System.Serializable]
public class PlayerInventoryData
{
    public List<int> ownedItemIds;
}