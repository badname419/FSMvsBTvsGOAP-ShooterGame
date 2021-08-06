using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private string playerTag = "Player";
    private EnemyStats enemyStats;
    private KnownEnemiesBlackboard knownEnemiesBlackboard;

    public List<VisibleEnemy> visibleEnemies;
    public bool seesEnemy;
    public Vector3 closestEnemyPosition;
    public Vector3 lastKnownEnemyPosition;
    public GameObject closestEnemyObject;

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
            StartCoroutine("FindEnemiesWithDelay", .1f);
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
        LayerMask thisMask = gameObject.layer;

        Collider[] enemiesInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);

        for (int i = 0; i < enemiesInViewRadius.Length; i++)
        {
            Transform enemy = enemiesInViewRadius[i].transform;
            Vector3 dirToEnemy = (enemy.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToEnemy) < viewAngle / 2)
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.position);

                Debug.DrawRay(transform.position, enemy.position - transform.position, Color.green, 0.1f);


                RaycastHit hit;
                if (Physics.SphereCast(transform.position, 0.3f, dirToEnemy, out hit))
                {
                    if (!hit.collider.CompareTag("Wall") && !hit.collider.CompareTag(transform.tag))
                    {
                        VisibleEnemy visibleEnemy = new VisibleEnemy(enemy, distToEnemy);
                        visibleEnemies.Add(visibleEnemy);
                        knownEnemiesBlackboard.UpdateEnemyList(enemy);
                    }
                }

                /*
                if (Physics.Raycast(transform.position, dirToEnemy, out hit, distToEnemy))
                {
                    Debug.Log(hit.collider.CompareTag(transform.tag));
                    if (!hit.collider.CompareTag("Wall") && !hit.collider.CompareTag(transform.tag))
                    {
                        VisibleEnemy visibleEnemy = new VisibleEnemy(enemy, distToEnemy);
                        visibleEnemies.Add(visibleEnemy);
                        knownEnemiesBlackboard.UpdateEnemyList(enemy);
                    }
                        
                }

                /*
                if (!Physics.Raycast(transform.position, dirToEnemy, distToEnemy, coverMask))
                {
                    if (!Physics.Raycast(transform.position, dirToEnemy, distToEnemy, thisMask))
                    {
                        VisibleEnemy visibleEnemy = new VisibleEnemy(enemy, distToEnemy);
                        visibleEnemies.Add(visibleEnemy);
                        knownEnemiesBlackboard.UpdateEnemyList(enemy);
                    }
                }*/
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
            closestEnemyObject = visibleEnemies[0].transform.gameObject;
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
                closestEnemyObject = visibleEnemies[index].transform.gameObject;
            }
            else
            {
                closestEnemyPosition = Vector3.zero;
                closestEnemyObject = null;
            }
        }

    }

    public List<Transform> FindVisibleObjects(float viewAngle, LayerMask coverMask, Collider[] targetObjects, GameObject ai)
    {
        List<Transform> visibleObjects = new List<Transform>();
        for (int i = 0; i < targetObjects.Length; i++)
        {
            Transform spot = targetObjects[i].transform;
            Vector3 spotPosition = new Vector3(spot.position.x, 1f, spot.position.z);
            Vector3 dirToSpot = (spotPosition - ai.transform.position).normalized;
            if (Vector3.Angle(ai.transform.forward, dirToSpot) < viewAngle / 2)
            {
                float distToSpot = Vector3.Distance(ai.transform.position, spotPosition);

                if (!Physics.Raycast(ai.transform.position, dirToSpot, distToSpot, coverMask))
                {
                    visibleObjects.Add(spot);
                }
            }
        }
        return visibleObjects;
    }
}
