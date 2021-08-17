using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public int maxNumOfEnemies = 1;
    public int numOfRounds = 1;
    public float roundDuration = 90f;
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

    [Header("Round Options")]
    [SerializeField] public bool pve;
    private int team1Members;
    private int team2Members;
    private int roundsPlayed;
    private bool playerAlive;
    [SerializeField] public bool randomizedEnemies;
    [SerializeField] public bool BT;


    [Header("Pop-up Options")]
    public Canvas popUpCanvas;
    private Text[] popUpTextArray;
    private GameObject popUpObject;
    private Button popUpButton;
    private bool victory;
    public string victoryText;
    public string defeatText;
    public string infoText;
    private bool paused;

    //Rounds
    List<int> team1Rounds;
    List<int> team2Rounds;
    int team1Wins;
    int team2Wins;
    private float timer;
    private bool swapped;
    private int firstDifficultyThreshold;
    private int secondDifficultyThreshold;
    private int numOfThresholds = 2;
    private int enemiesThisRound;
    private Text roundText;


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
        roundText = GameObject.Find("Round Display").GetComponent<Text>();
        paused = true;
        team1Members = team2Members = maxNumOfEnemies;
        swapped = false;
        
        if (pve)
        {
            if (randomizedEnemies)
            {
                numOfRounds *= 2;
            }
            firstDifficultyThreshold = numOfRounds / 3;
            secondDifficultyThreshold = (numOfRounds / 3) * 2;
            RandomizeTeamOrder();
        }

        StartGame();
    }

    private void Start()
    {

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (roundsPlayed < numOfRounds)
        {
            if (CheckIfRoundFinished())
            {
                if (pve)
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
                else
                {
                    UpdateScore();
                    if ((roundsPlayed + 1) != numOfRounds)
                    {
                        StartNextRound();
                    }
                    else
                    {
                        DisplaySummary();
                    }
                }
            }
            else if(timer > roundDuration)
            {
                roundsPlayed--;
                StartNextRound();
            }
        }
    }

    private void StartNextRound()
    {
        ResetGame();
        roundsPlayed++;
        if(roundsPlayed == numOfRounds / 2 && swapped == false && !pve)
        {
            SwapSpawnPoints();
        } 
        if (roundsPlayed < numOfRounds)
        {
            StartGame();
        }
    }

    private void UpdateScore(bool victory, bool team1Round)
    {
        if (!paused)
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
            paused = true;
        }
    }

    private void UpdateScore()
    {
        if (!paused)
        {
            if (team1Members == 0)
            {
                team2Wins++;
            }
            else
            {
                team1Wins++;
            }
            paused = true;
        }
    }

    private void DisplaySummary()
    {
        popUpObject.SetActive(true);
        roundsPlayed++;
        popUpTextArray[0].text = "Summary!";

        if (pve)
        {
            popUpTextArray[1].text = "You have played against Behavior-Tree-driven enemies " + numOfRounds / 2 + " times and won " + team1Wins + " times.\n\n" +
                "You have played against Finite-State Machine-driven enemies " + numOfRounds / 2 + " times and won " + team2Wins + " times.";
            popUpTextArray[2].text = "Main Menu";
            popUpButton.onClick.AddListener(LoadMainMenu);
        }
        else
        {
            popUpTextArray[1].text = "Behavior-Tree won " + team2Wins + "/" + numOfRounds + " times.\n\n" +
                "Finite-State Machine won " + team1Wins + "/" + numOfRounds + " times.";
            popUpTextArray[2].text = "Main Menu";
            popUpButton.onClick.AddListener(LoadMainMenu);
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
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
        UpdateRoundText();
        knownEnemies1 = new KnownEnemiesBlackboard();
        knownEnemies2 = new KnownEnemiesBlackboard();
        paused = false;
        timer = 0f;

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
            if (IsKitLocationValid(grid, posX, posZ))
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
            GameObject enemyPrefab = new GameObject();
            if (randomizedEnemies)
            {
                enemyPrefab = team1Rounds.Contains(roundsPlayed) ? BTPrefab : FSMPrefab;
            }
            else
            {
                enemyPrefab = BT ? BTPrefab : FSMPrefab;
            }

            if(roundsPlayed < firstDifficultyThreshold)
            {
                enemiesThisRound = maxNumOfEnemies - 2;
            }
            else
            {
                if(roundsPlayed < secondDifficultyThreshold)
                {
                    enemiesThisRound = maxNumOfEnemies - 1;
                }
                else
                {
                    enemiesThisRound = maxNumOfEnemies;
                }
            }
            team1Members = enemiesThisRound;
            for (int i = 0; i < enemiesThisRound; i++)
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
            for (int i = 0; i < maxNumOfEnemies; i++)
            {
                int pointIndex = i % spawnPoints1.Count;
                GameObject enemy = Instantiate(team2);
                EnemyThinker enemyThinker = enemy.GetComponent<EnemyThinker>();
                enemyThinker.Setup(spawnPoints1[pointIndex], pathfinding, knownEnemies1, searchPoints, 0);
            }

            //Second team
            for (int i = 0; i < maxNumOfEnemies; i++)
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
            if (pve && roundsPlayed < numOfRounds / 2 && team1Members == maxNumOfEnemies)
            {
                team1Members--;
            }
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

        team1Members = team2Members = enemiesThisRound;

        popUpObject.SetActive(false);
        timer = 0f;
    }

    private void RandomizeTeamOrder()
    {
        int roundsPerTeam = numOfRounds / 2;
        int numOfStages = numOfThresholds + 1;
        for (int i = 0; i < roundsPerTeam; i++)
        {
            int randNum = 0;
            if (i < roundsPerTeam / numOfStages)
            {
                randNum = Random.Range(0, numOfRounds / numOfStages - 1);
            }
            else if(i < roundsPerTeam / numOfStages * 2)
            {
                randNum = Random.Range(numOfRounds / numOfStages, numOfRounds / numOfStages * 2 - 1);
            }
            else
            {
                randNum = Random.Range(numOfRounds / numOfStages * 2, numOfRounds - 1);
            }

            while (team1Rounds.Contains(randNum))
            {
                if (i < roundsPerTeam / numOfStages)
                {
                    randNum = Random.Range(0, numOfRounds / numOfStages - 1);
                }
                else if (i < roundsPerTeam / numOfStages * 2)
                {
                    randNum = Random.Range(numOfRounds / numOfStages, numOfRounds / numOfStages * 2 - 1);
                }
                else
                {
                    randNum = Random.Range(numOfRounds / numOfStages * 2, numOfRounds - 1);
                }
            }

            team1Rounds.Add(randNum);
        }

        for (int i = 0; i < numOfRounds; i++)
        {
            if (!team1Rounds.Contains(i))
            {
                team2Rounds.Add(i);
            }
        }
    }

    public void SetPlayerStatus(bool alive)
    {
        playerAlive = alive;
    }

    private void SwapSpawnPoints()
    {
        List<Transform> bufferList = new List<Transform>();
        foreach(Transform spawnPoint in spawnPoints1)
        {
            bufferList.Add(spawnPoint);
        }

        spawnPoints1.Clear();
        foreach(Transform spawnPoint in spawnPoints2)
        {
            spawnPoints1.Add(spawnPoint);
        }

        spawnPoints2.Clear();
        foreach(Transform spawnPoint in bufferList)
        {
            spawnPoints2.Add(spawnPoint);
        }
    }

    public void UpdateRoundText()
    {
        roundText.text = "Rounds: " + (roundsPlayed + 1).ToString() + "/" + numOfRounds;
    }
}
