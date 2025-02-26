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

    [SerializeField] private float moveSpeed = 5f;    //�̵��ӵ�
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    [SerializeField] private float attackSpeed = 1f;  //���ݼӵ�
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }
    
    [SerializeField] private float shotSpeed = 6f;     //�߻�ü�� �ӵ�
    public float ShotSpeed
    {
        get { return shotSpeed; }
    }

    [SerializeField] private int numOfOneShot = 1; //���� �߻� ����
    public int NumOfOneShot
    {
        get { return numOfOneShot; }
    }

    [SerializeField] private int numOfShooting = 1; //���� �߻�
    public int NumOfShooting
    {
        get { return numOfShooting; }
    }

    [SerializeField] private float damage = 1f;     //������ ���ݷ� ���ϱ� or �׳� ���ݷ�
    public float Damage { get { return damage; } }

    [SerializeField] private float defense = 0f;
    public float Defense { get { return defense; } }

    [SerializeField] private int gold = 100;
    public int Gold { get { return gold; } }

    private const int maxLv = 20; //�ִ뷹��
    private int level = 1;
    
    private int exp = 0;

    private int[] expGuage = new int[maxLv]; //���� �� ����ġ ��

    [SerializeField] private float critRate = 0f;     //ũ�� Ȯ��
    public float CritRate { get { return critRate; } }
    [SerializeField] private float critDamage = 1.5f;   //ũ�� ������
    public float CritDamage { get { return critDamage; } }

    public GameObject[] target; //�߻��� �� //�����Ŵ������� �� ��ȯ�� �� �߰�

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }

    private void Init() //���� �ʱ�ȭ�� ����� �Ǵ� ���
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

    public GameObject GetTarget()   //����� �� ã��
    {
        if (target.Length == 0) return null; //���� ���ٸ�

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

    public void PlayerDead()    //�÷��̾� ����� ����
    {
        //�̱���
    }

    public void UpdateStat(float change, PlayerStat stat) //���� ����
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
