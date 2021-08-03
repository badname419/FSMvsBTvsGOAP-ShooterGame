using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsRouteAvailable : Node
{
    private EnemyAI ai;
    private EnemyThinker enemyThinker;

    public IsRouteAvailable(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyThinker = ai.enemyThinker;
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
