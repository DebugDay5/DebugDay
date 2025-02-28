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
        canvas.sortingOrder = 99; // ItemInfoPanel���� ���� ��ġ

        Image closePanelImage = GetComponent<Image>();
        if (closePanelImage == null)
        {
            closePanelImage = gameObject.AddComponent<Image>(); // Image�� ������ �߰�
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
        Debug.Log("ClosePanel Ŭ����");
        if (itemInfoPanel == null)
        {
            Debug.LogError("itemInfoPanel�� null�Դϴ�! ClosePanelHandler���� Ȯ�����ּ���.");
            return;
        }

        RectTransform itemInfoRect = itemInfoPanel.GetComponent<RectTransform>();
        if (itemInfoRect == null)
        {
            Debug.LogError("itemInfoPanel�� RectTransform�� �����ϴ�!");
            return;
        }

        Camera uiCamera = Camera.main;  // �⺻ ���� ī�޶� ���
        if (uiCamera == null)
        {
            Debug.LogError("Camera.main�� NULL�Դϴ�. UICamera�� ���� �������ּ���.");
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(itemInfoRect, eventData.position, uiCamera))
        {
            Debug.Log("ItemInfoPanel ���� Ŭ�� - ���� ����");
            return;
        }

        Debug.Log("ClosePanel Ŭ�� - ItemInfoPanel �ݱ�");
        itemInfoPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}
