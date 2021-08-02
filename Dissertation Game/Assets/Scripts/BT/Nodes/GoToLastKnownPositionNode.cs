using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToLastKnownPositionNode : Node
{
    private NavMeshAgent navMeshAgent;
    private EnemyStats enemyStats;
    private EnemyAI ai;
    private KnownEnemiesBlackboard blackboard;

    public GoToLastKnownPositionNode(EnemyAI ai)
    {
        this.ai = ai;
        this.navMeshAgent = ai.gameObject.GetComponent<NavMeshAgent>();
        this.enemyStats = ai.enemyStats;
        this.blackboard = ai.knownEnemiesBlackboard;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = ai.transform.position;
        Vector3 target = ai.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);
        if (target.Equals(Vector3.zero))
        {
            navMeshAgent.isStopped = true;
            return NodeState.FAILURE;
        }

        ai.SetColor(Color.yellow);
        float distance = Vector3.Distance(target, navMeshAgent.transform.position);
        if(distance > enemyStats.arrivalDistance)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target);
            return NodeState.RUNNING;
        }
        else
        {
            navMeshAgent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }
}
