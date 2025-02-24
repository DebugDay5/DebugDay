using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemStatManager : MonoBehaviour    // StatData.json, 아이템 스탯 관리
{
    public static ItemStatManager Instance;
    private Dictionary<int, string> statDictionary = new Dictionary<int, string>();

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        savePath = Path.Combine(Application.dataPath, "Scripts/Data/StatData.json");
        LoadStatData();
    }

    private void LoadStatData()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("StatData.json 파일이 존재하지 않음");
            return;
        }

        string json = File.ReadAllText(savePath);
        ItemStatList statList = JsonUtility.FromJson<ItemStatList>(json);

        foreach (var stat in statList.stats)
            statDictionary[stat.statCode] = stat.statName;

        Debug.Log($"{statDictionary.Count}개의 스탯 로드됨");
    }

    public string GetStatName(int statCode)
    {
        return statDictionary.ContainsKey(statCode) ? statDictionary[statCode] : "????";
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