using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    

    public float[] playerStat = new float[11]; //플레이어 매니저의 enum과 같음 //0번 index는 사용하지 않음

    private int gold = 100;
    public int Gold { get { return gold; } }

    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    private void Awake()
    {
        if(instance == null)
            instance = this;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStat(float change, PlayerManager.PlayerStat stat) //스탯 증가 //영구 스탯 증가
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
}
