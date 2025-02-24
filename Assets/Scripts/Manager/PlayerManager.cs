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

    private float maxhp = 100f;
    public float MaxHp { 
        get { return maxhp; } 
        set { maxhp = value; } 
    }

    private float moveSpeed = 5f;    //�̵��ӵ�
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    private float attackSpeed = 1f;  //���ݼӵ�
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }

    private float damage = 1f;

    private int gold = 100;

    private int level = 1;
    private int exp = 0;

    private float defense = 0f;

    private float critRate = 0f;     //ũ�� Ȯ��
    private float critDamage = 1f;   //ũ�� ������


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

    public GameObject GetTarget()
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
