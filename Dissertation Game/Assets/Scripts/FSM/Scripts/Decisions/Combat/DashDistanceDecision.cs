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
        float distance = Vector3.Distance(controller.enemyThinker.closestEnemyTransform.position, controller.gameObject.transform.position);
        return (distance <= controller.enemyStats.dashRange);
    }
}
