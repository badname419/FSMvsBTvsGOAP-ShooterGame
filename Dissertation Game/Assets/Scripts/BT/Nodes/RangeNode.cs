using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    private float range;
    //private int target;
    private EnemyAI ai;
    private FieldOfView fieldOfView;
    private EnemyAI.Target target;

    public RangeNode(float range, EnemyAI ai, EnemyAI.Target target)
    {
        this.range = range;
        this.target = target;
        this.ai = ai;
        this.fieldOfView = ai.fieldOfView;
    }

    public override NodeState Evaluate()
    {
        Vector3 targetPosition = new Vector3();
        Vector3 aiPosition = ai.transform.position;
        if (target.Equals(EnemyAI.Target.Enemy))
        {
            targetPosition = ai.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
        }
        else if(target.Equals(EnemyAI.Target.Kit))
        {
            return NodeState.FAILURE;
        }

        if(targetPosition == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        float distance = Vector3.Distance(targetPosition, aiPosition);
        return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
