using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsInCombatNode: Node
{
    private EnemyStats enemyStats;
    private EnemyAI ai;

    public IsInCombatNode(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyStats = ai.enemyStats;
    }

    public override NodeState Evaluate()
    {
        if (!ai.inCombat)
        {
            return NodeState.FAILURE;
        }
        else
        {
            if(ai.timer - ai.combatStartTime > enemyStats.combatDuration)
            {
                ai.SetCombat(false);
                return NodeState.FAILURE;
            }
            else
            {
                return NodeState.SUCCESS;
            }
        }
    }
}
