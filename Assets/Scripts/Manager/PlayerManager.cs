using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum PlayerStat
    {
        Damage = 1,
        MaxHP,
        Defence,
        CritRate,
        CritDamage,
        MoveSpeed,
        AttackSpeed,
        ShotSpeed,
        NumOfOneShot,
        NumOfShooting
    }

    private static PlayerManager instance;
    public static PlayerManager Instance { get => instance; }

    [SerializeField] private float hp = 100f;
    public float Hp { 
        get { return hp; } 
        set { hp = Mathf.Clamp(value, 0f, maxhp); } 
    }

    [SerializeField] private float maxhp = 100f;
    public float MaxHp { 
        get { return maxhp; } 
        set { maxhp = value; } 
    }

    [SerializeField] private float moveSpeed = 5f;    //이동속도
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    [SerializeField] private float attackSpeed = 1f;  //공격속도
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }
    
    [SerializeField] private float shotSpeed = 6f;     //발사체의 속도
    public float ShotSpeed
    {
        get { return shotSpeed; }
    }

    [SerializeField] private int numOfOneShot = 1; //동시 발사 개수
    public int NumOfOneShot
    {
        get { return numOfOneShot; }
    }

    [SerializeField] private int numOfShooting = 1; //연속 발사
    public int NumOfShooting
    {
        get { return numOfShooting; }
    }

    [SerializeField] private float damage = 1f;     //아이템 공격력 곱하기 or 그냥 공격력
    public float Damage { get { return damage; } }

    [SerializeField] private float defense = 0f;
    public float Defense { get { return defense; } }

    [SerializeField] private int gold = 100;
    public int Gold { get { return gold; } }

    private const int maxLv = 20; //최대레벨
    private int level = 1;
    
    private int exp = 0;

    private int[] expGuage = new int[maxLv]; //레벨 업 경험치 통

    [SerializeField] private float critRate = 0f;     //크리 확률
    public float CritRate { get { return critRate; } }
    [SerializeField] private float critDamage = 1.5f;   //크리 데미지
    public float CritDamage { get { return critDamage; } }

    public GameObject[] target; //발사할 적 //던전매니저에서 몸 소환할 때 추가

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }

    private void Init() //스탯 초기화를 해줘야 되는 경우
    {
        damage = 1f; 
        maxhp = 100f;
        hp = maxhp;
        defense = 0f;
        critDamage = 1.5f;
        critRate = 0f;
        moveSpeed = 5f;
        attackSpeed = 1f;
        shotSpeed = 6f;
        numOfShooting = 1;
        numOfOneShot = 1;

        level = 1;
        exp = 0;
    }

    public GameObject GetTarget()   //가까운 적 찾기
    {
        if (target.Length == 0) return null; //적이 없다면

        GameObject go = target[0];
        float min = Mathf.Infinity;

        foreach (var t in target)
        {
            float distance = (t.transform.position - transform.position).sqrMagnitude;
            if (distance < min)
            {
                min = distance;
                go = t;
            }
        }

        return go;
    }

    public void PlayerDead()    //플레이어 사망시 실행
    {
        //미구현
    }

    public void UpdateStat(float change, PlayerStat stat) //스탯 증가
    {
        int num = (int)stat;
        switch (num)
        {
            case 1:
                damage += change;
                break;
            case 2:
                MaxHp += change;
                break;
            case 3:
                defense += change;
                break;
            case 4:
                critRate += change;
                break;
            case 5:
                critDamage += change;
                break;
            case 6:
                moveSpeed += change;
                break;
            case 7:
                attackSpeed += change;
                break;
            case 8:
                shotSpeed += change;
                break;
            case 9:
                numOfOneShot += (int)change;
                break;
            case 10:
                numOfShooting += (int)change;
                break;
        }
    }

}
