using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Actions/GoTowardsKit")]
public class GoTowardsKitAction : Action
{
    public override void Act(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        NavMeshAgent agent = enemyThinker.navMeshAgent;
        Vector3 aiPosition = enemyThinker.transform.position;
        Vector3 targetPosition = enemyThinker.sensingSystem.DetermineClosestKit(aiPosition).position;

        enemyThinker.walkingTarget = targetPosition;

        controller.walkingTargetEnum = StateController.Target.Enemy;
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
    }
}
