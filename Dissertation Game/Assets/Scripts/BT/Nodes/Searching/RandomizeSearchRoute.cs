using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RandomizeSearchRoute : Node
{
    private EnemyThinker enemyThinker;

    public RandomizeSearchRoute(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
    }

    public override NodeState Evaluate()
    {
        int n = enemyThinker.searchPoints.Count;
        enemyThinker.randomizedRoute.Clear();

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

        for (int i = 0; i < randomizedResult.Length; i++) 
        {
            enemyThinker.randomizedRoute.Add(randomizedResult[i]);
        }

        return NodeState.SUCCESS;
    }
}
