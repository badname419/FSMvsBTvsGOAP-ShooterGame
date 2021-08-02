using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLastEnemyPositionKnownNode: Node
{
    public List<Transform> visibleEnemies = new List<Transform>();

    private FieldOfView fieldOfView;
    private EnemyAI ai;
    private EnemyAI enemyAI;

    public IsLastEnemyPositionKnownNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = ai.transform.position;
        int targetIndex = ai.knownEnemiesBlackboard.DetermineTheClosestEnemyIndex(aiPosition);

        return (targetIndex != -1) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
