using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsLookAroundEstablishedNode: Node
{
    private EnemyThinker enemyThinker;

    public IsLookAroundEstablishedNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = enemyThinker.transform.position;

        return aiPosition.Equals(enemyThinker.aiRotatingPosition) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
