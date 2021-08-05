using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToNode: Node
{
    private NavMeshAgent navMeshAgent;
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;
    private KnownEnemiesBlackboard knownEnemiesBlackboard;
    private EnemyAI ai;
    private EnemyAI.Target target;

    public GoToNode(EnemyAI ai, EnemyAI.Target target)
    {
        this.ai = ai;
        this.navMeshAgent = ai.gameObject.GetComponent<NavMeshAgent>();
        this.enemyStats = ai.enemyStats;
        this.enemyThinker = ai.enemyThinker;
        this.knownEnemiesBlackboard = ai.knownEnemiesBlackboard;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        //Vector3 target = fieldOfView.lastKnownEnemyPosition;
        Vector3 aiPosition = ai.transform.position;
        Vector3 targetPosition = new Vector3();
        if (target.Equals(EnemyAI.Target.Enemy))
        {
            targetPosition = ai.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
        }
        else if (target.Equals(EnemyAI.Target.Kit))
        {
            return NodeState.FAILURE;
        }
        else if(target.Equals(EnemyAI.Target.Cover))
        {
            targetPosition = ai.GetBestCoverSpot().position;
        }
        else if (target.Equals(EnemyAI.Target.SearchPoint))
        {
            int currentSpot = enemyThinker.currentSearchPoint;
            targetPosition = enemyThinker.searchPoints[enemyThinker.randomizedRoute[currentSpot]].position;
        }


        if (targetPosition == Vector3.zero)
        {
            navMeshAgent.isStopped = true;
            return NodeState.FAILURE;
        }

        float distance = Vector3.Distance(targetPosition, aiPosition);
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
                enemyThinker.lastKnownEnemyLoc = knownEnemiesBlackboard.GetClosestCurrentPosition(aiPosition);
                enemyThinker.aiRotatingPosition = aiPosition;
                ai.knownEnemiesBlackboard.RemoveEnemy(targetPosition);
            }
            else if (target.Equals(EnemyAI.Target.SearchPoint))
            {
                enemyThinker.currentSearchPoint++;
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
