using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensingSystem : MonoBehaviour
{
    private EnemyThinker enemyThinker;
    private EnemyStats enemyStats;
    private KnownEnemiesBlackboard knownEnemiesBlackboard;
    private List<Transform> foundKitsList;

    // Start is called before the first frame update
    void Start()
    {
        enemyThinker = GetComponent<EnemyThinker>();
        enemyStats = enemyThinker.enemyStats;
        knownEnemiesBlackboard = enemyThinker.knownEnemies;
        foundKitsList = new List<Transform>();

        StartCoroutine("FindEnemiesWithDelay", .2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FindEnemiesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            ListenForEnemies();
            DetectKits();
        }
    }

    private void ListenForEnemies()
    {
        float hearingRadius = enemyStats.hearingRange;
        LayerMask enemyMask = enemyStats.enemyLayer;
        Collider[] enemiesInHearingRadius = Physics.OverlapSphere(transform.position, hearingRadius, enemyMask);

        for(int i=0; i<enemiesInHearingRadius.Length; i++)
        {
            Transform enemy = enemiesInHearingRadius[i].transform;
            bool isMoving = enemy.GetComponent<PlayerMovement>().IsMoving();

            if (isMoving)
            {
                enemyThinker.SetCombat(true);
                knownEnemiesBlackboard.UpdateEnemyList(enemy);
            }
        }
    }

    private void DetectKits()
    {
        foundKitsList.Clear();

        LayerMask kitMask = enemyStats.kitMask;
        float kitDetectionRadius = enemyStats.kitDetectionRange;
        Collider[] kitsInRadius = Physics.OverlapSphere(transform.position, kitDetectionRadius, kitMask);

        foreach(Collider kit in kitsInRadius)
        {
            foundKitsList.Add(kit.transform);
        }
    }

    public bool KitsInRange()
    {
        return foundKitsList.Count != 0;
    }

    public Transform DetermineClosestKit(Vector3 origin)
    {
        if (foundKitsList.Count.Equals(1))
        {
            return foundKitsList[0].transform;
        }
        else
        {
            float shortestDistance = 0f;
            int index = 0;

            for(int i=0; i<foundKitsList.Count; i++)
            {
                float distance = Vector3.Distance(origin, foundKitsList[i].transform.position);
                if(i == 0)
                {
                    shortestDistance = distance;
                }
                else
                {
                    if(distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        index = i;
                    }
                }
            }
            return foundKitsList[index].transform;
        }
    }

    public void RegisterHit(Transform bulletOwner)
    {
        if (!bulletOwner.CompareTag(transform.tag))
        {
            knownEnemiesBlackboard.UpdateEnemyList(bulletOwner);
        }
    }
}
