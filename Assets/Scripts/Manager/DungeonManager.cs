using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    public List<BaseEnemy> enemies = new List<BaseEnemy>();  // 모든 적 리스트

    public GameObject startDungeon;
    public List<GameObject> normalDungeon;  // 노멀맵 프리팹 리스트
    public List<GameObject> hardDungeon;  // 하드맵 프리팹 리스트
    public List<GameObject> bossDungeon;  //  던전 프리팹 리스트
    public GameObject currentDungeon;  // 현재 활성화된 던전

    private int currentStage = 0;  // 현재 스테이지 번호
    public int passedNum = 0;  // 통과한 방의 수를 check하는 넘버
    public int toHardNum = 2;  // 2번 맵을 깨면 hard스테이지로 넘어간다는 뜻
    public int toBossNum = 4;  // 4번 맵을 깨면 Boss스테이지로 넘어간다는 듯

    public Transform player;  // 플레이어 사망 여부 체크
    public DungeonSO currentDungeonData;  // 현재 던전 데이터. ScriptableObject를 불러와 사용
    public Animator canvasAnim; //페이드인 아웃 기능
    public GateCollider currentGate;

    private HashSet<int> usedNormalIndices = new HashSet<int>(); // 한 번 등장한 맵의 인덱스 저장
    private HashSet<int> usedHardIndices = new HashSet<int>();
    private HashSet<int> usedBossIndices = new HashSet<int>();

    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    public GameObject winLosePanel;  // 승패 화면 패널
    public Text winLoseText;  // 승패 텍스트
    public Button homeButton;  // 홈으로 가는 버튼

    public bool isDungeonCleared = false;  // 던전 클리어 여부
    private bool isBossDungeonCleared = false;   // 보스 던전 클리어 여부

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentDungeon = Instantiate(startDungeon);
        
        currentStage = 0;  // 현재 스테이지 번호 초기화
        passedNum = 0;  // 통과한 방의 수를 check하는 넘버를 초기화

        postProcessVolume.profile.TryGetSettings(out colorGrading);   
        colorGrading.postExposure.value = 0f;  // 배경색상 초기화
        colorGrading.colorFilter.value = new Color(1f, 1f, 1f, 0);  // 배경색상 초기화

        winLosePanel.SetActive(false); // 승패 화면을 숨김
        winLoseText.gameObject.SetActive(false);  // Text 숨김
        homeButton.gameObject.SetActive(false);  // 홈 버튼 숨김
    }

    public void SetCurrentMap(DungeonSO dungeonData)  // 현재 던전 데이터. ScriptableObject를 불러와 사용
    {
        currentDungeonData = dungeonData;
    }

    private void Update()
    {
        if(!isDungeonCleared)  // 던전이 클리어되지 않았으면 던전 클리어 체크
        {
            CheckDungeonClear(); 
        }

        if (player == null)   // 플레이어 사망하면 패배 처리
        {
            OnDungeonFail();  
        }

    }

    public void AddEnemy(BaseEnemy enemy) 
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(BaseEnemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public void LoadCurrentDungeon()   // 현재 스테이지에 따라 맵을 로드하는 매서드
    {

        if(currentDungeon != null)  
        {
            Destroy(currentDungeon);
        }

        switch (currentStage)
        {
            case 0:  // 노멀 맵 로드
                currentDungeon = Instantiate(GetUniqueDungeon(normalDungeon, usedNormalIndices));
                break;
            case 1: // 하드 맵 로드
                currentDungeon = Instantiate(GetUniqueDungeon(hardDungeon, usedHardIndices));
                colorGrading.postExposure.value = 1.0f;  // 하드맵 배경 붉은색
                colorGrading.colorFilter.value = new Color(1f,0.3f,0.3f,0);
                break;
            case 2:  // 보스 맵 로드
                currentDungeon = Instantiate(GetUniqueDungeon(bossDungeon, usedBossIndices));
                colorGrading.postExposure.value = 6.93f;  // 보스맵 배경 파란색
                colorGrading.colorFilter.value = new Color(0.0055f, 0.0105f, 0.0943f, 0);
                break;
        }

    }

    private GameObject GetUniqueDungeon(List<GameObject> dungeonList, HashSet<int> usedIndices)
    {
        if (dungeonList.Count == 0)
        {
            Debug.LogError("던전 프리팹 리스트 비어있음");
            return null;
        }

        if (usedIndices.Count >= dungeonList.Count)
        {
            usedIndices.Clear();  // 모든 맵이 사용되었으면 초기화
        }

        int index;
        do
        {
            index = UnityEngine.Random.Range(0, dungeonList.Count);  // 랜덤 인덱스 생성
        }   
        while (usedIndices.Contains(index));   // 랜덤으로 뽑은 인덱스가 기존 인덱스에 있었다면 다시 뽑아라
        usedIndices.Add(index);  // 그게 아니라면 인덱스를 usedIndices에 추가

        return dungeonList[index];  // 해당 던전 프리팹 반환

    }

    public void CheckDungeonClear()
    {
        if (enemies.Count == 0 && !isDungeonCleared)  // 모든 적이 처치되었을 때 던전 클리어
        {
            isDungeonCleared = true;
            OnDungeonClear();
        }
    }

    private void OnDungeonClear()
    {
        Debug.Log("Dungeon Cleared!");
        currentGate.OpenGate();  // 던전 클리어 후 게이트를 연다
        
        passedNum++;  // 통과한 횟수 증가
        StageChecker(); 

        if (currentStage == 2)  // 보스방까지 깨면 승패ui를 켜라.
        {
            isBossDungeonCleared = true;
            ShowWinLoseUI(true); 
        }

    }

    public void OnDungeonFail()
    {
        Debug.Log("Dungeon Failed!");
        ShowWinLoseUI(false);
    }

    public void ShowWinLoseUI(bool isWin)
    {
        winLosePanel.SetActive(true);
        winLoseText.text = isWin ? "용사는(은) 던전을 정복했다!" : "죽음은 새로운 기회";
        homeButton.gameObject.SetActive(true);
    }

    public void OnHomeButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void StageChecker()
    {
        if (passedNum == toHardNum)  // 2번 통과하면 hard stage로 넘어감
        {
            currentStage = 1; 
        }
        else if (passedNum == toBossNum)  // 4번 통과하면 boss stage로 넘어감
        {
            currentStage = 2; 
        }

    }

    public void ResetPosition()  // 던전이 바뀔 때마다 플레이어의 포지션을 0,0,0으로 리셋해줌
    {
        player.position = Vector3.zero;
    }

    public void AdvanceToNextStage()   // 스테이지가 증가될 때 발동되는 메서드
    {
        StartCoroutine(NextStage());
    }

    IEnumerator NextStage()
    {
        canvasAnim.SetTrigger("Out");   // 페이드 아웃
        yield return new WaitForSeconds(0.3f);
        ResetPosition();  // 플레이어 위치 0,0,0으로 리셋
        LoadCurrentDungeon();  // 새로운 던전 진입
        canvasAnim.SetTrigger("In");  // 페이드 인
    }

}



    
