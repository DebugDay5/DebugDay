using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("===Panel===")]
    [SerializeField] private GameObject profilePanel;   // 프로필 패널
    [SerializeField] private GameObject storePanel;     // 상점 패널
    [SerializeField] private GameObject inventoryPanel; // 인벤토리 패널
    [SerializeField] private GameObject lobbyPanel;     // 로비 패널
    [SerializeField] private GameObject[] selectedIcon; // 선택 시 on 되는 아이콘

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

    [Header("===EXP===")]
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] Slider expSlider;

    private GameObject currentActivePanel; // 현재 활성화된 패널 (로비 제외)

    private void Awake()
    {
        // 현재 활성화 패널 초기화
        currentActivePanel = null;

        // 프로필버튼 이벤트 
        profileButton.onClick.AddListener(ProfilePanel);

        // 로비 버튼 이벤트 
        lobbyButton.onClick.AddListener(() => {
            OnOffPanel(null);
            UpdateSelectedIcon(1); // 로비 버튼에 해당하는 아이콘(1번째)
        });

        // 인벤토리 버튼 이벤트
        inventoryButton.onClick.AddListener(() => {
            OnOffPanel(inventoryPanel);
            UpdateSelectedIcon(2); // 인벤토리 버튼에 해당하는 아이콘(2번째)
        });

        // 상점 버튼 이벤트
        storeButton.onClick.AddListener(() => {
            OnOffPanel(storePanel);
            UpdateSelectedIcon(0); // 상점 버튼에 해당하는 아이콘(0번째)
        });

        // 게임시작 버튼 이벤트 => 던전 씬 load
        gameStartButton.onClick.AddListener( ()=> SceneManager.Instance.ChangeDungeonScene() );

        // 초기 상태 설정 - 로비 아이콘 활성화
        UpdateSelectedIcon(1);

    }

    private void Start()
    {
        UpdateGoldText();

        playerNameText.text = GameManager.Instance.playerName;
        statePlayerName.text = GameManager.Instance.playerName;

        // 경험치 ui 초기화
        UpdateExpUI();
    }

    // 현재 panel을 Off, 들어온패널 On
    private void OnOffPanel(GameObject newPanel) 
    {
        Debug.Log("패널 전환: " + (newPanel != null ? newPanel.name : "로비만"));

        // 이전에 활성화된 패널이 있고, 새 패널과 다르면 끄기
        if (currentActivePanel != null && currentActivePanel != newPanel)
        {
            currentActivePanel.SetActive(false);
        }

        // 새 패널이 null이면 로비만 표시
        if (newPanel == null)
        {
            currentActivePanel = null;
            return;
        }

        // 같은 패널을 다시 클릭한 경우 해당 패널 토글
        if (currentActivePanel == newPanel)
        {
            newPanel.SetActive(false);
            currentActivePanel = null;
            return;
        }

        // 새 패널 활성화
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
        
        // 모든 아이콘 비활성화
        for (int i = 0; i < selectedIcon.Length; i++)
        {
            selectedIcon[i].SetActive(false);
        }

        // 선택된 아이콘만 활성화
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
