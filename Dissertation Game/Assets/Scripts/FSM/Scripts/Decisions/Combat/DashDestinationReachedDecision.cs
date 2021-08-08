using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/DashDestinationReached")]
public class DashDestinationReachedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool destinationReached = CheckIfDestinationReached(controller);
        return destinationReached;
    }

    private bool CheckIfDestinationReached(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        EnemyStats enemyStats = enemyThinker.enemyStats;

        bool destinationReached = false;
        if (enemyThinker.isDashing) {

            Rigidbody rigidbody = enemyThinker.rigidBody;
            float dashDuration = enemyThinker.timer - enemyThinker.dashStartTime;

            if (dashDuration > enemyStats.dashDuration)
            {
                enemyThinker.isDashing = false;
                enemyThinker.dashEndTime = enemyThinker.timer;
                rigidbody.velocity = Vector3.zero;
                destinationReached = true;
            }
        }

        return destinationReached;
    }
}
