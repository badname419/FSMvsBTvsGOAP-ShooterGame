using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthThresholdNode : Node
{
    private EnemyAI ai;
    private float threshold;
    private EnemyStats enemyStats;

    public HealthThresholdNode(EnemyAI ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;
        this.enemyStats = ai.enemyStats;
    }

    public override NodeState Evaluate()
    {
        return ai.enemyThinker.currentHP <= enemyStats.maxHp * enemyStats.hpThreshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
