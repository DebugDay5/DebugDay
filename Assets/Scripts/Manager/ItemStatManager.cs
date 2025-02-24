using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemStatManager : MonoBehaviour
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
        
    }
}
