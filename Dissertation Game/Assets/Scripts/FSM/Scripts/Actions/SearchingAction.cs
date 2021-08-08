using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Actions/Search")]
public class SearchingAction : Action { 
    public override void Act(StateController controller)
    {
        Search(controller);
    }

    private void Search(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        EnemyStats enemyStats = enemyThinker.enemyStats;

        if (enemyThinker.randomizedRoute.Count != 0 &&
            enemyThinker.currentSearchPoint < enemyThinker.maximumSearchPoints)
        {
            GoTowardsSearchSpot(enemyThinker, enemyStats);
        }
        else
        {
            RandomizeSearchRoute(enemyThinker);
        }
    }

    private void RandomizeSearchRoute(EnemyThinker enemyThinker)
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

        Debug.Log(randomizedResult[0]);

        for (int i = 0; i < randomizedResult.Length; i++)
        {
            enemyThinker.randomizedRoute.Add(randomizedResult[i]);
        }
    }

    private void GoTowardsSearchSpot(EnemyThinker enemyThinker, EnemyStats enemyStats)
    {
        NavMeshAgent navMeshAgent = enemyThinker.navMeshAgent;
        int currentSpot = enemyThinker.currentSearchPoint;
        Vector3 aiPosition = enemyThinker.transform.position;
        Vector3 targetPosition = enemyThinker.searchPoints[enemyThinker.randomizedRoute[currentSpot]].position;

        float distance = Vector3.Distance(targetPosition, aiPosition);

        if (distance > enemyStats.arrivalDistance)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(targetPosition);
        }
        else
        {
            navMeshAgent.isStopped = true;
            enemyThinker.currentSearchPoint++;
            if(enemyThinker.currentSearchPoint >= enemyThinker.maximumSearchPoints - 1)
            {
                RandomizeSearchRoute(enemyThinker);
                enemyThinker.currentSearchPoint = 0;
            }
        }
    }
}