using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookAroundNode: Node
{
    private EnemyAI ai;
    private EnemyAI.Target target;
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;
    private KnownEnemiesBlackboard blackboard;

    public LookAroundNode(EnemyAI ai, EnemyAI.Target target)
    {
        this.ai = ai;
        this.target = target;
        this.enemyStats = enemyThinker.enemyStats;
        this.enemyThinker = ai.enemyThinker;
        this.blackboard = enemyThinker.knownEnemiesBlackboard;
    }

    public override NodeState Evaluate()
    {

        if (!enemyThinker.isDashing)
        {
            Vector3 targetPosition = new Vector3();
            Vector3 aiPosition = ai.transform.position;

            if (target.Equals(EnemyAI.Target.Enemy))
            {
                targetPosition = blackboard.GetClosestCurrentPosition(aiPosition);
            }
            if (targetPosition == Vector3.zero)
            {
                return NodeState.FAILURE;
            }

            Vector3 targetDir = targetPosition - aiPosition;
            float angle = Vector3.Angle(targetDir, ai.transform.forward);

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
