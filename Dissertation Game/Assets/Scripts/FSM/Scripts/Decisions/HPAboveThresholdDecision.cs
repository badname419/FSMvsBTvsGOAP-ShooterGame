using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/HPAboveThreshold")]
public class HPAboveThresholdDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool aboveThreshold = checkHP(controller);
        return aboveThreshold;
    }

    private bool checkHP(StateController controller)
    {
        return (controller.enemyStats.hpThreshold <= controller.currentHP);
    }
}