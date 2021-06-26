using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/ShootingDistance")]
public class ShootingDistanceDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool targetInRange = checkDistance(controller);
        return targetInRange;
    }

    private bool checkDistance(StateController controller)
    {
        float distance = Vector3.Distance(controller.closestEnemy.position, controller.gameObject.transform.position);
        return (distance <= controller.enemyStats.shootingRange);
    }
}
