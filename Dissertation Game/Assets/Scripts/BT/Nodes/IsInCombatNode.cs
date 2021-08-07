using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsInCombatNode: Node
{
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;

    public IsInCombatNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
        this.enemyStats = enemyThinker.enemyStats;
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
