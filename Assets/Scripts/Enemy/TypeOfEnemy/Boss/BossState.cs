using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState
{
    protected BossEnemy boss;
    public BossState(BossEnemy boss) { this.boss = boss; }
    public abstract void UpdateState();
    public abstract void Attack();
}