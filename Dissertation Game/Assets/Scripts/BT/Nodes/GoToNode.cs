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
    private EnemyAI.Target target;

    public GoToNode(EnemyThinker enemyThinker, EnemyAI.Target target)
    {
        this.enemyThinker = enemyThinker;
        this.enemyStats = enemyThinker.enemyStats;
        this.navMeshAgent = enemyThinker.navMeshAgent;
        this.target = target;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = enemyThinker.transform.position;
        Vector3 targetPosition = new Vector3();
        if (target.Equals(EnemyAI.Target.Enemy))
        {
            targetPosition = enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
        }
        else if (target.Equals(EnemyAI.Target.Kit))
        {
            targetPosition = enemyThinker.sensingSystem.DetermineClosestKit(aiPosition).position;
        }
        else if(target.Equals(EnemyAI.Target.Cover))
        {
            targetPosition = enemyThinker.GetBestCoverSpot().position;
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
        if (targetPosition == null)
        {
            targetPosition = aiPosition;
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
                enemyThinker.lastKnownEnemyLoc = enemyThinker.knownEnemiesBlackboard.GetClosestCurrentPosition(aiPosition);
                enemyThinker.aiRotatingPosition = aiPosition;
                enemyThinker.knownEnemiesBlackboard.RemoveEnemy(targetPosition);
            }
            else if (target.Equals(EnemyAI.Target.SearchPoint))
            {
                enemyThinker.currentSearchPoint++;
            }
            return NodeState.SUCCESS;
        }
    }
}
