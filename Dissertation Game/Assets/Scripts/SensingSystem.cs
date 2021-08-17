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
        knownEnemiesBlackboard = enemyThinker.knownEnemiesBlackboard;
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
        LayerMask enemyMask = enemyThinker.enemyMask;
        Collider[] enemiesInHearingRadius = Physics.OverlapSphere(transform.position, hearingRadius, enemyMask);

        for(int i=0; i<enemiesInHearingRadius.Length; i++)
        {
            Transform enemy = enemiesInHearingRadius[i].transform;
            bool isMoving;

            if(enemy.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
            {
                isMoving = playerMovement.IsMoving();
            }
            else
            {
                EnemyThinker thinker = enemy.GetComponent<EnemyThinker>();
                if (thinker != null)
                {
                    isMoving = !thinker.navMeshAgent.isStopped;
                    if (!isMoving)
                    {
                        isMoving = thinker.isDashing;
                    }
                }
                else
                {
                    return;
                }
            }

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
        Transform closestKit;
        int index = 0;
        if (foundKitsList.Count.Equals(0))
        {
            return this.transform;
        }
        else if (foundKitsList.Count.Equals(1))
        {
            if (foundKitsList[index].transform != null)
            {
                closestKit = foundKitsList[index].transform;
            }
            else
            {
                return this.transform;
            }
        }
        else
        {
            float shortestDistance = 0f;

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
            if (foundKitsList.Count > index)
            {
                closestKit = foundKitsList[index].transform;
            }
            else
            {
                closestKit = null;
            }
        }

        if(closestKit == null)
        {
            if (foundKitsList.Count > index)
            {
                foundKitsList.RemoveAt(index);
                if (foundKitsList.Count != 0)
                {
                    closestKit = DetermineClosestKit(origin);
                }
                else
                {
                    return this.transform;
                }
            }
        }

        return closestKit;
    }

    public void RegisterHit(Transform bulletOwner)
    {
        if (!bulletOwner.CompareTag(transform.tag))
        {
            knownEnemiesBlackboard.UpdateEnemyList(bulletOwner);
        }
    }

    public void RemoveKit(Transform transform)
    {
        for(int i=0; i < foundKitsList.Count; i++)
        {
            if (foundKitsList[i].transform.Equals(transform))
            {
                foundKitsList.RemoveAt(i);
                break;
            }
        }
    }
}
