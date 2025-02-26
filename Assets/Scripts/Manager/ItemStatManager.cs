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
        TextAsset jsonFile = Resources.Load<TextAsset>("Item/StatData");
        if (jsonFile == null)
        {
            Debug.LogError("Resources ������ Item/StatData.json �������� ����");
            return;
        }

        string json = jsonFile.text;

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("StatData.json�� �������");
            return;
        }

        Debug.Log($"�ҷ��� JSON ������ : {json}");

        ItemStatList statList = new ItemStatList();
        JsonUtility.FromJsonOverwrite(json, statList);

        if (statList == null || statList.stats == null)
        {
            Debug.LogError("LoadStatData���� �����߻� - ������ ����");
            return;
        }

        statDictionary.Clear();

        foreach (var stat in statList.stats)
        {
            if (stat == null)
            {
                Debug.LogWarning("NULL ���� ������ �߰ߵ�");
                continue;
            }

            Debug.Log($"���� �ε�� : �ڵ� {stat.statCode}, �̸� {stat.statName}");
            statDictionary[stat.statCode] = stat.statName;
        }

        

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
[System.Serializable]
public class ItemStatList
{
    public List<ItemStatData> stats;
}