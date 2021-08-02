using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessHealthThanEnemyNode : Node
{
    private EnemyAI ai;
    //private KnownEnemiesBlackboard blackboard;

    public LessHealthThanEnemyNode(EnemyAI ai)
    {
        this.ai = ai;
        //this.blackboard = ai.knownEnemiesBlackboard;
    }

    public override NodeState Evaluate()
    {
        KnownEnemiesBlackboard blackboard = ai.knownEnemiesBlackboard;
        GameObject theClosestEnemy = blackboard.DetermineTheClosestEnemyObject(ai.transform.position);
        int index;
        if (theClosestEnemy != null)
        {
            index = blackboard.FindEnemyIndex(theClosestEnemy.transform);
        }
        else
        {
            return NodeState.FAILURE;
        }

        return ai.enemyThinker.currentHP < blackboard.knownEnemiesList[index].hp ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
