using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthThresholdNode : Node
{
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;

    public HealthThresholdNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
        this.enemyStats = enemyThinker.enemyStats;
    }

    public override NodeState Evaluate()
    {
        return enemyThinker.currentHP <= enemyStats.maxHp * enemyStats.hpThreshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
