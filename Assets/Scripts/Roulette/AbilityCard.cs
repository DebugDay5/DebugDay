using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityCard : MonoBehaviour , IPointerClickHandler
{
    [SerializeField]
    private int index;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Ŭ�� �Ǿ����ϴ�");
        RouletteManager.Instance.SelectCard(index);
    }

}
