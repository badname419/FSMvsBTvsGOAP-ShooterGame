using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsAtLastKnownLocationNode: Node
{
    private EnemyAI ai;
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;

    public IsAtLastKnownLocationNode(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyThinker = ai.enemyThinker;
        this.enemyStats = enemyThinker.enemyStats;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = ai.transform.position;
        Vector3 targetPosition = enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
        float distance = Vector3.Distance(aiPosition, targetPosition);

        if(distance < enemyStats.arrivalDistance)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
        
    }
}
