using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessHealthThanEnemyNode : Node
{
    private EnemyThinker enemyThinker;

    public LessHealthThanEnemyNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
    }

    public override NodeState Evaluate()
    {
        KnownEnemiesBlackboard blackboard = enemyThinker.knownEnemiesBlackboard;
        Vector3 aiPosition = enemyThinker.transform.position;
        GameObject theClosestEnemy = blackboard.DetermineTheClosestEnemyObject(aiPosition);
        int index;
        if (theClosestEnemy != null)
        {
            index = blackboard.FindEnemyIndex(theClosestEnemy.transform);
        }
        else
        {
            return NodeState.FAILURE;
        }

        return enemyThinker.currentHP < blackboard.knownEnemiesList[index].hp ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
