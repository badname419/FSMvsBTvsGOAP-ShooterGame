using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredNode : Node
{
    private EnemyThinker enemyThinker;

    public IsCoveredNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
    }

    public override NodeState Evaluate()
    {
        bool covered;
        Vector3 aiPosition = enemyThinker.transform.position;
        
        Vector3 targetPosition = enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
        if (targetPosition.Equals(Vector3.zero))
        {
            return NodeState.SUCCESS;
        }

        Vector3 direction = (targetPosition - aiPosition).normalized;
        float distance = Vector3.Distance(aiPosition, targetPosition);
        if (!Physics.Raycast(aiPosition, direction, distance, enemyThinker.enemyStats.coverMask))
        {
            covered = false;
        }
        else
        {
            covered = true;
        }
 
        return covered ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
