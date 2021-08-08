using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Dash")]
public class DashAction : Action
{
    public override void Act(StateController controller)
    {
        Dash(controller);
    }

    private void Dash(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;

        EnemyStats enemyStats = enemyThinker.enemyStats;
        Rigidbody rigidbody = enemyThinker.gameObject.GetComponent<Rigidbody>();

        if (!enemyThinker.isDashing)
        {
            if (enemyThinker.lookingAtTarget)
            {
                enemyThinker.dashStartTime = enemyThinker.timer;
                enemyThinker.isDashing = true;
            }
        }

        if (enemyThinker.isDashing)
        {
            rigidbody.velocity = new Vector3(enemyThinker.transform.forward.x * enemyStats.dashForce, 0f, enemyThinker.transform.forward.z * enemyStats.dashForce);
        }
    }
}
