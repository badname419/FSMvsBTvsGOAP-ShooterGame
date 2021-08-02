using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchRandomlyNode: Node
{
    private EnemyAI ai;

    public SearchRandomlyNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("searching");
        return NodeState.RUNNING;
    }
}
