using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouletteUi : MonoBehaviour
{
    [Header("===Name===")]
    [SerializeField] private TextMeshProUGUI[] cardName;    // 카드 이름
    [SerializeField] private Image[] cardIcon;              // 아이콘
    [SerializeField] private TextMeshProUGUI[] cardDesc;    // 카드 설명

    public void RoulettON(Ability[] abli , AblityState[] state) 
    { 
        for(int i = 0; i < abli.Length; i++) 
        {
            try
            {
                cardName[i].text = abli[i].AbilityName;
                //cardIcon[i].sprite = abli[i].Abilityicon;
                cardDesc[i].text = abli[i].AbilityDescription;
            }
            catch (Exception e) { Debug.Log(e); }
        }
    }
}
