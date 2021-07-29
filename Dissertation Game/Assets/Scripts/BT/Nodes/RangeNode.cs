using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;
    private EnemyAI ai;

    public RangeNode(float range, Transform origin, EnemyAI ai)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Vector3 target = ai.fieldOfView.lastKnownEnemyPosition;

        float distance = Vector3.Distance(target, origin.position);
        return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
