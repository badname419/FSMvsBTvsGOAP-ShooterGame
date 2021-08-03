using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyVisibleNode : Node
{
    public List<Transform> visibleEnemies = new List<Transform>();

    private FieldOfView fieldOfView;
    private EnemyAI ai;
    private EnemyThinker enemyThinker;

    public IsEnemyVisibleNode(EnemyAI ai)
    {
        this.ai = ai;
        fieldOfView = ai.fieldOfView;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        if (fieldOfView.seesEnemy)
        {
            enemyThinker.SetCombat(true);
            ai.closestEnemy = fieldOfView.closestEnemyObject;
        }
        return (fieldOfView.seesEnemy) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
