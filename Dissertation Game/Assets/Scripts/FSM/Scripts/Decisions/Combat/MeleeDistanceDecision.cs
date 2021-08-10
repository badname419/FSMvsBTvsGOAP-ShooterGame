using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/MeleeDistance")]
public class MeleeDistanceDecision : Decision
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
        return (distance <= controller.enemyStats.meleeRange);
    }
}
