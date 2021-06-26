using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    /*private float viewRadius;
    [Range(0, 360)]
    private float viewAngle;

    private LayerMask enemyMask;
    private LayerMask coverMask;

    private List<Transform> visibleEnemies = new List<Transform>();
    private GameObject ai;*/

    /*private void Start()
    {
        //StartCoroutine("FindEnemiesWithDelay", .2f);
    }*/

    IEnumerator FindEnemiesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            //FindVisibleEnemies();
        }
    }

    public List<Transform> FindVisibleEnemies(float viewRadius, float viewAngle, LayerMask enemyMask, LayerMask coverMask, List<Transform> visibleEnemies, GameObject ai)
    {
        visibleEnemies.Clear();

        Collider[] enemiesInViewRadius = Physics.OverlapSphere(ai.transform.position, viewRadius, enemyMask);

        for (int i = 0; i < enemiesInViewRadius.Length; i++)
        {
            Transform enemy = enemiesInViewRadius[i].transform;
            Vector3 dirToEnemy = (enemy.position - ai.transform.position).normalized;
            if (Vector3.Angle(ai.transform.forward, dirToEnemy) < viewAngle / 2)
            {
                float distToEnemy = Vector3.Distance(ai.transform.position, enemy.position);

                if (!Physics.Raycast(ai.transform.position, dirToEnemy, distToEnemy, coverMask))
                {
                    visibleEnemies.Add(enemy);
                }
            }
        }
        return visibleEnemies;
    }

    //Use OverlapSphere first to determine all colliders around the object, then limit only to those between the viewing angles
    public List<Transform> FindVisibleObjects(float viewRadius, float viewAngle, LayerMask coverMask, LayerMask targetObjects, List<Transform> visibleObjects, GameObject ai)
    {
        visibleObjects.Clear();
        Collider[] objectsInViewRadius = Physics.OverlapSphere(ai.transform.position, viewRadius, targetObjects);

        return IterateThroughRadius(objectsInViewRadius, viewAngle, ai, coverMask, visibleObjects);
    }

    //If the list of colliders is already available, use this one
    public List<Transform> FindVisibleObjects(float viewAngle, LayerMask coverMask, Collider[] targetObjects, List<Transform> visibleObjects, GameObject ai)
    {
        return IterateThroughRadius(targetObjects, viewAngle, ai, coverMask, visibleObjects);
    }

    private List<Transform> IterateThroughRadius(Collider[] objectsInViewRadius, float viewAngle, GameObject ai, LayerMask coverMask, List<Transform> visibleObjects)
    {
        for (int i = 0; i < objectsInViewRadius.Length; i++)
        {
            Transform enemy = objectsInViewRadius[i].transform;
            Vector3 dirToEnemy = (enemy.position - ai.transform.position).normalized;
            if (Vector3.Angle(ai.transform.forward, dirToEnemy) < viewAngle / 2)
            {
                float distToEnemy = Vector3.Distance(ai.transform.position, enemy.position);

                if (!Physics.Raycast(ai.transform.position, dirToEnemy, distToEnemy, coverMask))
                {
                    visibleObjects.Add(enemy);
                }
            }
        }
        return visibleObjects;
    }
}
