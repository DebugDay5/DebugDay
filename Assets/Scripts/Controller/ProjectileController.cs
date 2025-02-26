using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject target; //�׽�Ʈ��
    private float damage = 1f;
    public float Damage
    {
        get { return damage; }
    }

    private float critRate = 0f;     //ũ�� Ȯ��
    private float critDamage = 1f;   //ũ�� ������ ����
    private bool isPiercing = false; //���뿩��

    private int wallBounceCount = 0; //�߻�ü �� ƨ�� //������ ����
    private float lifeTime = 5f; //�����ð� �� �߻�ü ����

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(this.gameObject);
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {


        Vector2 incoming = GetComponent<Rigidbody2D>().velocity; //������ ����
        Vector2 normal = collision.contacts[0].normal; //��������
        Vector2 reflect = Vector2.Reflect(incoming, normal); //�ݻ� ����
        GetComponent<Rigidbody2D>().velocity = reflect;

    }
     *///�̿ϼ�

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            if(Random.Range(0f, 1f) < critRate)
            {
                damage *= critDamage;
            }

            /*
            * ���Ϳ��� ������...
            */

            if (!isPiercing)
                Destroy(this.gameObject);
        }

        //�� �浹�� ����
        //if(other.CompareTag("Wall") Destroy(this.gameObject);
    }

    public void Init(float damage, float critRate, float critDamage, bool isPiercing)
    {
        this.damage = damage;
        this.critRate = critRate;
        this.critDamage = critDamage;
        this.isPiercing = isPiercing;
    }
}
