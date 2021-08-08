using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/LookAt")]
public class LookAtEnemyAction : Action
{
    public override void Act(StateController controller)
    {
        LookAt(controller);
    }

    private void LookAt(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        if (!enemyThinker.isDashing)
        {
            EnemyStats enemyStats = enemyThinker.enemyStats;
            Vector3 aiPosition = enemyThinker.transform.position;
            Vector3 targetPosition = enemyThinker.knownEnemiesBlackboard.GetClosestCurrentPosition(aiPosition);
            Transform aiTransform = enemyThinker.transform;

            Vector3 targetDir = targetPosition - aiPosition;
            float angle = Vector3.Angle(targetDir, aiTransform.forward);

            if (angle > enemyStats.minimumLookingAngle)
            {
                if (enemyThinker.lookingAtTarget == true)
                {
                    enemyThinker.lookingAtTarget = false;
                }
                var targetRotation = Quaternion.LookRotation(targetPosition - aiPosition);
                var str = Mathf.Min(enemyStats.rotationSpeed * Time.deltaTime, 1);
                enemyThinker.transform.rotation = Quaternion.Lerp(aiTransform.rotation, targetRotation, str);
            }
            else
            {
                enemyThinker.lookingAtTarget = true;
            }
        }
    }
}
