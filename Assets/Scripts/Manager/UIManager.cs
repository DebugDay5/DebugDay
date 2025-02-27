using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerManager;

public class UIManager : MonoBehaviour
{
    [Header("===Panel===")]
    [SerializeField] private GameObject profilePanel;   // ������ �г�
    [SerializeField] private GameObject storePanel;     // ���� �г�
    [SerializeField] private GameObject inventoryPanel; // �κ��丮 �г�
    [SerializeField] private GameObject lobbyPanel;     // �κ� �г�
    [SerializeField] private GameObject nowOnPanel;     // ���� �����ִ� panel

    [Header("===Button===")]
    [SerializeField] private Button profileButton;      // ������ ��ư
    [SerializeField] private Button lobbyButton;        // �κ� ��ư
    [SerializeField] private Button inventoryButton;    // �κ��丮 ��ư
    [SerializeField] private Button gameStartButton;    // ���ӽ��� ��ư
    [SerializeField] private Button storeButton;        // store button

    [Header("===Text===")]
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("===Profile Panel===")]
    [SerializeField] TextMeshProUGUI[] stateText;

    private void Awake()
    {
        // ���� panel�� lobby��
        nowOnPanel = lobbyPanel;

        // �����ʹ�ư �̺�Ʈ 
        profileButton.onClick.AddListener(ProfilePanel);
        // �κ� ��ư �̺�Ʈ 
        lobbyButton.onClick.AddListener(()=> OnOffPanel(lobbyPanel)) ;
        // �κ��丮 ��ư �̺�Ʈ
        inventoryButton.onClick.AddListener(() => OnOffPanel(inventoryPanel));
        // ���� ��ư �̺�Ʈ
        storeButton.onClick.AddListener(() => OnOffPanel(storePanel));
        // ���ӽ��� ��ư �̺�Ʈ => ���� �� load
        gameStartButton.onClick.AddListener( ()=> SceneManager.Instance.ChangeDungeonScene() );

    }

    private void Start()
    {
        UpdateGoldText();
    }

    // ���� panel�� Off, �����г� On
    private void OnOffPanel(GameObject panel) 
    {
        Debug.Log( panel.name + "��ưŬ�� ");

        if (nowOnPanel == panel)
        {
            nowOnPanel.SetActive(false);
        }

        // ���� panel ����
        if(panel.name == "LobbyPanel")
            nowOnPanel.SetActive(false);

        nowOnPanel = panel;

        nowOnPanel.SetActive(true);
    }

    private void ProfilePanel() 
    {
        Debug.Log( "������ ��ưŬ�� ");

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
