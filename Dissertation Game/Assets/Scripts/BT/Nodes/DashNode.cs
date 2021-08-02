using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DashNode: Node
{
    private NavMeshAgent navMeshAgent;
    private EnemyStats enemyStats;
    private FieldOfView fieldOfView;
    //private int target;
    private EnemyAI ai;
    private EnemyAI.Target target;

    private Rigidbody rigidbody;

    public DashNode(EnemyAI ai, EnemyAI.Target target)
    {
        this.ai = ai;
        this.rigidbody = ai.gameObject.GetComponent<Rigidbody>();

        this.navMeshAgent = ai.gameObject.GetComponent<NavMeshAgent>();
        this.enemyStats = ai.enemyStats;
        this.fieldOfView = ai.fieldOfView;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        //Vector3 target = fieldOfView.lastKnownEnemyPosition;

        Vector3 targetPosition = new Vector3();
        if (target.Equals(EnemyAI.Target.Enemy))
        {
            targetPosition = ai.fieldOfView.lastKnownEnemyPosition;
        }
        if (targetPosition == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        float dashDuration = 0f;
        if (ai.isDashing)
        {
            dashDuration = ai.timer - ai.dashStartTime;
        }

        if(!ai.isDashing || dashDuration < enemyStats.dashDuration)
        {
            if (!ai.isDashing)
            {
                ai.isDashing = true;
                ai.dashStartTime = ai.timer;
            }
            else
            {
                float velocityX = rigidbody.velocity.x;
                float velocityZ = rigidbody.velocity.z;
                if(velocityX == 0 && velocityZ == 0) //If it has hit something
                {
                    ai.isDashing = false;
                    ai.dashEndTime = ai.timer;
                    return NodeState.SUCCESS;
                }
            }

            rigidbody.velocity = new Vector3(ai.transform.forward.x * enemyStats.dashForce, 0f, ai.transform.forward.z * enemyStats.dashForce);
            return NodeState.RUNNING;
        }
        else
        {
            ai.isDashing = false;
            rigidbody.velocity = Vector3.zero;
            ai.dashEndTime = ai.timer;
            return NodeState.SUCCESS;
        }

        //bool lastPositionKnown = fieldOfView.lastKnownEnemyPosition != Vector3.zero;
        //return (lastPositionKnown) ? NodeState.SUCCESS : NodeState.FAILURE;
        //visibleEnemies = fieldOfView.FindVisibleEnemies(enemyStats.viewRadius, enemyStats.viewAngle, enemyStats.enemyLayer, enemyStats.coverMask, visibleEnemies, ai);
        //Debug.Log(visibleEnemies.Count);
        //return (visibleEnemies.Count != 0) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
