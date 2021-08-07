using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoverAvailableNode : Node
{
    private Transform target;
    private EnemyThinker enemyThinker;

    //Clean this up by removing the unused variables and functions.
    public IsCoverAvailableNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
    }

    public override NodeState Evaluate()
    {
        //Add check to see if the cover hasn't been taken by someone else
        GameObject bestCoverSpot = enemyThinker.coverSystem.FindBestCoveringSpot();
        enemyThinker.SetBestCoverSpot(bestCoverSpot.transform);
        bool valid = IsSpotValid(bestCoverSpot);
        return valid ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private bool IsSpotValid(GameObject spot)
    {
        bool isEnemyVisible = enemyThinker.fieldOfView.seesEnemy;
        if(isEnemyVisible == false)
        {
            return true;
        }

        RaycastHit hit;
        Vector3 spotPosition = new Vector3(spot.transform.position.x, 1f, spot.transform.position.z);
        Vector3 direction = (enemyThinker.fieldOfView.closestEnemyPosition - spotPosition).normalized;
        float distance = Vector3.Distance(spotPosition, enemyThinker.fieldOfView.closestEnemyPosition);
        if(!Physics.Raycast(spotPosition, direction, distance, enemyThinker.enemyStats.coverMask))
        {
            return false;
        }
        return true;
    }

    private bool CheckIfSpotIsValid(Transform spot)
    {
        RaycastHit hit;
        Vector3 spotPosition = new Vector3(spot.position.x, 1f, spot.position.z);
        Vector3 direction = target.position - spotPosition;
        if(Physics.Raycast(spotPosition, direction, out hit))
        {
            if(hit.collider.transform != target)
            {
                return true;
            }
        }
        return false;
    }
}
