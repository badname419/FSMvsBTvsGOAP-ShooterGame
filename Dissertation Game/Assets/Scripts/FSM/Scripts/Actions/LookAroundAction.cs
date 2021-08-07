using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/LookAround")]
public class LookAroundAction : Action
{
    public override void Act(StateController controller)
    {
        LookAround(controller);
    }

    private void LookAround(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        EnemyStats enemyStats = enemyThinker.enemyStats;
        Vector3 aiPosition = enemyThinker.transform.position;       
        float radius = 10f;
        
        //If the values are zero it means this script is run for the first time
        if(enemyThinker.forwardRotationTarget == Vector3.zero)
        {
           enemyThinker.targetArray = FindRotationTargets(enemyThinker, enemyStats, aiPosition, radius);
        }

        Vector3 targetPosition = enemyThinker.targetArray[enemyThinker.numOfRotations];
        Vector3 targetDir = targetPosition - aiPosition;
        float angle = Vector3.Angle(targetDir, controller.transform.forward);

        if(angle > enemyStats.minimumLookingAngle)
        {
            var targetRotation = Quaternion.LookRotation(targetPosition - aiPosition);
            var str = Mathf.Min(enemyStats.rotationSpeed / 2 * Time.deltaTime, 1);
            enemyThinker.transform.rotation = Quaternion.Lerp(controller.transform.rotation, targetRotation, str);
        }
        else
        {
            enemyThinker.numOfRotations++;
        }

    }

    private Vector3[] FindRotationTargets(EnemyThinker enemyThinker, EnemyStats enemyStats, Vector3 currentPosition, float radius)
    {
        Vector3[] targetArray = new Vector3[enemyThinker.totalRotations];
        Vector3 lastKnownPosition = enemyThinker.lastKnownEnemyLoc;
        Vector3 forwardVector = (enemyThinker.lastKnownEnemyLoc - currentPosition).normalized;
        float rotationAngle = enemyStats.rotationAngle;
        forwardVector.y = 0;
        
        enemyThinker.forwardRotationTarget = currentPosition + forwardVector * radius;

        Vector3 rightVector = Quaternion.Euler(0, rotationAngle, 0) * forwardVector;
        Vector3 leftVector = Quaternion.Euler(0, 360 - rotationAngle, 0) * forwardVector;

        enemyThinker.rightRotationTarget = currentPosition + rightVector * radius;
        enemyThinker.leftRotationTarget = currentPosition + leftVector * radius;

        targetArray[0] = targetArray[2] = targetArray[4] = enemyThinker.forwardRotationTarget;
        targetArray[1] = enemyThinker.leftRotationTarget;
        targetArray[3] = enemyThinker.rightRotationTarget;
        return targetArray;
    }
}