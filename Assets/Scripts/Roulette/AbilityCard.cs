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
        Debug.Log("클릭 되었습니다");
        RouletteManager.Instance.SelectCard(index);
    }

}
