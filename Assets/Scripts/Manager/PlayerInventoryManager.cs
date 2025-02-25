using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour // �÷��̾ �������� ��������
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
        Debug.Log("PlayerInventoryManager Start() �����");
        if (ownedItems.Count == 0) AddTestItems();
    }

    public void AddTestItems()      // �κ��ý��� �׽�Ʈ�� ������
    {
        Debug.Log("�׽�Ʈ�� ������ �߰�");

        List<Item> testItems = new List<Item>
        {
            new Item(new ItemData { itemId = 10001, itemName = "�Ϲ� Ȱ", itemRarity = "common", itemType = "weapon", itemStatCode = new int[]{1}, itemStat1 = 10, itemSellPrice = 500, iconPath = "weapon_1" }),
            new Item(new ItemData { itemId = 10002, itemName = "�Ϲ� ����", itemRarity = "common", itemType = "armor", itemStatCode = new int[]{3}, itemStat1 = 10, itemSellPrice = 500, iconPath = "armor_1" }),
            new Item(new ItemData { itemId = 10003, itemName = "������ �����", itemRarity = "common", itemType = "neckless", itemStatCode = new int[]{2}, itemStat1 = 100, itemSellPrice = 500, iconPath = "neckless_1" }),
            new Item(new ItemData { itemId = 10004, itemName = "������ ����", itemRarity = "common", itemType = "ring", itemStatCode = new int[]{4}, itemStat1 = 5, itemSellPrice = 500, iconPath = "ring_1" }),
            new Item(new ItemData { itemId = 30001, itemName = "������ Ȱ", itemRarity = "unique", itemType = "weapon", itemStatCode = new int[]{1, 5}, itemStat1 = 40, itemStat2 = 30, itemSellPrice = 5000, iconPath = "weapon_3" })
        };

        foreach (var item in testItems)
            AddItem(item);

        Debug.Log("�׽�Ʈ�� ������ �߰���");
    }

    public bool AddItem(Item item)  // ������ ȹ��
    {
        if (ownedItems.Count >= MAX_INVENTORY_SIZE)
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
        InventorySort.Instance.SortByRarity(ownedItems);
    }

    public List<Item> GetOwnedItems() => new List<Item>(ownedItems);
}

[System.Serializable]
public class PlayerInventoryData
{
    public List<int> ownedItemIds;
}