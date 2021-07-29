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

        List<Transform> visibleEnemiesList = new List<Transform>();

        for (int i = 0; i < enemiesInViewRadius.Length; i++)
        {
            Transform enemy = enemiesInViewRadius[i].transform;
            Vector3 dirToEnemy = (enemy.position - position).normalized;
            if (Vector3.Angle(controller.gameObject.transform.forward, dirToEnemy) < controller.enemyStats.viewAngle / 2)
            {
                float distToEnemy = Vector3.Distance(controller.gameObject.transform.position, enemy.position);

                if (!Physics.Raycast(controller.gameObject.transform.position, dirToEnemy, distToEnemy, controller.enemyStats.coverMask))
                {
                    visibleEnemiesList.Add(enemy);
                    controller.enemyThinker.knownEnemies.UpdateEnemyList(enemy);
                }
            }
        }

        if (visibleEnemiesList.Count != 0)
        {
            controller.enemyThinker.closestEnemy = ChooseTarget(controller, visibleEnemiesList);
            controller.enemyThinker.walkingTarget = controller.enemyThinker.closestEnemy.position;
            Debug.Log("True");
            return true;
        }
        else
        {
            Debug.Log("False");
            return false;
        }
 
    }

    private Transform ChooseTarget(StateController controller, List<Transform> visibleEnemiesList)
    {
        //KnownEnemies knownEnemies = controller.enemyThinker.knownEnemies;
        //Blackboard blackboard = controller.enemyBlackboard;
        float shortestDistance = Mathf.Infinity;
        int index = 0;

        for(int i=0; i < visibleEnemiesList.Count; i++)
        {
            float distance = Vector3.Distance(controller.gameObject.transform.position, visibleEnemiesList[i].position);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                index = i;
            }
        }

        return visibleEnemiesList[index];
    }
}