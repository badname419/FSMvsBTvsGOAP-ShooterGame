using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DashCooldownNode: Node
{
    private EnemyAI ai;
    private EnemyStats enemyStats;

    public DashCooldownNode(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyStats = ai.enemyStats;
    }

    public override NodeState Evaluate()
    {
        if(ai.timer - ai.dashEndTime > enemyStats.dashCooldown)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
