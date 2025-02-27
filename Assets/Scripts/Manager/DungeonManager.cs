using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    public GameObject startDungeon;
    public List<GameObject> normalDungeon;  // ��ָ� ������ ����Ʈ
    public List<GameObject> hardDungeon;  // �ϵ�� ������ ����Ʈ
    public List<GameObject> bossDungeon;  //  ���� ������ ����Ʈ
    public GameObject currentDungeon;  // ���� Ȱ��ȭ�� ����

    private int currentStage = 0;  // ���� �������� ��ȣ
    public int passedNum = 0;  // ����� ���� ���� check�ϴ� �ѹ�
    public int toHardNum = 2;  // 2�� ���� ���� hard���������� �Ѿ�ٴ� ��
    public int toBossNum = 4;  // 4�� ���� ���� Boss���������� �Ѿ�ٴ� ��

    public Transform player;  // �÷��̾� ��� ���� üũ
    public DungeonSO currentDungeonData;  // ���� ���� ������. ScriptableObject�� �ҷ��� ���
    public Animator canvasAnim; //���̵��� �ƿ� ���

    private HashSet<int> usedNormalIndices = new HashSet<int>(); // �� �� ������ ���� �ε��� ����
    private HashSet<int> usedHardIndices = new HashSet<int>();
    private HashSet<int> usedBossIndices = new HashSet<int>();

    public PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    public GameObject winLosePanel;  // ���� ȭ�� �г�
    public Text winLoseText;  // ���� �ؽ�Ʈ
    public Button homeButton;  // Ȩ���� ���� ��ư

    public bool isDungeonCleared = false;  // ���� Ŭ���� ����
    [SerializeField]
    public bool isBossDungeonCleared = false;   // ���� ���� Ŭ���� ����
    private bool isGameOver = false;

    public GateCollider currentGate;  // �� �ݸ���
    private int remainingEnemies;
    private int remainingPlayer;

    private bool isClearChecked;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentDungeon = Instantiate(startDungeon);
        isClearChecked = false;
        
        currentStage = 0;  // ���� �������� ��ȣ �ʱ�ȭ
        passedNum = 0;  // ����� ���� ���� check�ϴ� �ѹ��� �ʱ�ȭ

        postProcessVolume.profile.TryGetSettings(out colorGrading);
        colorGrading.postExposure.value = 0f;  // ������ �ʱ�ȭ
        colorGrading.colorFilter.value = new Color(1f, 1f, 1f, 0);  // ������ �ʱ�ȭ

        winLosePanel.SetActive(false); // ���� ȭ���� ����
        winLoseText.gameObject.SetActive(false);  // ���� �ؽ�Ʈ ����
        homeButton.gameObject.SetActive(false);  // Ȩ ��ư ����

        remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;  // ���� �� ���� �� ����
        // remainingPlayer = GameObject.FindGameObjectsWithTag("Player").Length;  //  �÷��̾� ã��
    }

    public void SetCurrentMap(DungeonSO dungeonData)  // ���� ���� ������. ScriptableObject�� �ҷ��� ���
    {
        currentDungeonData = dungeonData;
    }



    private void Update()
    {
        if (remainingEnemies <= 0  &&  isClearChecked == false)  // �����ִ� ���� ���� 0�̸� ���� ������
        {
            OnDungeonClear();  // �� ����
            isClearChecked = true;

        }

        if (player == null || !player.gameObject.activeInHierarchy)
        {
            if (!isGameOver)  // UI �ߺ� ���� ����
            {
                Debug.Log("�÷��̾� ��� - UI ����");
                isGameOver = true;
                ShowWinLoseUI(false);
            }
        }
    }

    public void OnEnemyDead()
    {
        remainingEnemies--;

        // enemy�� �׾��� �� �� �Լ��� �̱������� ȣ���ش޶�.
        // DungeonManager.Instance.OnEnemyDead
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
                colorGrading.postExposure.value = 1.0f;  // �ϵ�� ��� ������
                colorGrading.colorFilter.value = new Color(1f,0.3f,0.3f,0);
                break;
            case 2:  // ���� �� �ε�
                currentDungeon = Instantiate(GetUniqueDungeon(bossDungeon, usedBossIndices));
                colorGrading.postExposure.value = 6.93f;  // ������ ��� �Ķ���
                colorGrading.colorFilter.value = new Color(0.0055f, 0.0105f, 0.0943f, 0);
                break;
        }

        currentGate = currentDungeon.GetComponentInChildren<GateCollider>();
        if (currentGate == null)
        {
            Debug.LogWarning("���ο� ������ GateCollider�� ����");
        }

        remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        isClearChecked = false;
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
            index = UnityEngine.Random.Range(0, dungeonList.Count);  // ���� �ε��� ����
        }   
        while (usedIndices.Contains(index));   // �������� ���� �ε����� ���� �ε����� �־��ٸ� �ٽ� �̾ƶ�
        usedIndices.Add(index);  // �װ� �ƴ϶�� �ε����� usedIndices�� �߰�

        return dungeonList[index];  // �ش� ���� ������ ��ȯ

    }

    private void OnDungeonClear()
    {
        Debug.Log("Dungeon Cleared!");

        if (currentGate != null)
        {
            currentGate.OpenGate();  // ���� Ŭ���� �� ����Ʈ�� ����
        }
        else
        {
            Debug.LogWarning("currentGate�� �������� ����. �̹� �����Ǿ��� ���ɼ� ����.");
        }
        
        passedNum++;  // ����� Ƚ�� ����
        StageChecker(); 

        if (currentStage == 3)  // ��������� ���� ����ui�� �Ѷ�.
        {
            isBossDungeonCleared = true;
            ShowWinLoseUI(true); // ������ ����� ui ���� 
        }

        if (player == null)
        {
            Debug.Log("�÷��̾� ��� - UI ����");
            isGameOver = true;  
            ShowWinLoseUI(false);  // ������ ����� ui ����
        }
    }

    public void ShowWinLoseUI(bool isWin)
    {
        winLosePanel.SetActive(true);
        winLoseText.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);


        if (winLosePanel != null)
        {
            Debug.Log("WinLoseUIPanel ���������� �����");

        }
        else
        {
            Debug.LogError("WinLoseUIPanel ���� �� ��. Inspector Ȯ��");
        }

        winLoseText.text = isWin ? "You Win! ����(��) ������ �����ߴ�!" : "You Lose! ������ ���ο� ��ȸ. ��Ȱ�Ͻðڽ��ϱ�?";

    }

    public void pushHomeBtn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }


    public void StageChecker()
    {
        if (passedNum == toHardNum)  // 2�� ����ϸ� hard stage�� �Ѿ
        {
            currentStage = 1; 
        }
        else if (passedNum == toBossNum)  // 4�� ����ϸ� boss stage�� �Ѿ
        {
            currentStage = 2; 
        }
        else if (currentStage == 2)  // ���� �������� Ŭ���� üũ
        {
            currentStage = 3;  // ���� �������� Ŭ���� �� �ٸ� ȭ������ �̵�
        }

    }

    public void ResetPosition()  // ������ �ٲ� ������ �÷��̾��� �������� 0,0,0���� ��������
    {
        player.position = Vector3.zero;
    }

    public void AdvanceToNextStage()   // ���������� ������ �� �ߵ��Ǵ� �޼���
    {
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



    
