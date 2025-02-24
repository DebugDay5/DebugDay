using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState
{
    protected BossEnemy boss;
    protected GameObject projectilePrefab;
    public BossState(BossEnemy boss)
    {
        this.boss = boss;
        projectilePrefab = boss.projectilePrefab;
    }

    public abstract void UpdateState();
    public abstract void Attack();
}