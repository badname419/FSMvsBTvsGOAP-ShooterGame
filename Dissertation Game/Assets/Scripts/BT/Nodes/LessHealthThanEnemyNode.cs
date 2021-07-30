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
        GameObject theClosestEnemy = blackboard.DetermineTheClosestEnemy(ai.transform.position);
        int index = blackboard.FindEnemyIndex(theClosestEnemy.transform);

        return ai.currentHealth < blackboard.knownEnemiesList[index].hp ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
