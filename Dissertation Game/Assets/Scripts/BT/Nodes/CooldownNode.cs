using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CooldownNode: Node
{
    private EnemyThinker enemyThinker;
    private EnemyAI.EventType eventType;
    private EnemyStats enemyStats;
    private float eventTime;
    private float eventCooldown;

    public CooldownNode(EnemyAI ai, EnemyAI.EventType eventType)
    {
        this.enemyThinker = ai.enemyThinker;
        this.eventType = eventType;
        this.enemyStats = ai.enemyStats;
        eventTime = 0f;
        eventCooldown = 0f;
    }

    public override NodeState Evaluate()
    {
        if (eventType.Equals(EnemyAI.EventType.Dash))
        {
            eventTime = enemyThinker.dashEndTime;
            eventCooldown = enemyStats.dashCooldown;
        }
        else if (eventType.Equals(EnemyAI.EventType.Melee))
        {
            eventTime = enemyThinker.meleeAttackTime;
            eventCooldown = enemyStats.meleeWaitTime;
        }

        if(enemyThinker.timer - eventTime > eventCooldown)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
