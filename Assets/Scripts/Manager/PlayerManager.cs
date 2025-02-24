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

    public Transform[] target; //�߻��� ��

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

    public Vector2 GetTarget()
    {
        if (target.Length == 0) return Vector2.zero; //���� ���ٸ�

        Transform go = target[0];
        float min = Mathf.Infinity;

        foreach (var t in target)
        {
            float distance = (t.position - transform.position).sqrMagnitude;
            if (distance < min)
            {
                min = distance;
                go = t;
            }
        }

        return go.position;
    }
}
