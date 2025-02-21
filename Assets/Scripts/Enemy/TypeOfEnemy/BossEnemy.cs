using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int attackPatternCount = 5;

    protected void Awake()
    {
        hp = 1000;
        speed = 0;
        gold = 100;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(BossAttackPattern());
    }
    
    private IEnumerator BossAttackPattern()
    {
        while (hp > 0)
        {
            Attack();
            yield return new WaitForSeconds(attackCooldown);
        }
    }
    
    public override void Attack()
    {
        int pattern = Random.Range(0, attackPatternCount);
        switch (pattern)
        {
            case 0:
                Debug.Log("패턴 1: 범위 공격");
                break;
            case 1:
                Debug.Log("패턴 2: 투사체 발사");
                break;
            case 2:
                Debug.Log("패턴 3: 다중 타겟 공격");
                break;
            case 3:
                Debug.Log("패턴 4: 광역 스턴");
                break;
            case 4:
                Debug.Log("패턴 5: 체력 회복");
                break;
        }
    }
}
