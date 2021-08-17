using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/DashDistance")]
public class DashDistanceDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool targetInRange = CheckDistance(controller);
        return targetInRange;
    }

    private bool CheckDistance(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        Vector3 aiPosition = enemyThinker.transform.position;
        Vector3 targetPosition = enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
        float distance = Vector3.Distance(targetPosition, aiPosition);

        bool result = (distance <= controller.enemyStats.dashRange);
        return result;
    }
}
