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

    private string playerTag = "Player";
    private EnemyStats enemyStats;
    private KnownEnemiesBlackboard knownEnemiesBlackboard;

    public List<VisibleEnemy> visibleEnemies;
    public bool seesEnemy;
    public Vector3 closestEnemyPosition;
    public Vector3 lastKnownEnemyPosition;

    public class VisibleEnemy
    {
        public VisibleEnemy(Transform transform, float distance)
        {
            this.transform = transform;
            this.distance = distance;
        }

        public Transform transform { set; get; }
        public float distance { set; get; }
    }

    private void Start()
    {
        closestEnemyPosition = Vector3.zero;
        lastKnownEnemyPosition = closestEnemyPosition;
        knownEnemiesBlackboard = GetComponent<EnemyThinker>().knownEnemies;

        visibleEnemies = new List<VisibleEnemy>();
        seesEnemy = false;
        if (!this.CompareTag(playerTag))
        {
            enemyStats = GetComponent<EnemyThinker>().enemyStats;
            Debug.Log("Test");
            StartCoroutine("FindEnemiesWithDelay", .2f);
        }
    }

    IEnumerator FindEnemiesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleEnemies(enemyStats.viewRadius, enemyStats.viewAngle, enemyStats.enemyLayer, enemyStats.coverMask, this.transform);
            DetermineTheClosestEnemy();
        }
    }

    public void FindVisibleEnemies(float viewRadius, float viewAngle, LayerMask enemyMask, LayerMask coverMask, Transform transform)
    {
        visibleEnemies.Clear();

        Collider[] enemiesInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);

        for (int i = 0; i < enemiesInViewRadius.Length; i++)
        {
            Transform enemy = enemiesInViewRadius[i].transform;
            Vector3 dirToEnemy = (enemy.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToEnemy) < viewAngle / 2)
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.position);

                if (!Physics.Raycast(transform.position, dirToEnemy, distToEnemy, coverMask))
                {
                    VisibleEnemy visibleEnemy = new VisibleEnemy(enemy, distToEnemy);
                    visibleEnemies.Add(visibleEnemy);
                    knownEnemiesBlackboard.UpdateEnemyList(enemy);
                }
            }
        }     
        seesEnemy = visibleEnemies.Count != 0;
    }

    public List<Transform> GetVisibleEnemyTransforms()
    {
        if(visibleEnemies.Count != 0)
        {
            List<Transform> visibleTransforms = new List<Transform>();
            for(int i=0; i<visibleEnemies.Count; i++)
            {
                visibleTransforms.Add(visibleEnemies[i].transform);
            }
            return visibleTransforms;
        }
        else
        {
            return null;
        }
    }

    private void DetermineTheClosestEnemy()
    {
        float shortestDistance = 0f;
        int index = -1;

        if (visibleEnemies.Count == 1)
        {
            closestEnemyPosition = visibleEnemies[0].transform.position;
            lastKnownEnemyPosition = closestEnemyPosition;
        }
        else
        {
            for (int i = 0; i < visibleEnemies.Count; i++)
            {
                float distToEnemy = Vector3.Distance(transform.position, visibleEnemies[i].transform.position);

                if (i == 0)
                {
                    shortestDistance = distToEnemy;
                }
                else
                {
                    if(distToEnemy < shortestDistance)
                    {
                        shortestDistance = distToEnemy;
                        index = i;
                    }
                }
            }
            if (index != -1)
            {
                closestEnemyPosition = visibleEnemies[index].transform.position;
                lastKnownEnemyPosition = closestEnemyPosition;
            }
            else
            {
                closestEnemyPosition = Vector3.zero;
            }
        }

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
