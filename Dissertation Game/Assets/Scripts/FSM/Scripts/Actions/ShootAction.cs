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
        if (controller.CheckIfPeriodElapsed(controller.lastShotTime, controller.enemyStats.attackRate))
        {
            controller.lastShotTime = controller.stateTimeElapsed;
            controller.enemyShooting.Shoot();
        }
    }
}
