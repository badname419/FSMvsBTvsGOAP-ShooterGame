using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnyEnemiesSeenNode: Node
{
    private EnemyAI ai;

    public AnyEnemiesSeenNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("enemies seen: ");
        //Debug.Log(ai.knownEnemiesBlackboard.AnyEnemiesSeen());
        return ai.knownEnemiesBlackboard.AnyEnemiesSeen() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
