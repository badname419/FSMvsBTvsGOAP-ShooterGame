using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    private float range;
    private EnemyAI.Target target;
    private EnemyThinker enemyThinker;

    public RangeNode(EnemyThinker enemyThinker, float range, EnemyAI.Target target)
    {
        this.range = range;
        this.target = target;
        this.enemyThinker = enemyThinker;
    }

    public RangeNode(EnemyThinker enemyThinker, EnemyAI.Target target)
    {
        this.enemyThinker = enemyThinker;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        Vector3 targetPosition = new Vector3();
        Vector3 aiPosition = enemyThinker.transform.position;
        if (target.Equals(EnemyAI.Target.Enemy))
        {
            targetPosition = enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
        }
        else if(target.Equals(EnemyAI.Target.Kit))
        {
            return enemyThinker.sensingSystem.KitsInRange() ? NodeState.SUCCESS : NodeState.FAILURE;
        }

        if(targetPosition == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        float distance = Vector3.Distance(targetPosition, aiPosition);
        return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
