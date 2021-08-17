using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Decisions/!CoverInRange")]
public class CoverNotInRangeDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool coverInRange = IsCoverInRange(controller);
        return !coverInRange;
    }

    private bool IsCoverInRange(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        GameObject bestCoverSpot = enemyThinker.coverSystem.FindBestCoveringSpot();
        enemyThinker.SetBestCoverSpot(bestCoverSpot.transform);
        bool valid = IsSpotValid(bestCoverSpot, enemyThinker);
        return valid;
    }

    private bool IsSpotValid(GameObject spot, EnemyThinker enemyThinker)
    {
        bool isEnemyVisible = enemyThinker.fieldOfView.seesEnemy;
        if (isEnemyVisible == false)
        {
            return true;
        }

        Vector3 spotPosition = new Vector3(spot.transform.position.x, 1f, spot.transform.position.z);
        Vector3 direction = (enemyThinker.fieldOfView.closestEnemyPosition - spotPosition).normalized;
        float distance = Vector3.Distance(spotPosition, enemyThinker.fieldOfView.closestEnemyPosition);
        if (!Physics.Raycast(spotPosition, direction, distance, enemyThinker.enemyStats.coverMask))
        {
            return false;
        }
        return true;
    }
}