using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject BTPrefab;
    public GameObject FSMPrefab;

    [Header("Game Settings")]
    public int numOfEnemies = 1;
    public int numOfRounds = 1;
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

    public bool pve;
    private int team1Members;
    private int team2Members;
    private int roundsPlayed;
    private bool playerAlive;

    [Header("Pop-up Options")]
    public Canvas popUpCanvas;
    private Text[] popUpTextArray;
    private GameObject popUpObject;
    private Button popUpButton;
    private bool victory;
    public string victoryText;
    public string defeatText;
    public string infoText;

    //Rounds
    List<int> team1Rounds;
    List<int> team2Rounds;
    int team1Wins;
    int team2Wins;


    // Start is called before the first frame update
    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        kitLocationList = new List<Vector3>();
        roundsPlayed = 0;
        team1Rounds = new List<int>();
        team2Rounds = new List<int>();
        knownEnemies1 = new KnownEnemiesBlackboard();
        knownEnemies2 = new KnownEnemiesBlackboard();
        popUpObject = GameObject.Instantiate(popUpCanvas).gameObject;
        popUpObject.SetActive(false);
        popUpTextArray = popUpObject.GetComponentsInChildren<Text>();
        popUpButton = popUpObject.GetComponentsInChildren<Button>()[0];
        popUpButton.onClick.AddListener(StartNextRound);

        /*if (playerMask.value == (playerMask.value | (1 << team1.layer)))
        {
            pve = true;
        }*/

        if (pve)
        {
            numOfRounds *= 2;
            RandomizeTeamOrder();
        }

        StartGame();
    }

    private void Start()
    {
        team1Members = team2Members = numOfEnemies;
        StartCoroutine(KitCoroutine(kitSpawnRate));
    }

    private void Update()
    {
        if (roundsPlayed < numOfRounds)
        {
            if (CheckIfRoundFinished())
            {
                popUpObject.SetActive(true);
                bool victory = CheckIfVictory();
                popUpTextArray[0].text = victory ? victoryText : defeatText;
                string enemyName;
                string infoTextEnding = "-driven enemies! Please take note of that in order to complete the survey.";

                bool team1Round = team1Rounds.Contains(roundsPlayed);
                enemyName = team1Round ? " Behavior-Tree" : " Finite-State Machine";
                popUpTextArray[1].text = infoText + enemyName + infoTextEnding;

                UpdateScore(victory, team1Round);
                if ((roundsPlayed + 1) == numOfRounds)
                {
                    popUpTextArray[2].text = "Display summary";
                    popUpButton.onClick.AddListener(DisplaySummary);
                }
            }
        }
    }

    private void StartNextRound()
    {
        ResetGame();
        roundsPlayed++;
        if (roundsPlayed < numOfRounds)
        {
            StartGame();
        }
    }

    private void UpdateScore(bool victory, bool team1Round)
    {
        if (victory)
        {
            if (team1Round)
            {
                team1Wins++;
            }
            else
            {
                team2Wins++;
            }
        }
    }

    private void DisplaySummary()
    {
        popUpObject.SetActive(true);
        roundsPlayed++;
        popUpTextArray[0].text = "Summary!";
        popUpTextArray[1].text = "You have played against Behavior-Tree-driven enemies " + numOfRounds/2 + " times and won " + team1Wins + " times.\n\n" +
            "You have played against Finite-State Machine-driven enemies " + numOfRounds/2 + " times and won " + team2Wins + " times.";
        popUpTextArray[2].text = "Main Menu";
    }

    private bool CheckIfRoundFinished()
    {
        if(team1Members == 0)
        {
            return true;
        }
        else
        {
            if((pve && playerAlive == false) ||
                (!pve && team2Members == 0))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckIfVictory()
    {
        return team1Members == 0;
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

    private void StartGame()
    {
        knownEnemies1 = new KnownEnemiesBlackboard();
        knownEnemies2 = new KnownEnemiesBlackboard();

        if (pve)
        {
            //numOfRounds *= 2;
            SpawnPlayer();
        }
        SpawnEnemies();

        StartCoroutine(KitCoroutine(kitSpawnRate));
    }

    private bool KitCanBeSpawned()
    {
        return currentNumOfKits < maximumNumOfKits;
    }

    private void SpawnKit()
    {
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
        
        if (pve) 
        {
            GameObject enemyPrefab = team1Rounds.Contains(roundsPlayed) ? BTPrefab : FSMPrefab;
            for (int i = 0; i < numOfEnemies; i++)
            {
                int pointIndex = i % spawnPoints1.Count;
                GameObject enemy = Instantiate(enemyPrefab);
                EnemyThinker enemyThinker = enemy.GetComponent<EnemyThinker>();
                enemyThinker.Setup(spawnPoints1[pointIndex], pathfinding, knownEnemies1, searchPoints, 0);
            }
        }
        else
        {
            //First team
            for (int i = 0; i < numOfEnemies; i++)
            {
                int pointIndex = i % spawnPoints1.Count;
                GameObject enemy = Instantiate(team2);
                EnemyThinker enemyThinker = enemy.GetComponent<EnemyThinker>();
                enemyThinker.Setup(spawnPoints1[pointIndex], pathfinding, knownEnemies1, searchPoints, 0);
            }

            //Second team
            for (int i = 0; i < numOfEnemies; i++)
            {
                int pointIndex = i % spawnPoints2.Count;
                GameObject enemy = Instantiate(team1);
                EnemyThinker enemyThinker = enemy.GetComponent<EnemyThinker>();
                enemyThinker.Setup(spawnPoints2[pointIndex], pathfinding, knownEnemies2, searchPoints, 1);
            }
        }
    }

    private void SpawnPlayer()
    {
        playerAlive = true;
        var player = Instantiate(playerPrefab);
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
        cameraScript.pve = pve;
    }

    public void ReduceNumOfKits()
    {
        currentNumOfKits--;
    }

    public bool IsPVE()
    {
        return pve;
    }

    public void UpdateTeamMembers(int teamNumber)
    {
        if(teamNumber == 0)
        {
            team1Members--;
        }
        else
        {
            team2Members--;
        }
    }

    public void RemoveKit(Vector3 kitLocation)
    {
        for(int i = 0; i<kitLocationList.Count; i++)
        {
            if (kitLocationList[i].Equals(kitLocation))
            {
                kitLocationList.RemoveAt(i);
                return;
            }
        }
    }

    private void ResetGame()
    {
        StopAllCoroutines();
        kitLocationList.Clear();

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        Destroy(playerObject);

        GameObject[] enemies1 = GameObject.FindGameObjectsWithTag(team1Tag);
        foreach(GameObject enemy1 in enemies1)
        {
            Destroy(enemy1);
        }

        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag(team2Tag);
        foreach(GameObject enemy2 in enemies2)
        {
            Destroy(enemy2);
        }

        GameObject[] kits = GameObject.FindGameObjectsWithTag(firstAidKitObject.tag);
        foreach(GameObject kit in kits)
        {
            Destroy(kit);
        }

        knownEnemies1 = new KnownEnemiesBlackboard();
        knownEnemies2 = new KnownEnemiesBlackboard();

        team1Members = team2Members = numOfEnemies;
        team1Wins = team2Wins = 0;

        popUpObject.SetActive(false);
    }

    private void RandomizeTeamOrder()
    {
        Debug.Log("Team1:");
        for (int i = 0; i < numOfRounds; i++)
        {
            int randNum = Random.Range(0, numOfRounds * 2 - 1);

            while (team1Rounds.Contains(randNum))
            {
                randNum = Random.Range(0, numOfRounds * 2 - 1);
            }

            team1Rounds.Add(randNum);
            Debug.Log(team1Rounds[i]);
        }

        Debug.Log("Team2:");
        for (int i = 0; i < numOfRounds * 2; i++)
        {
            if (!team1Rounds.Contains(i))
            {
                team2Rounds.Add(i);
                Debug.Log(i);
            }
        }
    }

    public void SetPlayerStatus(bool alive)
    {
        playerAlive = alive;
    }
}
