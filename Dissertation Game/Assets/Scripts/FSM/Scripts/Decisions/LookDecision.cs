using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Decisions/Look")]
public class LookDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {

        Vector3 position = controller.gameObject.transform.position;
        Collider[] enemiesInViewRadius = Physics.OverlapSphere(position, controller.enemyStats.viewRadius, controller.enemyStats.enemyLayer);

        for (int i = 0; i < enemiesInViewRadius.Length; i++)
        {
            Transform enemy = enemiesInViewRadius[i].transform;
            Vector3 dirToEnemy = (enemy.position - position).normalized;
            if (Vector3.Angle(controller.gameObject.transform.forward, dirToEnemy) < controller.enemyStats.viewAngle / 2)
            {
                float distToEnemy = Vector3.Distance(controller.gameObject.transform.position, enemy.position);

                if (!Physics.Raycast(controller.gameObject.transform.position, dirToEnemy, distToEnemy, controller.enemyStats.coverMask))
                {
                    controller.enemyThinker.knownEnemies.AddEnemy(enemy);
                    //controller.enemyBlackboard.AddEnemy(enemy);
                }
            }
        }

        if (controller.enemyThinker.knownEnemies.enemyTransforms.Count != 0)
        {
            controller.spottedEnemy = ChooseTarget(controller);
            return true;
        }
        else
        {
            return false;
        }
 
    }

    private Transform ChooseTarget(StateController controller)
    {
        KnownEnemies knownEnemies = controller.enemyThinker.knownEnemies;
        //Blackboard blackboard = controller.enemyBlackboard;
        float shortestDistance = Mathf.Infinity;
        int index = 0;

        for(int i=0; i < knownEnemies.enemyTransforms.Count; i++)
        {
            float distance = Vector3.Distance(controller.gameObject.transform.position, knownEnemies.enemyPositions[i]);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                index = i;
            }
        }

        return knownEnemies.enemyTransforms[index];
    }
}