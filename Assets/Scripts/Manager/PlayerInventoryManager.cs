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

    public bool AddItem(Item item)
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