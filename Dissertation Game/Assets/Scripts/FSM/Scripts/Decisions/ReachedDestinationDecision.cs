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
        float distance = Vector3.Distance(controller.enemyThinker.walkingTarget, controller.transform.position);

        if (distance < controller.targetProximityThreshold)
        {
            //Remove the enemy from the knownEnemiesBlackboard,
            //The enemy's current location isn't known anymore
            KnownEnemiesBlackboard knownEnemies = controller.enemyThinker.knownEnemies;
            controller.enemyThinker.lastKnownEnemyLoc = knownEnemies.knownEnemiesList[controller.enemyThinker.closestEnemyIndex].currentPosition;
            controller.enemyThinker.knownEnemies.RemoveAtIndex(controller.enemyThinker.closestEnemyIndex);
            return true;
        }
        else
        {
            return false;
        }
    }
}