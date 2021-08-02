using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredNode : Node
{
    private Transform target;
    private Transform origin;
    private EnemyAI ai;

    public IsCoveredNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        bool covered;
        Vector3 aiPosition = ai.transform.position;
        
        Vector3 targetPosition = ai.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
        if (targetPosition.Equals(Vector3.zero))
        {
            return NodeState.SUCCESS;
        }

        Vector3 direction = (targetPosition - aiPosition).normalized;
        float distance = Vector3.Distance(aiPosition, targetPosition);
        if (!Physics.Raycast(aiPosition, direction, distance, ai.enemyStats.coverMask))
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
