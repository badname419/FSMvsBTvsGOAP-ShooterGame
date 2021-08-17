using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public string playerTag;
    private EnemyStats enemyStats;
    private KnownEnemiesBlackboard knownEnemiesBlackboard;

    public List<VisibleEnemy> visibleEnemies;
    public bool seesEnemy;
    public Vector3 closestEnemyPosition;
    public Vector3 lastKnownEnemyPosition;
    public GameObject closestEnemyObject;
    private EnemyThinker enemyThinker;

    //Debug
    GameObject signalCube;

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

    private void Awake()
    {
        enemyThinker = GetComponent<EnemyThinker>();
    }

    private void Start()
    {
        closestEnemyPosition = Vector3.zero;
        lastKnownEnemyPosition = closestEnemyPosition;
        //knownEnemiesBlackboard = GetComponent<EnemyThinker>().knownEnemiesBlackboard;
        //playerTag = enemyThinker.GetGameManager().playerTag;

        visibleEnemies = new List<VisibleEnemy>();
        seesEnemy = false;
        /*
        if (this.gameObject.name == "EnemyBT(Clone)")
        {
            signalCube = transform.Find("SignalCube").gameObject;
        }*/
        if (!this.CompareTag(playerTag))
        {
            enemyStats = enemyThinker.enemyStats;
            StartCoroutine("FindEnemiesWithDelay", .1f);
        }
    }

    IEnumerator FindEnemiesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleEnemies(enemyStats.viewRadius, enemyStats.viewAngle, enemyThinker.enemyMask, enemyStats.coverMask, this.transform);
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
            Vector3 fromVision = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z - 1);
            if (Vector3.Angle(transform.forward , dirToEnemy) < viewAngle / 2)
            {
                float distToEnemy = Vector3.Distance(transform.position, enemy.position);

                Debug.DrawRay(transform.position, enemy.position - transform.position, Color.green, 0.1f);


                RaycastHit hit;
                if (Physics.SphereCast(transform.position, 0.45f, dirToEnemy, out hit))
                {
                    if (!hit.collider.CompareTag(enemyStats.wallTag) && !hit.collider.CompareTag(transform.tag))
                    {
                        VisibleEnemy visibleEnemy = new VisibleEnemy(enemy, distToEnemy);
                        visibleEnemies.Add(visibleEnemy);
                        knownEnemiesBlackboard.UpdateEnemyList(enemy);
                    }
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
            if (ai != null && spot != null)
            {
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
            else
            {
                continue;
            }
        }
        return visibleObjects;
    }

    public void SetupEnemyBlackboard(KnownEnemiesBlackboard blackboard)
    {
        this.knownEnemiesBlackboard = blackboard;
    }

    public void SetupPlayerTag(string playerTag)
    {
        this.playerTag = playerTag;
    }
}
