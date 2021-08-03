using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DashCooldownNode: Node
{
    private EnemyAI ai;
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;

    public DashCooldownNode(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyStats = ai.enemyStats;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        if(ai.timer - enemyThinker.dashEndTime > enemyStats.dashCooldown)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
