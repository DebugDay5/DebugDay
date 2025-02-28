using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScrollManager : MonoBehaviour
{
    public ScrollRect scrollRect;       // ScrollRect
    public RectTransform content;       // Content(�κ����Ա׸���)
    public float snapThreshold = 100f;  // ���� ���� ����

    private void Start()
    {
        if (scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();

        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void OnScroll(Vector2 scrollPosition)
    {
        if (scrollRect.velocity.magnitude < 100f)    // ��ũ�� ���� ���߸�
        {
            StopAllCoroutines();
            StartCoroutine(SnapToPosition());
        }
    }

    private IEnumerator SnapToPosition()
    {
        yield return new WaitForSeconds(0.1f);  // ���� ���ߴ� �ð�

        float itemSlotHeight = 100f;                                //ItemSlot ���� ���鼭 ����
        float viewportHeight = scrollRect.viewport.rect.height;     // Viewport ����
        float contentHeight = content.rect.height;                  // ��ü �����۽��� ����

        int totalItems = content.childCount;
        int maxRows = Mathf.CeilToInt(totalItems / 5);  // ������ ���� ���� �� ���ٿ� 5���� ��ġ�Ұ���

        if (maxRows <= 4)   // ȭ��� ���������� �����ٰ���
            scrollRect.normalizedPosition = new Vector2(0, 1);
        else
        {
            float currentY = content.anchoredPosition.y;
            float targetY = Mathf.Round(currentY / itemSlotHeight) * itemSlotHeight;

            if (Mathf.Abs(currentY - targetY) < snapThreshold)
            {
                float duration = 0.2f;
                float elapsedTime = 0f;
                Vector2 startPos = content.anchoredPosition;
                Vector2 endPos = new Vector2(startPos.x, targetY);

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    content.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedTime / duration);
                    yield return null;
                }

                content.anchoredPosition = endPos;
            }
        }
    }
}
