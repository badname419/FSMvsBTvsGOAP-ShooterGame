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
    private KnownEnemiesBlackboard knownEnemiesBlackboard;

    public LookAroundNode(EnemyAI ai, EnemyAI.Target target)
    {
        this.ai = ai;
        this.target = target;
        this.enemyStats = ai.enemyStats;
        this.enemyThinker = ai.enemyThinker;
        this.knownEnemiesBlackboard = ai.knownEnemiesBlackboard;
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

    private Vector3[] FindRotationTargets(Vector3 currentPosition, float radius)
    {
        Vector3[] targetArray = new Vector3[enemyThinker.totalRotations];
        Vector3 lastKnownPosition = knownEnemiesBlackboard.GetClosestPreviousPosition(ai.transform.position);
         Vector3 forwardVector = (lastKnownPosition - currentPosition).normalized;
        forwardVector.y = 0;
        float rotationAngle = enemyStats.rotationAngle;

        enemyThinker.forwardRotationTarget = currentPosition + forwardVector * radius;

        Vector3 rightVector = Quaternion.Euler(0, rotationAngle, 0) * forwardVector;
        Vector3 leftVector = Quaternion.Euler(0, 360 - rotationAngle, 0) * forwardVector;

        enemyThinker.rightRotationTarget = currentPosition + rightVector * radius;
        enemyThinker.leftRotationTarget = currentPosition + leftVector * radius;

        targetArray[0] = targetArray[2] = targetArray[4] = enemyThinker.forwardRotationTarget;
        targetArray[1] = enemyThinker.leftRotationTarget;
        targetArray[3] = enemyThinker.rightRotationTarget;
        return targetArray;
    }
}
