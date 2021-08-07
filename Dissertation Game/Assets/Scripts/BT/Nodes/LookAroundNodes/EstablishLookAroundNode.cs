using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EstablishLookAroundNode: Node
{
    private EnemyStats enemyStats;
    private EnemyThinker enemyThinker;
    private Vector3[] targetArray;
    private float radius;

    public EstablishLookAroundNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
        this.enemyStats = enemyThinker.enemyStats;
        this.targetArray = new Vector3[enemyThinker.totalRotations];
        radius = 10f;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = enemyThinker.transform.position;
        Vector3 lastKnownPosition = enemyThinker.lastKnownEnemyLoc;
        Vector3 forwardVector = (lastKnownPosition - aiPosition).normalized;     
        float rotationAngle = enemyStats.rotationAngle;
        forwardVector.y = 0;

        enemyThinker.forwardRotationTarget = aiPosition + forwardVector * radius;

        Vector3 rightVector = Quaternion.Euler(0, rotationAngle, 0) * forwardVector;
        Vector3 leftVector = Quaternion.Euler(0, 360 - rotationAngle, 0) * forwardVector;

        enemyThinker.rightRotationTarget = aiPosition + rightVector * radius;
        enemyThinker.leftRotationTarget = aiPosition + leftVector * radius;

        targetArray[0] = targetArray[2] = targetArray[4] = enemyThinker.forwardRotationTarget;
        targetArray[1] = enemyThinker.leftRotationTarget;
        targetArray[3] = enemyThinker.rightRotationTarget;

        enemyThinker.targetArray = targetArray;
        enemyThinker.aiRotatingPosition = aiPosition;
        enemyThinker.numOfRotations = 0;

        return NodeState.SUCCESS;
    }
}
