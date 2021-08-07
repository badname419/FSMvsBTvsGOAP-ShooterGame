using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToLastKnownPositionNode : Node
{
    private NavMeshAgent navMeshAgent;
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;

    public GoToLastKnownPositionNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
        this.navMeshAgent = enemyThinker.navMeshAgent;
        this.enemyStats = enemyThinker.enemyStats;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = enemyThinker.transform.position;
        Vector3 target = enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);

        if (target.Equals(Vector3.zero))
        {
            navMeshAgent.isStopped = true;
            return NodeState.FAILURE;
        }

        float distance = Vector3.Distance(target, navMeshAgent.transform.position);
        if(distance > enemyStats.arrivalDistance)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target);
            return NodeState.RUNNING;
        }
        else
        {
            enemyThinker.knownEnemiesBlackboard.RemoveEnemy(target);
            navMeshAgent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }
}
