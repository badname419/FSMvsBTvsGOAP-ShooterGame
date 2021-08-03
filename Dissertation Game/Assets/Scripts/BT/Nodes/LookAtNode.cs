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
    private EnemyThinker enemyThinker;

    public LookAtNode(EnemyAI ai, EnemyAI.Target target)
    {
        this.ai = ai;
        this.target = target;
        this.enemyStats = ai.enemyStats;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {

        if (!enemyThinker.isDashing)
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

            Vector3 targetDir = targetPosition - aiPosition;
            float angle = Vector3.Angle(targetDir, ai.transform.forward);
            //Debug.Log(angle);

            if (angle > enemyStats.minimumLookingAngle)
            {
                var targetRotation = Quaternion.LookRotation(targetPosition - aiPosition);
                var str = Mathf.Min(enemyStats.rotationSpeed * Time.deltaTime, 1);
                ai.transform.rotation = Quaternion.Lerp(ai.transform.rotation, targetRotation, str);
                return NodeState.RUNNING;
            }
            else
            {
                //ai.knownEnemiesBlackboard.RemoveEnemy(targetPosition);  //The spot has been visited and no enemy spoted, thus removed
                return NodeState.SUCCESS;
            }
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }
}
