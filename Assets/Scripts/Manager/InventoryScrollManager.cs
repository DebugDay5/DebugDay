using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScrollManager : MonoBehaviour
{
    public ScrollRect scrollRect;       // ScrollRect
    public RectTransform content;       // Content(인벤슬롯그리드)
    public float snapThreshold = 100f;  // 스냅 조정 기준

    private void Start()
    {
        if (scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();

        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void OnScroll(Vector2 scrollPosition)
    {
        if (scrollRect.velocity.magnitude < 100f)    // 스크롤 거의 멈추면
        {
            StopAllCoroutines();
            StartCoroutine(SnapToPosition());
        }
    }

    private IEnumerator SnapToPosition()
    {
        yield return new WaitForSeconds(0.1f);  // 관성 멈추는 시간

        float itemSlotHeight = 100f;                                //ItemSlot 높이 보면서 조절
        float viewportHeight = scrollRect.viewport.rect.height;     // Viewport 높이
        float contentHeight = content.rect.height;                  // 전체 아이템슬롯 높이

        int totalItems = content.childCount;
        int maxRows = Mathf.CeilToInt(totalItems / 5);  // 아이템 개수 구한 뒤 한줄에 5개씩 배치할거임

        if (maxRows <= 4)   // 화면상 네줄정도만 보여줄거임
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
