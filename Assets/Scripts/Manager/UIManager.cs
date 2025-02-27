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

    [Header("===Button===")]
    [SerializeField] private Button profileButton;      // 프로필 버튼
    [SerializeField] private Button lobbyButton;        // 로비 버튼
    [SerializeField] private Button inventoryButton;    // 인벤토리 버튼
    [SerializeField] private Button gameStartButton;    // 게임시작 버튼
    [SerializeField] private Button storeButton;        // store button

    [Header("===Text===")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI playerNameText;

    [Header("===Profile Panel===")]
    [SerializeField] TextMeshProUGUI statePlayerName;
    [SerializeField] TextMeshProUGUI[] stateText;  

    private Stack<GameObject> PanelStack;

    private void Awake()
    {
        // 현재 panel을 lobby로
        PanelStack = new Stack<GameObject>();
        PanelStack.Push(lobbyPanel);

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

        playerNameText.text = GameManager.Instance.playerName;
        statePlayerName.text = GameManager.Instance.playerName;
    }

    // 현재 panel을 Off, 들어온패널 On
    private void OnOffPanel(GameObject panel) 
    {
        Debug.Log( panel.name + "버튼클릭 ");

        if (PanelStack.Peek().name == panel.name
            && panel.name != lobbyPanel.name)  
        {
            GameObject temp = PanelStack.Pop();
            temp.SetActive(false);
            return;
        }

        PanelStack.Push(panel);
        panel.SetActive(true);
    }

    private void ProfilePanel() 
    {
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
