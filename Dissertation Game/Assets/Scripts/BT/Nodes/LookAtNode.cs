using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookAtNode: Node
{
    private EnemyAI ai;
    private EnemyAI.Target target;
    private EnemyStats enemyStats;

    public LookAtNode(EnemyAI ai, EnemyAI.Target target)
    {
        this.ai = ai;
        this.target = target;
        this.enemyStats = ai.enemyStats;
    }

    public override NodeState Evaluate()
    {

        Vector3 targetPosition = new Vector3();
        Vector3 aiPosition = ai.transform.position;

        if (target.Equals(EnemyAI.Target.Enemy))
        {
            targetPosition = ai.knownEnemiesBlackboard.GetClosestCurrentPosition(aiPosition);
        }
        if (targetPosition == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        float dot = Vector3.Dot(ai.transform.forward, (targetPosition - aiPosition).normalized);
        if(dot > 0f)
        {
            var targetRotation = Quaternion.LookRotation(targetPosition - aiPosition);
            var str = Mathf.Min(enemyStats.rotationSpeed * Time.deltaTime, 1);
            ai.transform.rotation = Quaternion.Lerp(ai.transform.rotation, targetRotation, str);
            return NodeState.RUNNING;
        }
        else
        {
            ai.knownEnemiesBlackboard.RemoveEnemy(targetPosition);  //The spot has been visited and no enemy spoted, thus removed
            return NodeState.SUCCESS;
        }
    }
}
