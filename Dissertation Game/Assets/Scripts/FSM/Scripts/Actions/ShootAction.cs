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
        EnemyThinker enemyThinker = controller.enemyThinker;
        EnemyStats enemyStats = enemyThinker.enemyStats;

        enemyThinker.shooting.Shoot(enemyStats.shootingWaitTime, enemyStats.shootingDamage, enemyThinker.transform);
    }
}
