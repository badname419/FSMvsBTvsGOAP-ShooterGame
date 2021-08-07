using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Decisions/ReachedDestination")]
public class ReachedDestinationDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool destinationReached = CheckIfDestinationReached(controller);
        return destinationReached;
    }

    private bool CheckIfDestinationReached(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        EnemyStats enemyStats = controller.enemyStats;

        float distance = Vector3.Distance(enemyThinker.walkingTarget, controller.transform.position);

        if (distance < enemyStats.arrivalDistance)
        {
            //Remove the enemy from the knownEnemiesBlackboard,
            //The enemy's current location isn't known anymore
            if (controller.walkingTargetEnum.Equals(StateController.Target.Enemy))
            {
                enemyThinker.knownEnemiesBlackboard.RemoveEnemy(enemyThinker.walkingTarget);
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}