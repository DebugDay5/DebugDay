using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClosePanelHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject itemInfoPanel;

    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }
        canvas.overrideSorting = true;
        canvas.sortingOrder = 99; // ItemInfoPanel보다 위로 배치

        Image closePanelImage = GetComponent<Image>();
        if (closePanelImage == null)
        {
            closePanelImage = gameObject.AddComponent<Image>(); // Image가 없으면 추가
        }
        closePanelImage.color = new Color(0, 0, 0, 0);
        closePanelImage.raycastTarget = true;

        if (GetComponent<GraphicRaycaster>() == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ClosePanel 클릭됨");
        if (itemInfoPanel == null)
        {
            Debug.LogError("itemInfoPanel이 null입니다! ClosePanelHandler에서 확인해주세요.");
            return;
        }

        RectTransform itemInfoRect = itemInfoPanel.GetComponent<RectTransform>();
        if (itemInfoRect == null)
        {
            Debug.LogError("itemInfoPanel에 RectTransform이 없습니다!");
            return;
        }

        Camera uiCamera = Camera.main;  // 기본 메인 카메라 사용
        if (uiCamera == null)
        {
            Debug.LogError("Camera.main이 NULL입니다. UICamera를 직접 지정해주세요.");
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(itemInfoRect, eventData.position, uiCamera))
        {
            Debug.Log("ItemInfoPanel 내부 클릭 - 닫지 않음");
            return;
        }

        Debug.Log("ClosePanel 클릭 - ItemInfoPanel 닫기");
        itemInfoPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}
