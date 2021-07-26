using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachEnemyAction : GOAPAction
{
    private bool requiresInRange = true;
    private bool inShootingRange = false;

    public ApproachEnemyAction()
    { 
        AddPrecondition("seeEnemy", true);
        AddEffect("attackPlayer", true);
        cost = 1f;
    }

    public override bool CheckPrecondition(GameObject agent)
    {
        Debug.Log("Check");
        target = fieldOfView.lastKnownEnemyPosition;

        if(target != transform.position)
        {
            return true;
        }
        return false;
    }

    public override bool IsActionFinished()
    {
        Debug.Log("IsFinished");
        float distToEnemy = Vector3.Distance(transform.position, target);

        if(distToEnemy < enemyThinker.enemyStats.shootingRange)
        {
            inShootingRange = true;
        }

        return inShootingRange;
    }

    public override bool NeedsToBeInRange()
    {
        Debug.Log("IsFinished");
        float distToEnemy = Vector3.Distance(transform.position, target);

        if (distToEnemy < enemyThinker.enemyStats.shootingRange)
        {
            inShootingRange = true;
            requiresInRange = false;
        }
        return requiresInRange;
    }

    public override bool PerformAction(GameObject agent)
    {
        Debug.Log("Perform Action");
        inShootingRange = true;
        return true;
    }

    public override void ResetGA()
    {
        inShootingRange = false;
        target = Vector3.zero;
    }
}
