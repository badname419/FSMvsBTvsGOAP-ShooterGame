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
        Vector3 target = ChooseTarget(controller);

        float distance = Vector3.Distance(target, agent.transform.position);

        if (distance > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(target);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private Vector3 ChooseTarget(StateController controller)
    {
        int index = 0;
        float shortestDistance = Mathf.Infinity;
        Vector3 position1 = controller.transform.position;
        KnownEnemiesBlackboard knownEnemies = controller.enemyThinker.knownEnemies;

        // Compare distances to the known enemies
        for(int i=0; i<knownEnemies.knownEnemiesList.Count; i++)
        {
            float distance = Vector3.Distance(position1, knownEnemies.knownEnemiesList[i].currentPosition);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                index = i;
            }
        }

        return knownEnemies.knownEnemiesList[index].currentPosition;
    }
}
