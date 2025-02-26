using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    // private int totalEnemies = 0;  // 총 몬스터 수
    // private int defeatedEnemies = 0; // 처치된 몬스터 수

    public GameObject startDungeon;
    public List<GameObject> normalDungeon;  // 노멀맵 프리팹 리스트
    public List<GameObject> hardDungeon;  // 하드맵 프리팹 리스트
    public List<GameObject> bossDungeon;  //  던전 프리팹 리스트
    private GameObject currentDungeon;  // 현재 활성화된 던전

    private int currentStage = 0;  // 현재 스테이지 번호

    public int passedNum = 0;  // 통과한 방의 수를 check하는 넘버
    public int toHardNum = 3;  // passedNum값 안에 들어갈 숫자
    public int toBossNum = 5;  // passedNum값 안에 들어갈 숫자

    public Transform player;
    public DungeonSO currentDungeonData;  // 현재 던전 데이터. ScriptableObject를 불러와 사용
    public Animator canvasAnim; //페이드인 아웃 기능

    private HashSet<int> usedNormalIndices = new HashSet<int>(); // 한 번 등장한 맵의 인덱스 저장
    private HashSet<int> usedHardIndices = new HashSet<int>();
    private HashSet<int> usedBossIndices = new HashSet<int>();

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
                        // 핸드폰 화면 꺼놨다가 다시 했을 때 이어하기 되는 기능을 하려면 이 부분이 세이브가 되어야.
    }

    //public void EnemyDefeated()
    //{
    //    defeatedEnemies++;
    //    if (defeatedEnemies >= totalEnemies)
    //    {
    //        GateCollider.Instance.OpenGate();  // 게이트 열기
    //    }
    //}


    //  ** 관우님께
    //  BaseEnemy 스크립트 Die() 파트에 아래 예시와 같은 식을 추가하여
    //  DungeonManager EnemyDefeated 함수에 몬스터가 처치되었음을 알리는 신호를 보내주시면
    //  주석처리한 스크립트들이 원활히 가동될 것 같습니다. 
    //      if (DungeonManager.Instance != null)
    //      {
    //          DungeonManager.Instance.EnemyDefeated();
    //      }


    // public void SetTotalEnemies (int count)
    // {
    //    SetTotalEnemies = count;
    // }

    public void SetCurrentMap(DungeonSO dungeonData)  // 현재 던전 데이터. ScriptableObject를 불러와 사용
    {
        currentDungeonData = dungeonData;
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
                break;
            case 2:  // 보스 맵 로드
                currentDungeon = Instantiate(GetUniqueDungeon(bossDungeon, usedHardIndices));
                break;
        }

        SetTotalEnemies(FindObjectOfType<Enemy>().Length);  // 던전 로드 후 몬스터 수 설정

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
            index = Random.Range(0, dungeonList.Count);  // 랜덤 인덱스 생성
        }   
        while (usedIndices.Contains(index));   // 랜덤으로 뽑은 인덱스가 기존 인덱스에 있었다면 다시 뽑아라
        usedIndices.Add(index);  // 그게 아니라면 인덱스를 usedIndices에 추가

        return dungeonList[index];  // 해당 던전 프리팹 반환

    }

    public void StageCheker()
    {
        if (passedNum == toHardNum)  // 3번 통과하면 hard stage로 넘어감
        {
            currentStage = 1; 
        }
        else if (passedNum == toBossNum)  // 5번 통과하면 boss stage로 넘어감
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
        StageCheker();
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



    
