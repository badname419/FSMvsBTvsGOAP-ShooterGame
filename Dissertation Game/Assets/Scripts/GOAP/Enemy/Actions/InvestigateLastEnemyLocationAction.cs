using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateLastEnemyLocationAction : GOAPAction
{
    private bool requiresInRange = true;
    private bool attheInvestigationSpot = false;

    public InvestigateLastEnemyLocationAction()
    {
        AddPrecondition("knowLastEnemyLocation", true);
        AddPrecondition("seeEnemy", false);
        AddEffect("atTheInvestigationSpot", true);
    }
    public override bool CheckPrecondition(GameObject agent)
    {
        if(fieldOfView.lastKnownEnemyPosition != Vector3.zero &&
            fieldOfView.seesEnemy == false)
        {
            target = fieldOfView.lastKnownEnemyPosition;
            return true;
        }
        return false;
    }

    public override bool IsActionFinished()
    {
        return attheInvestigationSpot;
    }

    public override bool NeedsToBeInRange()
    {
        return requiresInRange;
    }

    public override bool PerformAction(GameObject agent)
    {
        attheInvestigationSpot = true;
        fieldOfView.lastKnownEnemyPosition = Vector3.zero;
        return true;
    }

    public override void ResetGA()
    {
        attheInvestigationSpot = false;
        target = Vector3.zero;
    }
}
