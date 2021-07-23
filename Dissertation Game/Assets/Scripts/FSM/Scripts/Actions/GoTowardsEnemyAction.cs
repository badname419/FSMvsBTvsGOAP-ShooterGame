using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Actions/GoTowardsEnemy")]
public class GoTowardsEnemyAction : Action
{
    public override void Act(StateController controller)
    {
        NavMeshAgent agent = controller.navMeshAgent;

        int closestEnemyIndex = ChooseTarget(controller);
        controller.closestEnemyIndex = closestEnemyIndex;
        controller.walkingTarget = controller.enemyThinker.knownEnemies.knownEnemiesList[closestEnemyIndex].previousPosition;

        agent.isStopped = false;
        agent.SetDestination(controller.walkingTarget);
    }

    private int ChooseTarget(StateController controller)
    {

        int index = 0;
        float shortestDistance = Mathf.Infinity;
        Vector3 position1 = controller.transform.position;
        KnownEnemiesBlackboard knownEnemies = controller.enemyThinker.knownEnemies;

        // Compare distances to the known enemies
        for(int i=0; i<knownEnemies.knownEnemiesList.Count; i++)
        {
            float distance = Vector3.Distance(position1, knownEnemies.knownEnemiesList[i].previousPosition);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                index = i;
            }
        }

        return index;
    }
}
