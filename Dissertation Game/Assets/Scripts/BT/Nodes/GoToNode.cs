using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToNode: Node
{
    private FieldOfView fieldOfView;
    private NavMeshAgent navMeshAgent;
    private EnemyStats enemyStats;

    public GoToNode(EnemyAI ai)
    {
        this.fieldOfView = ai.fieldOfView;
        this.navMeshAgent = ai.gameObject.GetComponent<NavMeshAgent>();
        this.enemyStats = ai.enemyStats;
    }

    public override NodeState Evaluate()
    {
        Vector3 target = fieldOfView.lastKnownEnemyPosition;

        float distance = Vector3.Distance(target, navMeshAgent.transform.position);
        if (distance > enemyStats.arrivalDistance)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target);
            return NodeState.RUNNING;
        }
        else
        {
            navMeshAgent.isStopped = true;
            fieldOfView.lastKnownEnemyPosition = Vector3.zero;
            return NodeState.SUCCESS;
        }

        //bool lastPositionKnown = fieldOfView.lastKnownEnemyPosition != Vector3.zero;
        //return (lastPositionKnown) ? NodeState.SUCCESS : NodeState.FAILURE;
        //visibleEnemies = fieldOfView.FindVisibleEnemies(enemyStats.viewRadius, enemyStats.viewAngle, enemyStats.enemyLayer, enemyStats.coverMask, visibleEnemies, ai);
        //Debug.Log(visibleEnemies.Count);
        //return (visibleEnemies.Count != 0) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
