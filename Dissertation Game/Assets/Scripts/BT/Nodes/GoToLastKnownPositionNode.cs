using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToLastKnownPositionNode : Node
{
    private NavMeshAgent navMeshAgent;
    private FieldOfView fieldOfView;
    private EnemyStats enemyStats;
    private EnemyAI ai;

    public GoToLastKnownPositionNode(NavMeshAgent agent, EnemyAI ai)
    {
        this.ai = ai;
        this.fieldOfView = ai.fieldOfView;
        this.navMeshAgent = ai.gameObject.GetComponent<NavMeshAgent>();
        this.enemyStats = ai.enemyStats;
    }

    public override NodeState Evaluate()
    {
        Vector3 target = fieldOfView.lastKnownEnemyPosition;

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
            fieldOfView.lastKnownEnemyPosition = Vector3.zero;
            return NodeState.SUCCESS;
        }
    }
}
