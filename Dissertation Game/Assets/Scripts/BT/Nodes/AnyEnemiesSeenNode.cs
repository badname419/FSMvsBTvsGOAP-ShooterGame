using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnyEnemiesSeenNode: Node
{
    private EnemyThinker enemyThinker;

    public AnyEnemiesSeenNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("enemies seen: ");
        //Debug.Log(ai.knownEnemiesBlackboard.AnyEnemiesSeen());
        return enemyThinker.knownEnemiesBlackboard.AnyEnemiesSeen() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
