using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensingSystem : MonoBehaviour
{
    private EnemyThinker enemyThinker;
    private EnemyStats enemyStats;
    private KnownEnemiesBlackboard knownEnemiesBlackboard;

    // Start is called before the first frame update
    void Start()
    {
        enemyThinker = GetComponent<EnemyThinker>();
        enemyStats = enemyThinker.enemyStats;
        knownEnemiesBlackboard = enemyThinker.knownEnemies;

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

    public void RegisterHit(Transform bulletOwner)
    {
        if (!bulletOwner.CompareTag(transform.tag))
        {
            knownEnemiesBlackboard.UpdateEnemyList(bulletOwner);
        }
    }
}
