using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject target; //테스트용
    private float damage = 1f;
    public float Damage
    {
        get { return damage; }
    }

    private float critRate = 0f;     //크리 확률
    private float critDamage = 1f;   //크리 데미지 배율
    private bool isPiercing = false; //관통여부

    private int wallBounceCount = 0; //발사체 벽 튕김 //구현은 아직
    private float lifeTime = 5f; //일정시간 뒤 발사체 삭제

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(this.gameObject);
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {


        Vector2 incoming = GetComponent<Rigidbody2D>().velocity; //들어오는 벡터
        Vector2 normal = collision.contacts[0].normal; //법선벡터
        Vector2 reflect = Vector2.Reflect(incoming, normal); //반사 벡터
        GetComponent<Rigidbody2D>().velocity = reflect;

    }
     *///미완성

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            if(Random.Range(0f, 1f) < critRate)
            {
                damage *= critDamage;
            }

            /*
            * 몬스터에게 데미지...
            */

            if (!isPiercing)
                Destroy(this.gameObject);
        }

        //벽 충돌시 삭제
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
