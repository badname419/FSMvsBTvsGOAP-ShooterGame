using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyVisibleNode : Node
{
    public List<Transform> visibleEnemies = new List<Transform>();

    private FieldOfView fieldOfView;
    private EnemyAI ai;

    public IsEnemyVisibleNode(EnemyAI ai)
    {
        this.ai = ai;
        fieldOfView = ai.fieldOfView;
    }

    public override NodeState Evaluate()
    {
        //visibleEnemies = fieldOfView.GetVisibleEnemyTransforms();
        Debug.Log(fieldOfView.seesEnemy);
        return (fieldOfView.seesEnemy) ? NodeState.SUCCESS : NodeState.FAILURE;
        //visibleEnemies = fieldOfView.FindVisibleEnemies(enemyStats.viewRadius, enemyStats.viewAngle, enemyStats.enemyLayer, enemyStats.coverMask, visibleEnemies, ai);
        //Debug.Log(visibleEnemies.Count);
        //return (visibleEnemies.Count != 0) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
