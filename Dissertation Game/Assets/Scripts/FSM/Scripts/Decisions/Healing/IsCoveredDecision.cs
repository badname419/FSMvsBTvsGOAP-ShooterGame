using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Decisions/IsCovered")]
public class IsCoveredDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool isCovered = IsCovered(controller);
        return isCovered;
    }

    private bool IsCovered(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        Vector3 aiPosition = enemyThinker.transform.position;
        Vector3 enemyPosition = enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);

        if (enemyPosition.Equals(Vector3.zero))
        {
            return true;
        }

        Vector3 direction = (enemyPosition - aiPosition).normalized;
        float distance = Vector3.Distance(aiPosition, enemyPosition);
        if (!Physics.Raycast(aiPosition, direction, distance, enemyThinker.enemyStats.coverMask))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}