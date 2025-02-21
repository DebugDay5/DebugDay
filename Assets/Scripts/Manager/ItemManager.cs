using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    private List<Item> itemDatabase = new List<Item>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        LoadItemsFromJson();
    }

    void LoadItemsFromJson()
    {
        
    }
}
