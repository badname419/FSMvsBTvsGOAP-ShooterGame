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
        KnownEnemiesBlackboard knownEnemies = controller.enemyThinker.knownEnemies;
        if(knownEnemies.knownEnemiesList.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}