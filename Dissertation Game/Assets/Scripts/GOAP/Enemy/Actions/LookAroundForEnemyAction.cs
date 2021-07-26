using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundForEnemyAction : GOAPAction
{
    private bool requiresInRange = false;
    private bool lookedAround = false;

    public LookAroundForEnemyAction()
    {
        AddPrecondition("atTheInvestigationSpot", true);
        AddPrecondition("seeEnemy", false);
        AddEffect("seeEnemy", true);
        cost = 1f;
    }

    public override bool CheckPrecondition(GameObject agent)
    {
        if(fieldOfView.seesEnemy == false)
        {
            return true;
        }
        return false;
    }

    public override bool IsActionFinished()
    {
        return lookedAround;
    }

    public override bool NeedsToBeInRange()
    {
        return requiresInRange;
    }

    public override bool PerformAction(GameObject agent)
    {
        Debug.Log("Rotate");
        return true;
    }

    public override void ResetGA()
    {
        lookedAround = false;
    }
}
