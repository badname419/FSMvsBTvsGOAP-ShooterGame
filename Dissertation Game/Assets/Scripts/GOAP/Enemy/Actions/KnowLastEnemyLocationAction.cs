using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowLastEnemyLocationAction : GOAPAction
{
    private bool requiresInRange = false;
    private bool knowsLastLocation = false;

    public KnowLastEnemyLocationAction()
    {
        AddPrecondition("seeEnemy", false);
        AddEffect("knowLastEnemyLocation", true);
        cost = 1f;
    }
    public override bool CheckPrecondition(GameObject agent)
    {
        if(fieldOfView.lastKnownEnemyPosition != Vector3.zero)
        {
            return true;
        }
        return false;
    }

    public override bool IsActionFinished()
    {
        return knowsLastLocation;
    }

    public override bool NeedsToBeInRange()
    {
        return requiresInRange;
    }

    public override bool PerformAction(GameObject agent)
    {
        knowsLastLocation = true;
        return true;
    }

    public override void ResetGA()
    {
        knowsLastLocation = false;
        target = Vector3.zero;
    }
}
