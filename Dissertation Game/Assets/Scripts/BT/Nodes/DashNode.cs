using UnityEngine;

public class DashNode: Node
{
    private EnemyAI.Target target;
    private EnemyThinker enemyThinker;

    public DashNode(EnemyThinker enemyThinker, EnemyAI.Target target)
    {
        this.target = target;
        this.enemyThinker = enemyThinker;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = enemyThinker.transform.position;

        Vector3 targetPosition = new Vector3();
        if (target.Equals(EnemyAI.Target.Enemy))
        {
            targetPosition = enemyThinker.knownEnemiesBlackboard.GetClosestCurrentPosition(aiPosition);
        }
        if (targetPosition == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        enemyThinker.isDashing = true;
        enemyThinker.dashStartTime = enemyThinker.timer;
        return NodeState.SUCCESS;
        
    }
}
