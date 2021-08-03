using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToNode: Node
{
    private NavMeshAgent navMeshAgent;
    private EnemyStats enemyStats;
    private FieldOfView fieldOfView;
    //private int target;
    private EnemyAI ai;
    private EnemyAI.Target target;

    public GoToNode(EnemyAI ai, EnemyAI.Target target)
    {
        this.ai = ai;
        this.navMeshAgent = ai.gameObject.GetComponent<NavMeshAgent>();
        this.enemyStats = ai.enemyStats;
        this.fieldOfView = ai.fieldOfView;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        //Vector3 target = fieldOfView.lastKnownEnemyPosition;

        Vector3 targetPosition = new Vector3();
        if (target.Equals(EnemyAI.Target.Enemy))
        {
            targetPosition = ai.knownEnemiesBlackboard.GetClosestPreviousPosition(ai.transform.position);
        }
        else if (target.Equals(EnemyAI.Target.Kit))
        {
            return NodeState.FAILURE;
        }
        else if(target.Equals(EnemyAI.Target.Cover))
        {
            targetPosition = ai.GetBestCoverSpot().position;
        }


        if (targetPosition == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        float distance = Vector3.Distance(targetPosition, navMeshAgent.transform.position);
        if (distance > enemyStats.arrivalDistance)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(targetPosition);
            return NodeState.RUNNING;
        }
        else
        {
            navMeshAgent.isStopped = true;
            if (target.Equals(EnemyAI.Target.Enemy))
            {
                ai.knownEnemiesBlackboard.RemoveEnemy(targetPosition);
            }
            return NodeState.SUCCESS;
        }

        //bool lastPositionKnown = fieldOfView.lastKnownEnemyPosition != Vector3.zero;
        //return (lastPositionKnown) ? NodeState.SUCCESS : NodeState.FAILURE;
        //visibleEnemies = fieldOfView.FindVisibleEnemies(enemyStats.viewRadius, enemyStats.viewAngle, enemyStats.enemyLayer, enemyStats.coverMask, visibleEnemies, ai);
        //Debug.Log(visibleEnemies.Count);
        //return (visibleEnemies.Count != 0) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
