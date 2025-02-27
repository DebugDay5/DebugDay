using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("===Panel===")]
    [SerializeField] private GameObject profilePanel;   // ������ �г�
    [SerializeField] private GameObject storePanel;     // ���� �г�
    [SerializeField] private GameObject inventoryPanel; // �κ��丮 �г�
    [SerializeField] private GameObject lobbyPanel;     // �κ� �г�
    [SerializeField] private GameObject[] selectedIcon; // ���� �� on �Ǵ� ������

    [Header("===Button===")]
    [SerializeField] private Button profileButton;      // ������ ��ư
    [SerializeField] private Button lobbyButton;        // �κ� ��ư
    [SerializeField] private Button inventoryButton;    // �κ��丮 ��ư
    [SerializeField] private Button gameStartButton;    // ���ӽ��� ��ư
    [SerializeField] private Button storeButton;        // store button

    [Header("===Text===")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI playerNameText;

    [Header("===Profile Panel===")]
    [SerializeField] TextMeshProUGUI statePlayerName;
    [SerializeField] TextMeshProUGUI[] stateText;

    [Header("===EXP===")]
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] Slider expSlider;

    private GameObject currentActivePanel; // ���� Ȱ��ȭ�� �г� (�κ� ����)

    private void Awake()
    {
        // ���� Ȱ��ȭ �г� �ʱ�ȭ
        currentActivePanel = null;

        // �����ʹ�ư �̺�Ʈ 
        profileButton.onClick.AddListener(ProfilePanel);

        // �κ� ��ư �̺�Ʈ 
        lobbyButton.onClick.AddListener(() => {
            OnOffPanel(null);
            UpdateSelectedIcon(1); // �κ� ��ư�� �ش��ϴ� ������(1��°)
        });

        // �κ��丮 ��ư �̺�Ʈ
        inventoryButton.onClick.AddListener(() => {
            OnOffPanel(inventoryPanel);
            UpdateSelectedIcon(2); // �κ��丮 ��ư�� �ش��ϴ� ������(2��°)
        });

        // ���� ��ư �̺�Ʈ
        storeButton.onClick.AddListener(() => {
            OnOffPanel(storePanel);
            UpdateSelectedIcon(0); // ���� ��ư�� �ش��ϴ� ������(0��°)
        });

        // ���ӽ��� ��ư �̺�Ʈ => ���� �� load
        gameStartButton.onClick.AddListener( ()=> SceneManager.Instance.ChangeDungeonScene() );

        // �ʱ� ���� ���� - �κ� ������ Ȱ��ȭ
        UpdateSelectedIcon(1);

    }

    private void Start()
    {
        UpdateGoldText();

        playerNameText.text = GameManager.Instance.playerName;
        statePlayerName.text = GameManager.Instance.playerName;

        // ����ġ ui �ʱ�ȭ
        UpdateExpUI();
    }

    // ���� panel�� Off, �����г� On
    private void OnOffPanel(GameObject newPanel) 
    {
        Debug.Log("�г� ��ȯ: " + (newPanel != null ? newPanel.name : "�κ�"));

        // ������ Ȱ��ȭ�� �г��� �ְ�, �� �гΰ� �ٸ��� ����
        if (currentActivePanel != null && currentActivePanel != newPanel)
        {
            currentActivePanel.SetActive(false);
        }

        // �� �г��� null�̸� �κ� ǥ��
        if (newPanel == null)
        {
            currentActivePanel = null;
            return;
        }

        // ���� �г��� �ٽ� Ŭ���� ��� �ش� �г� ���
        if (currentActivePanel == newPanel)
        {
            newPanel.SetActive(false);
            currentActivePanel = null;
            return;
        }

        // �� �г� Ȱ��ȭ
        newPanel.SetActive(true);
        currentActivePanel = newPanel;
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

    private void UpdateSelectedIcon(int activeIconIndex)
    {
        
        // ��� ������ ��Ȱ��ȭ
        for (int i = 0; i < selectedIcon.Length; i++)
        {
            selectedIcon[i].SetActive(false);
        }

        // ���õ� �����ܸ� Ȱ��ȭ
        if (activeIconIndex >= 0 && activeIconIndex < selectedIcon.Length)
        {
            selectedIcon[activeIconIndex].SetActive(true);
        }
    }

    private void UpdateExpUI() 
    {
        
        int level = GameManager.Instance.Level; ;
        expText.text = level.ToString();
        expSlider.value = GameManager.Instance.Exp / GameManager.Instance.LevelPerMaxExp(level) ;
        
    }
}
