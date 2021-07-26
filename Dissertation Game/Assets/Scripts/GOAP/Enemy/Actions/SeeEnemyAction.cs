using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeEnemyAction : GOAPAction
{
    private bool requiresInRange = false;
    private bool seesPlayer = false;

    public SeeEnemyAction()
    {
        AddEffect("seeEnemy", true);
        AddEffect("knowLastEnemyLocation", true);
        cost = 1f;
    }

    public override bool CheckPrecondition(GameObject agent)
    {
        if (fieldOfView.seesEnemy)
        {
            //target = fieldOfView.lastKnownEnemyPosition;
            return true;
        }
        return false;
    }

    public override bool IsActionFinished()
    {
        return seesPlayer;
    }

    public override bool NeedsToBeInRange()
    {
        return requiresInRange;
    }

    public override bool PerformAction(GameObject agent)
    {
        seesPlayer = true;
        return true;
    }

    public override void ResetGA()
    {
        seesPlayer = false;
        target = Vector3.zero;
    }

}
