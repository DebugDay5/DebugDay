using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    public GameObject startDungeon;
    public List<GameObject> normalDungeon;  // ��ָ� ������ ����Ʈ
    public List<GameObject> hardDungeon;  // �ϵ�� ������ ����Ʈ
    // public List<GameObject> angelDungeon;  //  �����ϴ� ����npc�� �ִ� �� (���Ŀ� �ð� �Ǹ� �߰�)
    public List<GameObject> bossDungeon;  //  ���� ������ ����Ʈ

    private GameObject currentDungeon;  // ���� Ȱ��ȭ�� ����
    private int currentStage = 0;  // ���� �������� ��ȣ

    public int passedNum = 0;  // ����� ���� ���� check�ϴ� �ѹ�
    public int toHardNum = 3;  // passedNum�� �ȿ� �� ����
    // public int toAngelNum = 5;  // passedNum�� �ȿ� �� ����
    public int toBossNum = 5;  // passedNum�� �ȿ� �� ����

    public DungeonSO currentDungeonData;  // ���� ���� ������. ScriptableObject�� �ҷ��� ���
    public Animator canvasAnim; //���̵��� �ƿ� ���

    public Transform player;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //  �� ��ȯ �� object ����
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
                if (normalDungeon.Count > 0)
                {

                    int normalIndex = Random.Range(0, normalDungeon.Count);
                    currentDungeon = Instantiate(normalDungeon[normalIndex]);
                }
                else
                {
                    Debug.LogError("��ָ� ������ ����Ʈ �������");
                }
                break;
            case 1: // �ϵ� �� �ε�
                if (hardDungeon.Count > 0)
                {
                    int hardIndex = Random.Range(0, hardDungeon.Count);
                    currentDungeon = Instantiate(hardDungeon[hardIndex]);
                }
                else
                {
                    Debug.LogError("�ϵ�� ������ ����Ʈ �������");
                }
                break;
            //case 2: // ���� �� �ε�
            //    if (angelDungeon != null)
            //    {
            //        int angelIndex = 0;
            //        currentDungeon = Instantiate(angelDungeon[0]);
            //    }
            //    else
            //    {
            //        Debug.LogError("�ϵ�� ������ ����Ʈ �������");
            //    }
            //    break;
            case 2: // ���� �� �ε�
                if (bossDungeon.Count > 0)
                {
                    int bossIndex = Random.Range(0, bossDungeon.Count);
                    currentDungeon = Instantiate(bossDungeon[bossIndex]);
                }
                else
                {
                    Debug.LogError("������ ������ ����Ʈ �������");
                }
                break;
            default:
                break;
        }
    }

    public void StageCheker()
    {
        if (passedNum == toHardNum)  // 3�� ����ϸ� hard stage�� �Ѿ
        {
            currentStage = 1; 
        }
        //else if (passedNum == toAngelNum)
        //{
        //    currentStage = 2;
        //}
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

    //ĳ���Ͱ� �δ�ġ�� ��ο���
    //��ο���ƴ��Ÿ ĳ���Ͱ� �̵���
    //�ʵ� �ٲ�
    //�׸��� �ٽ� ������� �� ���� �ٱž���
}



    
