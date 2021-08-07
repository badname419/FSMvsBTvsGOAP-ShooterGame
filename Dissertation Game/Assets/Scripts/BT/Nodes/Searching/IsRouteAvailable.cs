using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRouteAvailable : Node
{
    private EnemyThinker enemyThinker;

    public IsRouteAvailable(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
    }

    public override NodeState Evaluate()
    {
        if(enemyThinker.randomizedRoute.Count != 0 && 
            enemyThinker.currentSearchPoint < enemyThinker.maximumSearchPoints)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
