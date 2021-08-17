using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Decisions/EnemiesVisible")]
public class EnemiesVisibleDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;
        FieldOfView fieldOfView = enemyThinker.fieldOfView;
        bool enemyVisible = fieldOfView.seesEnemy;
        if (enemyVisible)
        {
            enemyThinker.closestEnemyObject = fieldOfView.closestEnemyObject;
            enemyThinker.walkingTarget = fieldOfView.closestEnemyPosition;
        }
        return enemyVisible;
 
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