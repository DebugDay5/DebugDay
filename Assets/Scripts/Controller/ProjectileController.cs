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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("충돌!");
        
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
