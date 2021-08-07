using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ShootNode : Node
{
    private Shooting shooting;
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;

    public ShootNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
        this.enemyStats = enemyThinker.enemyStats;
        this.shooting = enemyThinker.shooting;
    }

    public override NodeState Evaluate()
    {
        shooting.Shoot(enemyStats.shootingWaitTime, enemyStats.shootingDamage, enemyThinker.transform);

        return NodeState.RUNNING;
    }

}
