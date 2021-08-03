using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DashNode: Node
{
    private EnemyStats enemyStats;
    private EnemyAI ai;
    private EnemyAI.Target target;
    private Rigidbody rigidbody;
    private KnownEnemiesBlackboard blackboard;
    private EnemyThinker enemyThinker;

    public DashNode(EnemyAI ai, EnemyAI.Target target)
    {
        this.ai = ai;
        this.rigidbody = ai.gameObject.GetComponent<Rigidbody>();
        this.enemyStats = ai.enemyStats;
        this.target = target;
        this.blackboard = ai.knownEnemiesBlackboard;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = ai.transform.position;

        if (!enemyThinker.isDashing)
        {
            Vector3 targetPosition = new Vector3();
            if (target.Equals(EnemyAI.Target.Enemy))
            {
                targetPosition = blackboard.GetClosestCurrentPosition(aiPosition);
            }
            if (targetPosition == Vector3.zero)
            {
                return NodeState.FAILURE;
            }
        }
        
        float dashDuration = 0f;
        if (enemyThinker.isDashing)
        {
            dashDuration = ai.timer - enemyThinker.dashStartTime;
        }

        if (!enemyThinker.isDashing || dashDuration < enemyStats.dashDuration)
        {
            if (!enemyThinker.isDashing)
            {
                ai.SetColor(Color.magenta);
                enemyThinker.isDashing = true;
                enemyThinker.dashStartTime = ai.timer;
            }
            else
            {
                float velocityX = rigidbody.velocity.x;
                float velocityZ = rigidbody.velocity.z;
                if (velocityX == 0 && velocityZ == 0) //If it has hit something
                {
                    ai.SetColor(Color.white);
                    enemyThinker.isDashing = false;
                    enemyThinker.dashEndTime = ai.timer;
                    return NodeState.SUCCESS;
                }
            }

            rigidbody.velocity = new Vector3(ai.transform.forward.x * enemyStats.dashForce, 0f, ai.transform.forward.z * enemyStats.dashForce);
            return NodeState.RUNNING;
        }
        else
        {
            ai.SetColor(Color.white);
            enemyThinker.isDashing = false;
            rigidbody.velocity = Vector3.zero;
            enemyThinker.dashEndTime = ai.timer;
            return NodeState.SUCCESS;
        }
        
    }
}
