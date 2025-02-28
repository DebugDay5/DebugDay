using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour // �÷��̾ �������� ��������
{
    public static PlayerInventoryManager Instance;
    private List<Item> ownedItems = new List<Item>();
    private Dictionary<string, Item> equippedItems = new Dictionary<string, Item>();
    private string savePath;
    private string equippedItemsPath;
    private const int MAX_INVENTORY_SIZE = 100;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        savePath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");
        equippedItemsPath = Path.Combine(Application.persistentDataPath, "EquippedItems.json");
        LoadInventory();
        LoadEquippedItems();
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
            new Item(new ItemData { itemId = 10001, itemName = "�Ϲ� Ȱ", itemRarity = "common", itemType = "weapon", itemStatCode = new int[]{1}, itemStat1 = 10, itemSellPrice = 500, iconPath = "Item/weapon_1" }),
            new Item(new ItemData { itemId = 10002, itemName = "�Ϲ� ����", itemRarity = "common", itemType = "armor", itemStatCode = new int[]{3}, itemStat1 = 10, itemSellPrice = 500, iconPath = "Item/armor_1" }),
            new Item(new ItemData { itemId = 10003, itemName = "������ �����", itemRarity = "common", itemType = "neckless", itemStatCode = new int[]{2}, itemStat1 = 100, itemSellPrice = 500, iconPath = "Item/neckless_1" }),
            new Item(new ItemData { itemId = 10004, itemName = "������ ����", itemRarity = "common", itemType = "ring", itemStatCode = new int[]{4}, itemStat1 = 5, itemSellPrice = 500, iconPath = "Item/ring_1" }),
            new Item(new ItemData { itemId = 30001, itemName = "������ Ȱ", itemRarity = "unique", itemType = "weapon", itemStatCode = new int[]{1, 5}, itemStat1 = 40, itemStat2 = 30, itemSellPrice = 5000, iconPath = "Item/weapon_3" })
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
        else
            Debug.LogError($"{item.name} ���� ���� - �κ��丮�� �������� ����.");
    }

    public void EquipItem(string itemType, Item item)
    {
        if (equippedItems.ContainsKey(itemType))
            equippedItems[itemType] = item;
        else
            equippedItems.Add(itemType, item);

        SaveEquippedItems();
    }

    public void UnequipItem(string itemType)
    {
        if (equippedItems.ContainsKey(itemType))
            equippedItems.Remove(itemType);

        SaveEquippedItems();
    }

    public bool IsEquipped(string itemType) => equippedItems.ContainsKey(itemType);

    public Item GetEquippedItem(string itemType) => equippedItems.TryGetValue(itemType, out var item) ? item : null;

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

    private void LoadEquippedItems()
    {
        if (!File.Exists(equippedItemsPath))
        {
            Debug.Log("������ ������ �����Ͱ� �����ϴ�.");
            CreateDefaultEquippedFile();
            return;
        }

        string json = File.ReadAllText(equippedItemsPath);
        PlayerEquippedData data = JsonUtility.FromJson<PlayerEquippedData>(json);

        equippedItems.Clear();
        foreach (var kvp in data.equippedItemIds)
        {
            Item item = ItemManager.Instance.GetItemById(kvp.Value);
            if (item != null)
            {
                equippedItems[kvp.Key] = item;
                Debug.Log($"�ҷ��� ���� ������: {item.name} (����: {kvp.Key})");

                EquipSlot equipSlot = FindEquipSlot(kvp.Key);
                if (equipSlot != null)
                {
                    equipSlot.UpdateSlot(item);
                    Debug.Log($"EquipSlot�� ������ ���� : {item.name}");
                }
                else
                    Debug.LogError($"EquipSlot�� ã�� �� ���� : {kvp.Key}");
            }
            else
            {
                Debug.LogError($"������ �������� ã�� �� ����! itemId: {kvp.Value}");
            }
        }
    }
    private EquipSlot FindEquipSlot(string itemType)
    {
        EquipSlot[] equipSlots = FindObjectsOfType<EquipSlot>();
        foreach (var slot in equipSlots)
        {
            if (slot.itemType.Equals(itemType, System.StringComparison.OrdinalIgnoreCase))
            {
                return slot;
            }
        }
        return null;
    }

    private void CreateDefaultInventoryFile()
    {
        PlayerInventoryData defaultData = new PlayerInventoryData { ownedItemIds = new List<int>() };
        string json = JsonUtility.ToJson(defaultData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("�� �κ��丮 ���� ������");
    }

    private void CreateDefaultEquippedFile()
    {
        PlayerEquippedData defaultData = new PlayerEquippedData();
        string json = JsonUtility.ToJson(defaultData, true);
        File.WriteAllText(equippedItemsPath, json);
        Debug.Log("�� ���� ������ ���� ������: " + equippedItemsPath);
    }

    private void SaveInventory()
    {
        PlayerInventoryData data = new PlayerInventoryData { ownedItemIds = new List<int>() };
        foreach (Item item in ownedItems)
            data.ownedItemIds.Add(item.id);
        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
    }

    private void SaveEquippedItems()
    {
        PlayerEquippedData data = new PlayerEquippedData();

        foreach (var kvp in equippedItems)
        {
            data.equippedItemIds[kvp.Key] = kvp.Value.id;
        }
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(equippedItemsPath, json);
        Debug.Log($"������ ������ ���� �Ϸ�: {json}");
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
[System.Serializable]
public class PlayerEquippedData
{
    public Dictionary<string, int> equippedItemIds = new Dictionary<string, int>(); // ������ ������ ID ����
}