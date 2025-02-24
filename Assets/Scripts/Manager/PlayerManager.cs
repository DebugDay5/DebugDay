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

    [SerializeField] private float moveSpeed = 5f;    //�̵��ӵ�
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    [SerializeField] private float attackSpeed = 3f;  //���ݼӵ�
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }
    
    [SerializeField] private float shotSpeed = 3f;     //�߻�ü�� �ӵ�
    public float ShotSpeed
    {
        get { return shotSpeed; }
    }

    [SerializeField] private float damage = 1f;     //������ ���ݷ� ���ϱ� or �׳� ���ݷ�
    [SerializeField] private float defense = 0f;

    [SerializeField] private int gold = 100;

    private const int maxLv = 20; //�ִ뷹��
    private int level = 1;
    
    private int exp = 0;

    private int[] expGuage = new int[maxLv]; //���� �� ����ġ ��

    [SerializeField] private float critRate = 0f;     //ũ�� Ȯ��
    [SerializeField] private float critDamage = 1f;   //ũ�� ������

    public GameObject[] target; //�߻��� ��

    private void Awake()
    {
        instance = this;
    }

    private void Init() //���� ���� ���� �� ���� �ʱ�ȭ�� ����� �Ǵ� ���
    {
        hp = maxhp;

    }

    private void UpdateStat() //������ ����Ǵ� ��� //�Ķ���� ����
    {

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
}
