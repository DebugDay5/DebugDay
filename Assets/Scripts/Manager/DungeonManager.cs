using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    // private int totalEnemies = 0;  // �� ���� ��
    // private int defeatedEnemies = 0; // óġ�� ���� ��

    public GameObject startDungeon;
    public List<GameObject> normalDungeon;  // ��ָ� ������ ����Ʈ
    public List<GameObject> hardDungeon;  // �ϵ�� ������ ����Ʈ
    public List<GameObject> bossDungeon;  //  ���� ������ ����Ʈ
    private GameObject currentDungeon;  // ���� Ȱ��ȭ�� ����

    private int currentStage = 0;  // ���� �������� ��ȣ

    public int passedNum = 0;  // ����� ���� ���� check�ϴ� �ѹ�
    public int toHardNum = 3;  // passedNum�� �ȿ� �� ����
    public int toBossNum = 5;  // passedNum�� �ȿ� �� ����

    public Transform player;
    public DungeonSO currentDungeonData;  // ���� ���� ������. ScriptableObject�� �ҷ��� ���
    public Animator canvasAnim; //���̵��� �ƿ� ���

    private HashSet<int> usedNormalIndices = new HashSet<int>(); // �� �� ������ ���� �ε��� ����
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
        currentStage = 0;  // ���� �������� ��ȣ �ʱ�ȭ
        passedNum = 0;  // ����� ���� ���� check�ϴ� �ѹ��� �ʱ�ȭ
                        // �ڵ��� ȭ�� �����ٰ� �ٽ� ���� �� �̾��ϱ� �Ǵ� ����� �Ϸ��� �� �κ��� ���̺갡 �Ǿ��.
    }

    //public void EnemyDefeated()
    //{
    //    defeatedEnemies++;
    //    if (defeatedEnemies >= totalEnemies)
    //    {
    //        GateCollider.Instance.OpenGate();  // ����Ʈ ����
    //    }
    //}


    //  ** ����Բ�
    //  BaseEnemy ��ũ��Ʈ Die() ��Ʈ�� �Ʒ� ���ÿ� ���� ���� �߰��Ͽ�
    //  DungeonManager EnemyDefeated �Լ��� ���Ͱ� óġ�Ǿ����� �˸��� ��ȣ�� �����ֽø�
    //  �ּ�ó���� ��ũ��Ʈ���� ��Ȱ�� ������ �� �����ϴ�. 
    //      if (DungeonManager.Instance != null)
    //      {
    //          DungeonManager.Instance.EnemyDefeated();
    //      }


    // public void SetTotalEnemies (int count)
    // {
    //    SetTotalEnemies = count;
    // }

    public void SetCurrentMap(DungeonSO dungeonData)  // ���� ���� ������. ScriptableObject�� �ҷ��� ���
    {
        currentDungeonData = dungeonData;
    }

    public void LoadCurrentDungeon()   // ���� ���������� ���� ���� �ε��ϴ� �ż���
    {
        if(currentDungeon != null)  
        {
            Destroy(currentDungeon);
        }

        switch (currentStage)
        {
            case 0:  // ��� �� �ε�
                currentDungeon = Instantiate(GetUniqueDungeon(normalDungeon, usedNormalIndices));
                break;
            case 1: // �ϵ� �� �ε�
                currentDungeon = Instantiate(GetUniqueDungeon(hardDungeon, usedHardIndices));
                break;
            case 2:  // ���� �� �ε�
                currentDungeon = Instantiate(GetUniqueDungeon(bossDungeon, usedHardIndices));
                break;
        }

        SetTotalEnemies(FindObjectOfType<Enemy>().Length);  // ���� �ε� �� ���� �� ����

    }

    private GameObject GetUniqueDungeon(List<GameObject> dungeonList, HashSet<int> usedIndices)
    {
        if (dungeonList.Count == 0)
        {
            Debug.LogError("���� ������ ����Ʈ �������");
            return null;
        }

        if (usedIndices.Count >= dungeonList.Count)
        {
            usedIndices.Clear();  // ��� ���� ���Ǿ����� �ʱ�ȭ
        }

        int index;
        do
        {
            index = Random.Range(0, dungeonList.Count);  // ���� �ε��� ����
        }   
        while (usedIndices.Contains(index));   // �������� ���� �ε����� ���� �ε����� �־��ٸ� �ٽ� �̾ƶ�
        usedIndices.Add(index);  // �װ� �ƴ϶�� �ε����� usedIndices�� �߰�

        return dungeonList[index];  // �ش� ���� ������ ��ȯ

    }

    public void StageCheker()
    {
        if (passedNum == toHardNum)  // 3�� ����ϸ� hard stage�� �Ѿ
        {
            currentStage = 1; 
        }
        else if (passedNum == toBossNum)  // 5�� ����ϸ� boss stage�� �Ѿ
        {
            currentStage = 2; 
        }

    }

    public void ResetPosition()  // ������ �ٲ� ������ �÷��̾��� �������� 0,0,0���� ��������
    {
        player.position = Vector3.zero;
    }

    public void AdvanceToNextStage()   // ���������� ������ �� �ߵ��Ǵ� �޼���
    {
        StageCheker();
        StartCoroutine(NextStage());
    }

    IEnumerator NextStage()
    {
        canvasAnim.SetTrigger("Out");   // ���̵� �ƿ�
        yield return new WaitForSeconds(0.3f);
        ResetPosition();  // �÷��̾� ��ġ 0,0,0���� ����
        LoadCurrentDungeon();  // ���ο� ���� ����
        canvasAnim.SetTrigger("In");  // ���̵� ��
    }

}



    
