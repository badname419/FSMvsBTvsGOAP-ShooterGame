using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Pathfinding pathfinding;
    private string cameraTag = "MainCamera";

    public int numOfEnemies = 1;
    public List<Transform> spawnPoints;
    public List<Transform> searchPoints;
    public GameObject enemyObject;
    public GameObject playerObject;
    public GameObject firstAidKitObject;
    public KnownEnemiesBlackboard knownEnemies;
    [SerializeField] private int maximumNumOfKits;
    [SerializeField] private int currentNumOfKits;
    [SerializeField] private int kitSpawnRate;
    private List<Vector3> kitLocationList;


    // Start is called before the first frame update
    void Awake()
    {
        knownEnemies = new KnownEnemiesBlackboard();
        pathfinding = GetComponent<Pathfinding>();
        kitLocationList = new List<Vector3>();
        SpawnPlayer();
        SpawnEnemies();
    }

    private void Start()
    {
        StartCoroutine(KitCoroutine(kitSpawnRate));
    }

    private void Update()
    {

    }

    IEnumerator KitCoroutine(int waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            if (KitCanBeSpawned())
            {
                SpawnKit();
            }
        }
    }

    private bool KitCanBeSpawned()
    {
        return currentNumOfKits < maximumNumOfKits;
    }

    private void SpawnKit()
    {
        Debug.Log("Kit");
        GameObject kitObject = Instantiate(firstAidKitObject, RandomizePosition(), Quaternion.identity);
        kitObject.transform.Rotate(-90f, 0f, 0f);
        KitScript kitScript = kitObject.GetComponent<KitScript>();
        kitScript.SetGameManager(this);
        kitLocationList.Add(kitObject.transform.position);
        currentNumOfKits++;
    }

    private Vector3 RandomizePosition()
    {
        Grid grid = pathfinding.GetGrid();
        int minX = 0;
        int minZ = 0;
        int maxX = grid.GetGridSizeX();
        int maxZ = grid.GetGridSizeZ();
        int posX = 0;
        int posZ = 0;
        bool valid = false;

        while (!valid)
        {
            posX = Random.Range(minX, maxX - 1);
            posZ = Random.Range(minZ, maxZ - 1);
            if (IsKitLocationValid(grid, posX, posZ));
            {
                valid = true;
            }
        }

        return grid.GetNodePosition(posX, posZ);
    }

    private bool IsKitLocationValid(Grid grid, int posX, int posZ)
    {
        if(grid.IsValid(posX, posZ))
        {
            for(int i=0; i<kitLocationList.Count; i++)
            {
                Vector3 searchedLocation = grid.GetNodePosition(posX, posZ);
                if (searchedLocation.Equals(kitLocationList[i]))
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpawnEnemies()
    { 
        for(int i=0; i<numOfEnemies; i++)
        {
            int pointIndex = i % spawnPoints.Count;
            var enemy = Instantiate(enemyObject);

            var enemyThinker = enemy.GetComponent<EnemyThinker>();
            enemyThinker.Setup(spawnPoints[pointIndex], pathfinding, knownEnemies, searchPoints);
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

    public void ReduceNumOfKits()
    {
        currentNumOfKits--;
    }
}
