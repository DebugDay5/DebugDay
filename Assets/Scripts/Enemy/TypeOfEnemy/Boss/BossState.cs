using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState
{
    protected BossEnemy boss;
    protected GameObject firstProjectilePrefab;
    protected GameObject stone;
    public PlayerController playerController;
    public BossState(BossEnemy boss)
    {
        this.boss = boss;
        firstProjectilePrefab = boss.firstProjectilePrefab;
        stone = boss.stone;
        playerController = boss.playerController;
    }

    public abstract void UpdateState();
    public abstract void Attack();
}