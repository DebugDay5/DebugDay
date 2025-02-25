using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemStatManager : MonoBehaviour    // StatData.json, ������ ���� ����
{
    public static ItemStatManager Instance;
    private Dictionary<int, string> statDictionary = new Dictionary<int, string>();

    private string savePath;

    private void Awake()
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

        savePath = Path.Combine(Application.persistentDataPath, "StatData.json");
        LoadStatData();
    }

    private void LoadStatData()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("StatData.json ������ �������� ����");
            return;
        }

        string json = File.ReadAllText(savePath);
        ItemStatList statList = JsonUtility.FromJson<ItemStatList>(json);

        if (statList == null || statList.stats == null)
        {
            Debug.LogError("LoadStatData���� �����߻� - ������ ����");
            return;
        }

        foreach (var stat in statList.stats)
            statDictionary[stat.statCode] = stat.statName;

        Debug.Log($"{statDictionary.Count}���� ���� �ε��");
    }

    public string GetStatName(int statCode)
    {
        return statDictionary.TryGetValue(statCode, out string statName) ? statName : "????";
    }
}

[System.Serializable]
public class ItemStatData
{
    public int statCode;
    public string statName;
}

public class ItemStatList
{
    public List<ItemStatData> stats;
}