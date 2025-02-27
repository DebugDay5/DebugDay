using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerManager;

public class UIManager : MonoBehaviour
{
    [Header("===Panel===")]
    [SerializeField] private GameObject profilePanel;   // 프로필 패널
    [SerializeField] private GameObject storePanel;     // 상점 패널
    [SerializeField] private GameObject inventoryPanel; // 인벤토리 패널
    [SerializeField] private GameObject lobbyPanel;     // 로비 패널
    [SerializeField] private GameObject nowOnPanel;     // 현재 켜져있는 panel

    [Header("===Button===")]
    [SerializeField] private Button profileButton;      // 프로필 버튼
    [SerializeField] private Button lobbyButton;        // 로비 버튼
    [SerializeField] private Button inventoryButton;    // 인벤토리 버튼
    [SerializeField] private Button gameStartButton;    // 게임시작 버튼
    [SerializeField] private Button storeButton;        // store button

    [Header("===Text===")]
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("===Profile Panel===")]
    [SerializeField] TextMeshProUGUI[] stateText;

    private void Awake()
    {
        // 현재 panel을 lobby로
        nowOnPanel = lobbyPanel;

        // 프로필버튼 이벤트 
        profileButton.onClick.AddListener(ProfilePanel);
        // 로비 버튼 이벤트 
        lobbyButton.onClick.AddListener(()=> OnOffPanel(lobbyPanel)) ;
        // 인벤토리 버튼 이벤트
        inventoryButton.onClick.AddListener(() => OnOffPanel(inventoryPanel));
        // 상점 버튼 이벤트
        storeButton.onClick.AddListener(() => OnOffPanel(storePanel));
        // 게임시작 버튼 이벤트 => 던전 씬 load
        gameStartButton.onClick.AddListener( ()=> SceneManager.Instance.ChangeDungeonScene() );

    }

    private void Start()
    {
        UpdateGoldText();
    }

    // 현재 panel을 Off, 들어온패널 On
    private void OnOffPanel(GameObject panel) 
    {
        Debug.Log( panel.name + "버튼클릭 ");

        if (nowOnPanel == panel)
        {
            nowOnPanel.SetActive(false);
        }

        // 현재 panel 끄기
        if(panel.name == "LobbyPanel")
            nowOnPanel.SetActive(false);

        nowOnPanel = panel;

        nowOnPanel.SetActive(true);
    }

    private void ProfilePanel() 
    {
        Debug.Log( "프로필 버튼클릭 ");

        for (int i = 1; i < GameManager.Instance.playerStat.Length; i++) 
        {
            stateText[i - 1].text = GameManager.Instance.playerStat[i].ToString();
        }

        OnOffPanel(profilePanel);
    }

    public void UpdateGoldText() 
    {
        goldText.text = GameManager.Instance.Gold.ToString();
    }

}
