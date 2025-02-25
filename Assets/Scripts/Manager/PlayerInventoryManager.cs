using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour // 플레이어가 보유중인 장비아이템
{
    public static PlayerInventoryManager Instance;
    private List<Item> ownedItems = new List<Item>();
    private string savePath;
    private const int MAX_INVENTORY_SIZE = 100;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        savePath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");
        LoadInventory();
    }
    void Start()
    {
        Debug.Log("PlayerInventoryManager Start() 실행됨");
        if (ownedItems.Count == 0) AddTestItems();
    }

    public void AddTestItems()      // 인벤시스템 테스트용 아이템
    {
        Debug.Log("테스트용 아이템 추가");

        List<Item> testItems = new List<Item>
        {
            new Item(new ItemData { itemId = 10001, itemName = "일반 활", itemRarity = "common", itemType = "weapon", itemStatCode = new int[]{1}, itemStat1 = 10, itemSellPrice = 500, iconPath = "weapon_1" }),
            new Item(new ItemData { itemId = 10002, itemName = "일반 갑옷", itemRarity = "common", itemType = "armor", itemStatCode = new int[]{3}, itemStat1 = 10, itemSellPrice = 500, iconPath = "armor_1" }),
            new Item(new ItemData { itemId = 10003, itemName = "수수한 목걸이", itemRarity = "common", itemType = "neckless", itemStatCode = new int[]{2}, itemStat1 = 100, itemSellPrice = 500, iconPath = "neckless_1" }),
            new Item(new ItemData { itemId = 10004, itemName = "수수한 반지", itemRarity = "common", itemType = "ring", itemStatCode = new int[]{4}, itemStat1 = 5, itemSellPrice = 500, iconPath = "ring_1" }),
            new Item(new ItemData { itemId = 30001, itemName = "정교한 활", itemRarity = "unique", itemType = "weapon", itemStatCode = new int[]{1, 5}, itemStat1 = 40, itemStat2 = 30, itemSellPrice = 5000, iconPath = "weapon_3" })
        };

        foreach (var item in testItems)
            AddItem(item);

        Debug.Log("테스트용 아이템 추가됨");
    }

    public bool AddItem(Item item)  // 아이템 획득
    {
        if (ownedItems.Count >= MAX_INVENTORY_SIZE)
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
        if (!File.Exists(savePath))
        {
            CreateDefaultInventoryFile();
            return;
        }
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
        InventorySort.Instance.SortByRarity(ownedItems);
    }

    public List<Item> GetOwnedItems() => new List<Item>(ownedItems);
}

[System.Serializable]
public class PlayerInventoryData
{
    public List<int> ownedItemIds;
}