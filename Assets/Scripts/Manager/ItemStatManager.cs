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
        TextAsset jsonFile = Resources.Load<TextAsset>("Item/StatData");
        if (jsonFile == null)
        {
            Debug.LogError("Resources 폴더에 Item/StatData.json 존재하지 않음");
            return;
        }

        string json = jsonFile.text;

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("StatData.json이 비어있음");
            return;
        }

        Debug.Log($"불러온 JSON 데이터 : {json}");

        ItemStatList statList = new ItemStatList();
        JsonUtility.FromJsonOverwrite(json, statList);

        if (statList == null || statList.stats == null)
        {
            Debug.LogError("LoadStatData에서 오류발생 - 데이터 오류");
            return;
        }

        statDictionary.Clear();

        foreach (var stat in statList.stats)
        {
            if (stat == null)
            {
                Debug.LogWarning("NULL 스탯 데이터 발견됨");
                continue;
            }

            Debug.Log($"스탯 로드됨 : 코드 {stat.statCode}, 이름 {stat.statName}");
            statDictionary[stat.statCode] = stat.statName;
        }

        

        Debug.Log($"{statDictionary.Count}개의 스탯 로드됨");
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
[System.Serializable]
public class ItemStatList
{
    public List<ItemStatData> stats;
}