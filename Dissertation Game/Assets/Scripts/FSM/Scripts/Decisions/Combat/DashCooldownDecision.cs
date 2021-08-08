using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/DashCooldown")]
public class DashCooldownDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool isAvailable = CheckCooldown(controller);
        return isAvailable;
    }

    private bool CheckCooldown(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        EnemyStats enemyStats = enemyThinker.enemyStats;

        return enemyThinker.timer - enemyThinker.dashEndTime > enemyStats.dashCooldown;
    }
}
