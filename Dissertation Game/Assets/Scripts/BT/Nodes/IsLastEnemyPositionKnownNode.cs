using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLastEnemyPositionKnownNode: Node
{
    public List<Transform> visibleEnemies = new List<Transform>();
    private EnemyThinker enemyThinker;

    public IsLastEnemyPositionKnownNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = enemyThinker.transform.position;
        int targetIndex = enemyThinker.knownEnemiesBlackboard.DetermineTheClosestEnemyIndex(aiPosition);

        return (targetIndex != -1) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
