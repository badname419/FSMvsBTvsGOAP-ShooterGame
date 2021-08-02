using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Shoot")]
public class ShootAction : Action
{
    public override void Act(StateController controller)
    {
        Shoot(controller);
    }

    private void Shoot(StateController controller)
    {
        Debug.LogWarning(controller.enemyThinker.lastShotTime);
        Debug.LogWarning(controller.enemyThinker.stateTimeElapsed);
        Debug.LogWarning(controller.enemyStats.attackRate);
        if (controller.CheckIfPeriodElapsed(controller.enemyThinker.lastShotTime, controller.enemyStats.attackRate))
        {
            Debug.LogWarning("Shoot 2");
            controller.enemyThinker.lastShotTime = controller.enemyThinker.stateTimeElapsed;
            controller.enemyShooting.Shoot(controller.enemyStats.shootingWaitTime, controller.enemyStats.shootingDamage, controller.transform);
        }
    }
}
