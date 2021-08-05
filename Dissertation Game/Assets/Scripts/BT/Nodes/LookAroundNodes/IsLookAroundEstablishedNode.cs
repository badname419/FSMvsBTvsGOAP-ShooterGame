using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsLookAroundEstablishedNode: Node
{
    private EnemyAI ai;
    private EnemyThinker enemyThinker;

    public IsLookAroundEstablishedNode(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = ai.transform.position;

        return aiPosition.Equals(enemyThinker.aiRotatingPosition) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
