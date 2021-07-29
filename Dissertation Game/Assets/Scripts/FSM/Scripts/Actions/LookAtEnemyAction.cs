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
        Vector3 current = controller.transform.position;
        Vector3 target = controller.enemyThinker.closestEnemy.position;

        var targetRotation = Quaternion.LookRotation(target - current);
        var str = Mathf.Min(controller.enemyStats.rotationSpeed * Time.deltaTime, 1);
        controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, targetRotation, str);
    }
}
