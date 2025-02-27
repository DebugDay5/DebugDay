using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Header("===playerState===")]

    public float[] playerStat = new float[11]; //�÷��̾� �Ŵ����� enum�� ���� //0�� index�� ������� ����
    public string playerName;

    [Header("===GOld===")]
    private int gold = 100;
    public int Gold { get { return gold; } }

    private const int maxLv = 20; //�ִ뷹��
    private int level = 1;

    private int exp = 0;

    private int[] expGuage = new int[maxLv] {
        10, 20, 30, 40, 50, 60, 70, 80, 90, 100,
        110, 120, 130, 140, 150, 160, 170, 180, 190, 200
    }; //���� �� ����ġ ��

    private void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϰ� ���� ������Ʈ�� �ƴ϶��
        if (instance != null && instance != this)
        {
            // �ߺ��� �ν��Ͻ� ����
            Destroy(gameObject);
            return;
        }

        // �ν��Ͻ��� ������ ���� �ν��Ͻ� ����
        instance = this;

        playerName = "���ĸ�Ÿ";

        // �÷��̾� ���� �ʱ�ȭ
        playerStat[0] = 0f;     //no usage
        playerStat[1] = 1f;     //Damage
        playerStat[2] = 100f;   //MaxHP
        playerStat[3] = 0f;     //Defence
        playerStat[4] = 0f;     //CritRate
        playerStat[5] = 1f;     //CritDamage
        playerStat[6] = 5f;     //MoveSpeed
        playerStat[7] = 1f;     //AttackSpeed
        playerStat[8] = 10f;    //ShotSpeed
        playerStat[9] = 1f;     //NumOfOneShot
        playerStat[10] = 1f;    //NumOfShooting

    }

    public void UpdateGold(int g) 
    {
        this.gold += g;
    }

    public void UpdateStat(float change, PlayerManager.PlayerStat stat) //���� ���� //���� ���� ����
    {
        int num = (int)stat;
        switch (num)
        {
            case 1: //damage
                playerStat[1] += change;
                break;
            case 2: //MaxHp
                playerStat[2] += change;
                break;
            case 3://Defence
                playerStat[3] += change;
                break;
            case 4://CritRate
                playerStat[4] += change;
                break;
            case 5://CritDamage
                playerStat[5] += change;
                break;
            case 6://MoveSpeed
                playerStat[6] += change;
                break;
            case 7://AttackSpeed
                playerStat[7] += change;
                break;
            case 8://ShotSpeed
                playerStat[8] += change;
                break;
            case 9://NumOfOneShot
                playerStat[9] += (int)change;
                break;
            case 10://NumOfShooting
                playerStat[10] += (int)change;
                break;
        }
    }

    public void GetExp(int amount)
    {
        if (maxLv == level) return;

        exp += amount;

        if (exp >= expGuage[level - 1])
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        exp -= expGuage[level - 1];
        level++;

        /*
         * give additional stat 
         */

    }
}
