using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Actions/GoTowardsCover")]
public class GoTowardsCoverAction : Action
{
    public override void Act(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        NavMeshAgent agent = enemyThinker.navMeshAgent;
        Vector3 targetPosition = enemyThinker.GetBestCoverSpot().position;

        enemyThinker.walkingTarget = targetPosition;

        controller.walkingTargetEnum = StateController.Target.Enemy;
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
    }
}
