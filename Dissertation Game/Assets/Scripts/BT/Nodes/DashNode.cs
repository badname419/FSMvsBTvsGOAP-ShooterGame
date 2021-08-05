using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DashNode: Node
{
    private EnemyStats enemyStats;
    private EnemyAI ai;
    private EnemyAI.Target target;
    private Rigidbody rigidbody;
    private KnownEnemiesBlackboard blackboard;
    private EnemyThinker enemyThinker;

    public DashNode(EnemyAI ai, EnemyAI.Target target)
    {
        this.ai = ai;
        this.rigidbody = ai.gameObject.GetComponent<Rigidbody>();
        this.enemyStats = ai.enemyStats;
        this.target = target;
        this.blackboard = ai.knownEnemiesBlackboard;
        this.enemyThinker = ai.enemyThinker;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = ai.transform.position;

        Vector3 targetPosition = new Vector3();
        if (target.Equals(EnemyAI.Target.Enemy))
        {
            targetPosition = blackboard.GetClosestCurrentPosition(aiPosition);
        }
        if (targetPosition == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        ai.SetColor(Color.magenta);
        enemyThinker.isDashing = true;
        enemyThinker.dashStartTime = ai.timer;
        Debug.LogError("Dash");
        return NodeState.SUCCESS;
        
    }
}
