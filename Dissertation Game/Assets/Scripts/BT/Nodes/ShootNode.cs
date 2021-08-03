using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ShootNode : Node
{
    private EnemyAI ai;
    private Shooting shooting;
    private EnemyStats enemyStats;

    public ShootNode(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyStats = ai.enemyStats;
        shooting = ai.shooting;
    }

    public override NodeState Evaluate()
    {
        shooting.Shoot(enemyStats.shootingWaitTime, ai.enemyStats.shootingDamage, ai.transform);

        return NodeState.RUNNING;
    }

}
