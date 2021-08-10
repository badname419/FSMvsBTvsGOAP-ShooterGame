using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyVisibleNode : Node
{
    public List<Transform> visibleEnemies = new List<Transform>();
    private FieldOfView fieldOfView;
    private EnemyThinker enemyThinker;

    public IsEnemyVisibleNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
        this.fieldOfView = enemyThinker.fieldOfView;
    }

    public override NodeState Evaluate()
    {
        bool seesEnemy = fieldOfView.seesEnemy;
        Debug.LogWarning("sees enemy:");
        Debug.LogWarning(seesEnemy);
        if (seesEnemy)
        {
            enemyThinker.SetCombat(true);
            enemyThinker.closestEnemyObject = fieldOfView.closestEnemyObject;
        }
        return seesEnemy ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
