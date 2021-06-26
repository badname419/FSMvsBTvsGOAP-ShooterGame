using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numOfEnemies = 1;
    public List<Transform> spawnPoints;
    public GameObject enemyObject;

    private Pathfinding pathfinding;
    private KnownEnemiesBlackboard knownEnemies;


    // Start is called before the first frame update
    void Awake()
    {
        knownEnemies = new KnownEnemiesBlackboard();
        pathfinding = GetComponent<Pathfinding>();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    { 
        for(int i=0; i<numOfEnemies; i++)
        {
            int pointIndex = i % spawnPoints.Count;
            var enemy = Instantiate(enemyObject);

            var enemyThinker = enemy.GetComponent<EnemyThinker>();
            enemyThinker.Setup(spawnPoints[pointIndex], pathfinding, knownEnemies);
            enemyThinker.SetupUI(true, spawnPoints);
        }
    }
}
