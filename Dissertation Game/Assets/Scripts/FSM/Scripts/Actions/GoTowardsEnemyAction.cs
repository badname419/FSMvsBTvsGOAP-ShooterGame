using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Actions/GoTowardsEnemy")]
public class GoTowardsEnemyAction : Action
{
    public override void Act(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        NavMeshAgent agent = enemyThinker.navMeshAgent;
        Vector3 aiPosition = enemyThinker.transform.position;
        Vector3 targetPosition = enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(aiPosition);

        enemyThinker.walkingTarget = targetPosition;

        controller.walkingTargetEnum = StateController.Target.Enemy;
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
    }
}
