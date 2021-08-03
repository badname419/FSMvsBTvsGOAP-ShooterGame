using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RandomizeSearchRoute : Node
{
    private EnemyAI ai;
    private EnemyThinker enemyThinker;

    public RandomizeSearchRoute(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        int n = enemyThinker.searchPoints.Count;

        var random = new System.Random();
        var randomizedResult = new int[n];
        for (var i = 0; i < n; i++)
        {
            var j = random.Next(0, i + 1);
            if (i != j)
            {
                randomizedResult[i] = randomizedResult[j];
            }
            randomizedResult[j] = i;
        }

        return NodeState.SUCCESS;
    }
}
