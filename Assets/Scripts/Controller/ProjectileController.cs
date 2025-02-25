using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject target; //테스트용
    private float damage;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("충돌!");
        if(other.CompareTag("Enemy"))
            Destroy(this.gameObject);
    }
}
