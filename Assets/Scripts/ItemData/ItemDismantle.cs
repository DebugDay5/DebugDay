using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDismantle : MonoBehaviour  // 아이템 분해
{
    public static ItemDismantle Instance;

    public Button dismantleButton;
    private List<Item> selectedItems = new List<Item>();
    private bool isDismantling = false;
    private Dictionary<string, int> reinforcementMaterials = new Dictionary<string, int>();

    public bool IsDismantling => isDismantling;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        dismantleButton.onClick.AddListener(ToggleDismantleMode);
    }

    private void ToggleDismantleMode()
    {
        isDismantling = !isDismantling;

        if(isDismantling)
        {
            dismantleButton.GetComponentInChildren<TextMeshProUGUI>().text = "Confirm";
            dismantleButton.onClick.RemoveAllListeners();
            dismantleButton.onClick.AddListener(DismantleItems);
        }
        else
        {
            dismantleButton.GetComponentInChildren<TextMeshProUGUI>().text = "Dismantle";
            dismantleButton.onClick.RemoveAllListeners();
            dismantleButton.onClick.AddListener(ToggleDismantleMode);
            ClearSelection();
        }
    }

    public void ToggleItemSelection(Item item, ItemSlot slot)
    {
        if (!isDismantling || item == null) return;

        if (selectedItems.Contains(item))
        {
            selectedItems.Remove(item);
            slot.Deselect();  // UI 효과 제거
        }
        else
        {
            selectedItems.Add(item);
            slot.Select();  // UI 효과 추가
        }

        dismantleButton.interactable = selectedItems.Count > 0;
    }

    public void DismantleItems()
    {
        if (!isDismantling || selectedItems.Count == 0)
        {
            Debug.LogError("분해할 아이템이 없습니다!");
            return;
        }

        Debug.Log($"분해할 아이템 개수: {selectedItems.Count}");

        int totalSellPrice = 0;
        Dictionary<string, int> obtainedMaterials = new Dictionary<string, int>();

        foreach (Item item in selectedItems)
        {
            totalSellPrice += item.sellPrice;
            PlayerInventoryManager.Instance.RemoveItem(item);

            string itemType = item.type;
            int reinforcementAmount = Mathf.Max(1, item.sellPrice / 100);

            if (reinforcementMaterials.ContainsKey(itemType))
                reinforcementMaterials[itemType] += reinforcementAmount;
            else
                reinforcementMaterials[itemType] = reinforcementAmount;

            if (obtainedMaterials.ContainsKey(itemType))
                obtainedMaterials[itemType] += reinforcementAmount;
            else
                obtainedMaterials[itemType] = reinforcementAmount;
        }
        GameManager.Instance.UpdateGold(totalSellPrice);

        selectedItems.Clear();
        dismantleButton.interactable = false;

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RefreshInventory();   // 디버그 끝나면 if문 이줄만 남기기
            Debug.Log("인벤토리 UI 갱신됨.");
        }
        else
        {
            Debug.LogError("InventoryManager 인스턴스를 찾을 수 없습니다.");
        }

        string materialsLog = "";
        foreach (var mat in obtainedMaterials)
        {
            materialsLog += $"{mat.Key}: {mat.Value}, ";
        }
        materialsLog = materialsLog.TrimEnd(',', ' ');

        Debug.Log($"아이템을 분해함. 획득 골드: {totalSellPrice}, 강화 재료: {reinforcementMaterials}");

        // 분해 모드 해제
        ToggleDismantleMode();
    }

    private void ClearSelection()
    {
        foreach (var slot in FindObjectsOfType<ItemSlot>())
        {
            slot.Deselect();
        }
        selectedItems.Clear();
        dismantleButton.interactable = false;
    }
}
