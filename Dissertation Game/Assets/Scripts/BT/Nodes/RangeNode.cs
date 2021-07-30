using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    private float range;
    private int target;
    private EnemyAI ai;

    public RangeNode(float range, EnemyAI ai, int target)
    {
        this.range = range;
        this.target = target;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Vector3 targetPosition = new Vector3();
        if (target == (int)EnemyAI.Target.Enemy)
        {
            targetPosition = ai.fieldOfView.lastKnownEnemyPosition;
        }
        else if(target == (int)EnemyAI.Target.Kit)
        {
            targetPosition = Vector3.zero;
        }

        if(targetPosition == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        float distance = Vector3.Distance(targetPosition, ai.transform.position);
        return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
