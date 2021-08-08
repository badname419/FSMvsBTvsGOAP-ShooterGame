using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Decisions/KitInRange")]
public class KitInRangeDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;

        bool kitInRange = enemyThinker.sensingSystem.KitsInRange();
        return kitInRange;
    }
}