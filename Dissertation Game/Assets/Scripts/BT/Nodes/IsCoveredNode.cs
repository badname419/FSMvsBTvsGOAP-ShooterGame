using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredNode : Node
{
    private Transform target;
    private Transform origin;
    private EnemyAI ai;

    public IsCoveredNode(EnemyAI ai)
    {
        this.ai = ai;
        //this.target = target;
        //this.origin = origin;

    }

    public override NodeState Evaluate()
    {
        bool covered = false;

        bool isEnemyVisible = ai.fieldOfView.seesEnemy;
        if (isEnemyVisible == false)
        {
            covered = true;
        }
        else
        {

            RaycastHit hit;
            Vector3 direction = (ai.fieldOfView.closestEnemyPosition - ai.bestCoverSpot.position).normalized;
            float distance = Vector3.Distance(ai.bestCoverSpot.position, ai.fieldOfView.closestEnemyPosition);
            if (!Physics.Raycast(ai.bestCoverSpot.position, direction, distance, ai.enemyStats.coverMask))
            {
                covered = false;
            }
            else
            {
                covered = true;
            }
        }
        return covered ? NodeState.SUCCESS : NodeState.FAILURE;
        /*
        RaycastHit hit;
        if(Physics.Raycast(origin.position, target.position - origin.position, out hit))
        {
            if(hit.collider.transform != target)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;*/
    }
}
