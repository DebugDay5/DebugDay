using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        // ���� panel�� lobby��
        nowOnPanel = lobbyPanel;

        // �����ʹ�ư �̺�Ʈ 
        profileButton.onClick.AddListener(() => OnOffPanel(profilePanel)) ;
        // �κ� ��ư �̺�Ʈ 
        lobbyButton.onClick.AddListener(()=> OnOffPanel(lobbyPanel)) ;
        // �κ��丮 ��ư �̺�Ʈ
        inventoryButton.onClick.AddListener(() => OnOffPanel(inventoryPanel));
        // ���� ��ư �̺�Ʈ
        storeButton.onClick.AddListener(() => OnOffPanel(storePanel));
        // ���ӽ��� ��ư �̺�Ʈ => ���� �� load
        gameStartButton.onClick.AddListener( ()=> SceneManager.Instance.ChangeDungeonScene() );

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

}
