using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoverAvailableNode : Node
{
    private Cover[] availableCovers;
    private Transform target;
    private EnemyAI ai;

    //Clean this up by removing the unused variables and functions.
    public IsCoverAvailableNode(EnemyAI ai)
    {
        //this.availableCovers = availableCovers;
        //this.target = target;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        //Add check to see if the cover hasn't been taken by someone else
        GameObject bestCoverSpot = ai.coverSystem.FindBestCover();
        ai.SetBestCoverSpot(bestCoverSpot.transform);
        return IsSpotValid(bestCoverSpot) ? NodeState.SUCCESS : NodeState.FAILURE;

        /*
        Transform bestSpot = FindBestCoverSpot();
        ai.SetBestCoverSpot(bestSpot);
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;*/
    }

    private Transform FindBestCoverSpot()
    {
        if(ai.GetBestCoverSpot() != null)
        {
            if (CheckIfSpotIsValid(ai.GetBestCoverSpot()))
            {
                return ai.GetBestCoverSpot();
            }
        }

        float minAngle = 90;
        Transform bestSpot = null;
        for(int i=0; i <availableCovers.Length; i++)
        {
            Transform bestSpotInCover = FindBestSpotInCover(availableCovers[i], ref minAngle);
            if(bestSpotInCover != null)
            {
                bestSpot = bestSpotInCover;
            }
        }
        return bestSpot;
    }

    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        Transform[] availableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for(int i = 0; i < availableSpots.Length; i++)
        {
            Vector3 direction = target.position - availableSpots[i].position;
            if (CheckIfSpotIsValid(availableSpots[i]))
            {
                float angle = Vector3.Angle(availableSpots[i].forward, direction);
                if(angle < minAngle)
                {
                    minAngle = angle;
                    bestSpot = availableSpots[i];
                }
            }
        }
        return bestSpot;
    }

    private bool IsSpotValid(GameObject spot)
    {
        bool isEnemyVisible = ai.fieldOfView.seesEnemy;
        if(isEnemyVisible == false)
        {
            return true;
        }

        RaycastHit hit;
        Vector3 direction = (ai.fieldOfView.closestEnemyPosition - spot.transform.position).normalized;
        float distance = Vector3.Distance(spot.transform.position, ai.fieldOfView.closestEnemyPosition);
        if(!Physics.Raycast(spot.transform.position, direction, distance, ai.enemyStats.coverMask))
        {
            return false;
        }
        return true;
    }

    private bool CheckIfSpotIsValid(Transform spot)
    {
        RaycastHit hit;
        Vector3 direction = target.position - spot.position;
        if(Physics.Raycast(spot.position, direction, out hit))
        {
            if(hit.collider.transform != target)
            {
                return true;
            }
        }
        return false;
    }
}
