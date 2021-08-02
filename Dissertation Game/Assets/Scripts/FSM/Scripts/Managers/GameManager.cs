using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numOfEnemies = 1;
    public List<Transform> spawnPoints;
    public GameObject enemyObject;
    public GameObject playerObject;
    //public GameObject cameraObject;

    private Pathfinding pathfinding;
    public KnownEnemiesBlackboard knownEnemies;
    private string cameraTag = "MainCamera";


    // Start is called before the first frame update
    void Awake()
    {
        knownEnemies = new KnownEnemiesBlackboard();
        pathfinding = GetComponent<Pathfinding>();
        SpawnPlayer();
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

    private void SpawnPlayer()
    {
        var player = Instantiate(playerObject);
        SetupCamera(player);
        var playerLogic = player.GetComponent<PlayerLogic>();

        playerLogic.Setup(knownEnemies);
    }

    private void SetupCamera(GameObject playerObject)
    {
        GameObject mainCamera = GameObject.FindGameObjectsWithTag(cameraTag)[0];
        Camera cameraScript = mainCamera.GetComponent<Camera>();
        cameraScript.player = playerObject.transform;
    }
}
