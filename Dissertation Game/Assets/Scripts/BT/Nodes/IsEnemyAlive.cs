using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsEnemyAlive: Node
{
    private EnemyAI ai;

    public IsEnemyAlive(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        bool enemyAlive = (ai.closestEnemy != null);
        return enemyAlive ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
