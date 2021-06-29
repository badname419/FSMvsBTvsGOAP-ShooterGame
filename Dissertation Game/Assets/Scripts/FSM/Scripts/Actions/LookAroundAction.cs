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
        Vector3 current = controller.transform.position;       
        float radius = 10f;
        
        //If the values are zero it means this script is run for the first time
        if(controller.forwardRotationTarget == Vector3.zero)
        {
           controller.targetArray = FindRotationTargets(controller, current, radius);
        }
        Vector3 target = controller.targetArray[controller.numOfRotations];

        var targetRotation = Quaternion.LookRotation(target - current);
        var str = Mathf.Min(controller.enemyStats.rotationSpeed * Time.deltaTime, 1);
        controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, targetRotation, 0.3f * str);

        Vector3 targetDir = target - current;
        float angleToTarget = Vector3.Angle(controller.transform.forward, targetDir);

        if(angleToTarget <= 4f)
        {
            controller.numOfRotations++;
        }

    }

    private Vector3[] FindRotationTargets(StateController controller, Vector3 currentPosition, float radius)
    {
        Vector3[] targetArray = new Vector3[5];
        Vector3 forwardVector = (controller.lastKnownEnemyLoc - currentPosition).normalized;
        forwardVector.y = 0;
        float rotationAngle = controller.enemyStats.rotationAngle;

        controller.forwardRotationTarget = currentPosition + forwardVector * radius;

        Vector3 rightVector = Quaternion.Euler(0, rotationAngle, 0) * forwardVector;
        Vector3 leftVector = Quaternion.Euler(0, 360 - rotationAngle, 0) * forwardVector;

        controller.rightRotationTarget = currentPosition + rightVector * radius;
        controller.leftRotationTarget = currentPosition + leftVector * radius;

        targetArray[0] = targetArray[2] = targetArray[4] = controller.forwardRotationTarget;
        targetArray[1] = controller.leftRotationTarget;
        targetArray[3] = controller.rightRotationTarget;
        return targetArray;
    }
}