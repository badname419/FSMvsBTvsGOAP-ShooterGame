using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Pathfinding pathfinding;
    private string cameraTag = "MainCamera";

    [Header("Teams")]
    public GameObject team1;
    public GameObject team2;

    [Header("Tags")]
    public string playerTag;
    public string team1Tag;
    public string team2Tag;

    [Header("Masks")]
    public LayerMask playerMask;
    public List<LayerMask> enemyMasksList;

    [Header("Game Settings")]
    public int numOfEnemies = 1;
    public List<Transform> spawnPoints1;
    public List<Transform> spawnPoints2;
    public List<Transform> searchPoints;
    public GameObject enemyObject;
    public GameObject playerObject;
    public GameObject firstAidKitObject;
    public KnownEnemiesBlackboard knownEnemies1;
    public KnownEnemiesBlackboard knownEnemies2;
    [SerializeField] private int maximumNumOfKits;
    [SerializeField] private int currentNumOfKits;
    [SerializeField] private int kitSpawnRate;
    private List<Vector3> kitLocationList;

    private bool pve;


    // Start is called before the first frame update
    void Awake()
    {
        knownEnemies1 = new KnownEnemiesBlackboard();
        knownEnemies2 = new KnownEnemiesBlackboard();
        pathfinding = GetComponent<Pathfinding>();
        kitLocationList = new List<Vector3>();

        if (playerMask.value == (playerMask.value | (1 << team1.layer)))
        {
            SpawnPlayer();
            pve = true;
        }
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
        for (int i = 0; i < numOfEnemies; i++)
        {
            int pointIndex = i % spawnPoints1.Count;
            GameObject enemy = Instantiate(team2);
            EnemyThinker enemyThinker = enemy.GetComponent<EnemyThinker>();
            enemyThinker.Setup(spawnPoints1[pointIndex], pathfinding, knownEnemies1, searchPoints);
        }

        if (!pve)
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                int pointIndex = i % spawnPoints2.Count;
                GameObject enemy = Instantiate(team1);
                EnemyThinker enemyThinker = enemy.GetComponent<EnemyThinker>();
                enemyThinker.Setup(spawnPoints2[pointIndex], pathfinding, knownEnemies2, searchPoints);
            }
        }
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(team1);
        SetupCamera(player);
        var playerLogic = player.GetComponent<PlayerLogic>();

        playerLogic.Setup(knownEnemies1);
    }

    private void SetupCamera(GameObject playerObject)
    {
        GameObject mainCamera = GameObject.FindGameObjectsWithTag(cameraTag)[0];
        Camera cameraScript = mainCamera.GetComponent<Camera>();
        if (pve)
        {
            cameraScript.player = playerObject.transform;
        }
        else
        {
            cameraScript.pve = pve;
        }
    }

    public void ReduceNumOfKits()
    {
        currentNumOfKits--;
    }

    public bool IsPVE()
    {
        return pve;
    }
}
