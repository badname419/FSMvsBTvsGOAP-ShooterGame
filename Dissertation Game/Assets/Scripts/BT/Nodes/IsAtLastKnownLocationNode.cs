using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsAtLastKnownLocationNode: Node
{
    private EnemyAI ai;
    private KnownEnemiesBlackboard knownEnemiesBlackboard;
    private EnemyStats enemyStats;

    public IsAtLastKnownLocationNode(EnemyAI ai)
    {
        this.ai = ai;
        this.knownEnemiesBlackboard = ai.knownEnemiesBlackboard;
        this.enemyStats = ai.enemyStats;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = ai.transform.position;
        Vector3 targetPosition = knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
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
