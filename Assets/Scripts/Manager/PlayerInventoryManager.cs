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
            Debug.Log("인벤토리가 가득 차 더 이상 아이템을 획득할 수 없음");
            return false;
        }

        ownedItems.Add(item);
        SortInventory();
        SaveInventory();
        Debug.Log($"{item.name} 획득 완료. 저장됨");
        return true;
    }

    public void RemoveItem(Item item)
    {
        if (ownedItems.Contains(item))
        {
            ownedItems.Remove(item);
            SaveInventory();
            Debug.Log($"{item.name} 제거 완료. 저장됨");
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
        Debug.Log("빈 인벤토리 파일 생성됨");
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