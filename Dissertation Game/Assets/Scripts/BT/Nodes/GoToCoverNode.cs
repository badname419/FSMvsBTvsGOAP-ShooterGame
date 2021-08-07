using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToCoverNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI ai;
    private EnemyThinker enemyThinker;

    public GoToCoverNode( NavMeshAgent agent, EnemyAI ai)
    {
        this.agent = agent;
        this.ai = ai;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        Transform coverSpot = enemyThinker.GetBestCoverSpot();
        if(coverSpot == null)
        {
            return NodeState.FAILURE;
        }

        ai.SetColor(Color.yellow);
        float distance = Vector3.Distance(coverSpot.position, agent.transform.position);
        if(distance > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(coverSpot.position);
            return NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }
}
