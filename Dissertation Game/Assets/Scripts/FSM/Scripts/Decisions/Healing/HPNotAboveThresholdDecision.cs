using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/!HPAboveThreshold")]
public class HPNotAboveThresholdDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool aboveThreshold = checkHP(controller);
        return !aboveThreshold;
    }

    private bool checkHP(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        EnemyStats enemyStats = enemyThinker.enemyStats;
        KnownEnemiesBlackboard blackboard = enemyThinker.knownEnemiesBlackboard;
        Vector3 aiPosition = enemyThinker.transform.position;
        GameObject theClosestEnemy = blackboard.DetermineTheClosestEnemyObject(aiPosition);
        
        int index;
        bool hpAboveEnemy;
        if(theClosestEnemy != null)
        {
            index = blackboard.FindEnemyIndex(theClosestEnemy.transform);
            hpAboveEnemy = enemyThinker.currentHP > blackboard.knownEnemiesList[index].hp;
        }
        else
        {
            hpAboveEnemy = true;
        }

        bool hpAboveThreshold = enemyThinker.currentHP > enemyStats.maxHp * enemyStats.hpThreshold;

        if (hpAboveThreshold)
        {
            return true;
        }
        else
        {
            return hpAboveEnemy;
        }
    }
}