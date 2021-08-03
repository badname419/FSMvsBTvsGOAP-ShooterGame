using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsInCombatNode: Node
{
    private EnemyStats enemyStats;
    private EnemyAI ai;
    private EnemyThinker enemyThinker;

    public IsInCombatNode(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyStats = ai.enemyStats;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        if (!enemyThinker.inCombat)
        {
            return NodeState.FAILURE;
        }
        else
        {
            if(enemyThinker.timer - enemyThinker.combatStartTime > enemyStats.combatDuration)
            {
                enemyThinker.SetCombat(false);
                return NodeState.FAILURE;
            }
            else
            {
                return NodeState.SUCCESS;
            }
        }
    }
}
