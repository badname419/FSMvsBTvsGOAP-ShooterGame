using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsDashingNode: Node
{
    private EnemyStats enemyStats;
    private EnemyAI ai;
    private Rigidbody rigidbody;
    private EnemyThinker enemyThinker;

    public IsDashingNode(EnemyAI ai)
    {
        this.ai = ai;
        this.rigidbody = ai.gameObject.GetComponent<Rigidbody>();
        this.enemyStats = ai.enemyStats;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        if (!enemyThinker.isDashing)
        {
            return NodeState.FAILURE;
        }
        else
        {
            float dashDuration = ai.timer - enemyThinker.dashStartTime;

            if (dashDuration > enemyStats.dashDuration)
            {
                ai.SetColor(Color.white);
                enemyThinker.isDashing = false;
                enemyThinker.dashEndTime = ai.timer;
                rigidbody.velocity = Vector3.zero;
                return NodeState.FAILURE;
            }
            else
            {
                rigidbody.velocity = new Vector3(ai.transform.forward.x * enemyStats.dashForce, 0f, ai.transform.forward.z * enemyStats.dashForce);
                return NodeState.RUNNING;
            }
        }
    }
}
