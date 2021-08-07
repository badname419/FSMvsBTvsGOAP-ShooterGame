using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsEnemyAlive: Node
{
    private EnemyThinker enemyThinker;

    public IsEnemyAlive(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;       
    }

    public override NodeState Evaluate()
    {
        bool enemyAlive = (enemyThinker.closestEnemyObject != null);
        return enemyAlive ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
