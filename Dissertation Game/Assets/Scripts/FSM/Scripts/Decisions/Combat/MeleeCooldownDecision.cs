using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/MeleeCooldown")]
public class MeleeCooldownDecision : Decision
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

        bool result = enemyThinker.timer - enemyThinker.meleeAttackTime > enemyStats.meleeWaitTime;
        return result;
    }
}
