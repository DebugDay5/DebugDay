using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    public GameObject startDungeon;
    public List<GameObject> normalDungeon;  // 노멀맵 프리팹 리스트
    public List<GameObject> hardDungeon;  // 하드맵 프리팹 리스트
    // public List<GameObject> angelDungeon;  //  버프하는 엔젤npc가 있는 맵 (이후에 시간 되면 추가)
    public List<GameObject> bossDungeon;  //  던전 프리팹 리스트

    private GameObject currentDungeon;  // 현재 활성화된 던전
    private int currentStage = 0;  // 현재 스테이지 번호

    public int passedNum = 0;  // 통과한 방의 수를 check하는 넘버
    public int toHardNum = 3;  // passedNum값 안에 들어갈 숫자
    // public int toAngelNum = 5;  // passedNum값 안에 들어갈 숫자
    public int toBossNum = 5;  // passedNum값 안에 들어갈 숫자

    public DungeonSO currentDungeonData;  // 현재 던전 데이터. ScriptableObject를 불러와 사용
    public Animator canvasAnim; //페이드인 아웃 기능

    public Transform player;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //  씬 전환 시 object 유지
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
                if (normalDungeon.Count > 0)
                {

                    int normalIndex = Random.Range(0, normalDungeon.Count);
                    currentDungeon = Instantiate(normalDungeon[normalIndex]);
                }
                else
                {
                    Debug.LogError("노멀맵 프리팹 리스트 비어있음");
                }
                break;
            case 1: // 하드 맵 로드
                if (hardDungeon.Count > 0)
                {
                    int hardIndex = Random.Range(0, hardDungeon.Count);
                    currentDungeon = Instantiate(hardDungeon[hardIndex]);
                }
                else
                {
                    Debug.LogError("하드맵 프리팹 리스트 비어있음");
                }
                break;
            //case 2: // 엔젤 맵 로드
            //    if (angelDungeon != null)
            //    {
            //        int angelIndex = 0;
            //        currentDungeon = Instantiate(angelDungeon[0]);
            //    }
            //    else
            //    {
            //        Debug.LogError("하드맵 프리팹 리스트 비어있음");
            //    }
            //    break;
            case 2: // 보스 맵 로드
                if (bossDungeon.Count > 0)
                {
                    int bossIndex = Random.Range(0, bossDungeon.Count);
                    currentDungeon = Instantiate(bossDungeon[bossIndex]);
                }
                else
                {
                    Debug.LogError("보스맵 프리팹 리스트 비어있음");
                }
                break;
            default:
                break;
        }
    }

    public void StageCheker()
    {
        if (passedNum == toHardNum)  // 3번 통과하면 hard stage로 넘어감
        {
            currentStage = 1; 
        }
        //else if (passedNum == toAngelNum)
        //{
        //    currentStage = 2;
        //}
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

    //캐릭터가 부닥치면 어두워짐
    //어두워진틈을타 캐릭터가 이동됨
    //맵도 바뀜
    //그리고 다시 밝아지면 오 맵이 바궈엇군
}



    
