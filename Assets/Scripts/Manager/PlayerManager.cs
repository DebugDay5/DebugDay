using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance { get => instance; }

    private float hp = 100f;
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

    [SerializeField] private float attackSpeed = 3f;  //공격속도
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }
    
    [SerializeField] private float shotSpeed = 3f;     //발사체의 속도
    public float ShotSpeed
    {
        get { return shotSpeed; }
    }

    [SerializeField] private float damage = 1f;     //아이템 공격력 곱하기 or 그냥 공격력
    [SerializeField] private float defense = 0f;

    [SerializeField] private int gold = 100;

    private const int maxLv = 20; //최대레벨
    private int level = 1;
    
    private int exp = 0;

    private int[] expGuage = new int[maxLv]; //레벨 업 경험치 통

    [SerializeField] private float critRate = 0f;     //크리 확률
    [SerializeField] private float critDamage = 1f;   //크리 데미지

    public GameObject[] target; //발사할 적

    private void Awake()
    {
        instance = this;
    }

    private void Init() //만약 던전 입장 시 마다 초기화를 해줘야 되는 경우
    {
        hp = maxhp;

    }

    private void UpdateStat() //스탯이 변경되는 경우 //파라미터 미정
    {

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
}
