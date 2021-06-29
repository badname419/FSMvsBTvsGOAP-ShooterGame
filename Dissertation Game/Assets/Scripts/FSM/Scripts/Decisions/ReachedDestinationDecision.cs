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
        float distance = Vector3.Distance(controller.walkingTarget, controller.transform.position);

        if (distance < controller.targetProximityThreshold)
        {
            //Remove the enemy from the knownEnemiesBlackboard,
            //The enemy's current location isn't known anymore
            KnownEnemiesBlackboard knownEnemies = controller.enemyThinker.knownEnemies;
            controller.lastKnownEnemyLoc = knownEnemies.knownEnemiesList[controller.closestEnemyIndex].currentPosition;
            controller.enemyThinker.knownEnemies.RemoveAtIndex(controller.closestEnemyIndex);
            return true;
        }
        else
        {
            return false;
        }
    }
}