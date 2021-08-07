using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsDashingNode: Node
{
    private EnemyStats enemyStats;
    private Rigidbody rigidbody;
    private EnemyThinker enemyThinker;

    public IsDashingNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
        this.rigidbody = enemyThinker.gameObject.GetComponent<Rigidbody>();
        this.enemyStats = enemyThinker.enemyStats;
    }

    public override NodeState Evaluate()
    {
        if (!enemyThinker.isDashing)
        {
            return NodeState.FAILURE;
        }
        else
        {
            float dashDuration = enemyThinker.timer - enemyThinker.dashStartTime;

            if (dashDuration > enemyStats.dashDuration)
            {
                enemyThinker.isDashing = false;
                enemyThinker.dashEndTime = enemyThinker.timer;
                rigidbody.velocity = Vector3.zero;
                return NodeState.FAILURE;
            }
            else
            {
                rigidbody.velocity = new Vector3(enemyThinker.transform.forward.x * enemyStats.dashForce, 0f, enemyThinker.transform.forward.z * enemyStats.dashForce);
                return NodeState.RUNNING;
            }
        }
    }
}
