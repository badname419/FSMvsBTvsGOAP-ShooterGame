using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Decisions/KnownEnemies")]
public class KnownEnemiesDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool enemiesKnown = CheckIfEnemiesKnown(controller);
        return enemiesKnown;
    }

    private bool CheckIfEnemiesKnown(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        Vector3 aiPosition = enemyThinker.transform.position;
        int targetIndex = enemyThinker.knownEnemiesBlackboard.DetermineTheClosestEnemyIndex(aiPosition);

        return targetIndex != -1;
    }
}