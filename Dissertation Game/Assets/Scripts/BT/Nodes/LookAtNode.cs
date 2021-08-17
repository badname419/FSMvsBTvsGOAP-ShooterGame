using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookAtNode: Node
{
    private EnemyAI.Target target;
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;
    private float rotationSpeed;

    public LookAtNode(EnemyThinker enemyThinker, EnemyAI.Target target)
    {
        this.target = target;
        this.enemyThinker = enemyThinker;
        this.enemyStats = enemyThinker.enemyStats;

        if (target.Equals(EnemyAI.Target.Around))
        {
            this.rotationSpeed = enemyStats.rotationSpeed / 2;
        }
        else
        {
            this.rotationSpeed = enemyStats.rotationSpeed;
        }
    }

    public override NodeState Evaluate()
    {

        if (!enemyThinker.isDashing)
        {
            Vector3 targetPosition = new Vector3();
            Vector3 aiPosition = enemyThinker.transform.position;
            Transform aiTransform = enemyThinker.transform;


            if (target.Equals(EnemyAI.Target.Enemy))
            {
                targetPosition = enemyThinker.fieldOfView.closestEnemyPosition;
            }
            else if (target.Equals(EnemyAI.Target.Around))
            {
                targetPosition = enemyThinker.targetArray[enemyThinker.numOfRotations];
            }
            if (targetPosition == Vector3.zero)
            {
                return NodeState.FAILURE;
            }


            Vector3 targetDir = targetPosition - aiPosition;
            float angle = Vector3.Angle(targetDir, aiTransform.forward); 

            if (angle > enemyStats.minimumLookingAngle)
            {
                var targetRotation = Quaternion.LookRotation(targetPosition - aiPosition);
                var str = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                enemyThinker.transform.rotation = Quaternion.Lerp(aiTransform.rotation, targetRotation, str);
                return NodeState.RUNNING;
            }
            else
            {
                if (target.Equals(EnemyAI.Target.Around))
                {
                    if(enemyThinker.numOfRotations < enemyThinker.totalRotations - 1)
                    {
                        enemyThinker.numOfRotations++;
                        return NodeState.RUNNING;
                    }
                    else
                    {
                        enemyThinker.aiRotatingPosition = Vector3.zero;
                        enemyThinker.knownEnemiesBlackboard.RemoveEnemy(targetPosition);
                    }
                }
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
