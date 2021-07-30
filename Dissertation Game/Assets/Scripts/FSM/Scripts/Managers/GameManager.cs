using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numOfEnemies = 1;
    public List<Transform> spawnPoints;
    public GameObject enemyObject;
    public GameObject playerObject;
    public GameObject cameraObject;

    private Pathfinding pathfinding;
    private KnownEnemiesBlackboard knownEnemies;


    // Start is called before the first frame update
    void Awake()
    {
        knownEnemies = new KnownEnemiesBlackboard();
        pathfinding = GetComponent<Pathfinding>();
        SpawnPlayer();
        SpawnEnemies();
        SpawnCamera(playerObject);
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

    private void SpawnPlayer()
    {
        var player = Instantiate(playerObject);
        SpawnCamera(player);
        var playerLogic = player.GetComponent<PlayerLogic>();

        playerLogic.Setup(knownEnemies);
    }

    private void SpawnCamera(GameObject player)
    {
        var camera = Instantiate(cameraObject);
        Camera cameraScript = camera.GetComponent<Camera>();
        cameraScript.player = player.transform;
    }
}
