using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AtRotationPositionNode: Node
{
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;
    private EnemyAI ai;

    public AtRotationPositionNode(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyStats = ai.enemyStats;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        if (!enemyThinker.aiRotatingPosition.Equals(Vector3.zero))
        {
            Vector3 aiPosition = ai.transform.position;
            float distance = Vector3.Distance(aiPosition, enemyThinker.aiRotatingPosition);

            if(distance < enemyStats.arrivalDistance)
            {
                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }
}
