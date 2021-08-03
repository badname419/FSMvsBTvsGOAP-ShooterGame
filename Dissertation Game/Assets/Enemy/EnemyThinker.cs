using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public Pathfinding pathfinding;
    [HideInInspector] public KnownEnemiesBlackboard knownEnemies;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public float distanceToEnemy;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public float lastShotTime;
    [HideInInspector] public float currentHP;

    #region Combat
    [HideInInspector] public bool inCombat;
    [HideInInspector] public float combatStartTime;
    #endregion

    #region Chasing and searching
    [HideInInspector] public Transform closestEnemy;
    [HideInInspector] public int closestEnemyIndex;
    [HideInInspector] public Vector3 walkingTarget;
    [HideInInspector] public Vector3 lastKnownEnemyLoc;
    [HideInInspector] public List<Transform> searchPoints;
    [HideInInspector] public List<int> randomizedRoute;
    [HideInInspector] public int maximumSearchPoints;
    [HideInInspector] public int currentSearchPoint;
    #endregion

    #region Dashing
    [HideInInspector] public bool isDashing;
    [HideInInspector] public float dashStartTime;
    [HideInInspector] public float dashEndTime;
    #endregion

    #region Rotations
    [HideInInspector] public int numOfRotations;            //How many times the agent has rotated so far
    [HideInInspector] public int totalRotations;            //The maximum number of rotations the agent should conduct. Default 5 entails looking right and left.   
    [HideInInspector] public Vector3 forwardRotationTarget; //What forward point the agent should point when rotating
    [HideInInspector] public Vector3 rightRotationTarget;   //What point to the right of the agent they should rotate towards
    [HideInInspector] public Vector3 leftRotationTarget;    //What point to the left of the agent they should rotate towards
    [HideInInspector] public Vector3[] targetArray;         //And array storying the points the agent should rotate towards
    #endregion

    private Image healthBar;
    public EnemyStats enemyStats;
    public float timer;
    private GameManager gameManager;

    private void Start()
    {
        currentHP = enemyStats.maxHp;
        healthBar = transform.Find("Canvas/HealthBG/HealthBar").GetComponent<Image>();
        searchPoints = new List<Transform>();
        isDashing = false;
        timer = 0f;
        currentSearchPoint = 0;
        maximumSearchPoints = 0;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        searchPoints = gameManager.searchPoints;
        maximumSearchPoints = searchPoints.Count;

        //healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void Setup(Transform spawnPoint, Pathfinding pathfinding, KnownEnemiesBlackboard knownEnemies, List<Transform> searchPoints)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        this.pathfinding = pathfinding;
        this.knownEnemies = knownEnemies;

        this.numOfRotations = 0;
        this.totalRotations = enemyStats.maxRotations;
        this.targetArray = new Vector3[totalRotations];

        //this.searchPoints = searchPoints;
        //maximumSearchPoints = searchPoints.Count;
    }

    public void SetupUI(bool aiActivation, List<Transform> spawnPoints)
    {
        StateController stateController = GetComponent<StateController>();
        if (stateController != null)
        {
            stateController.SetupAI(aiActivation, spawnPoints);
        }
    }

    public void LowerHP(int value)
    {
        currentHP -= value;
        inCombat = true;
        combatStartTime = timer;
        UpdateHPBar();
    }

    public void RestoreHP(int value)
    {
        currentHP += value;

        if(currentHP > enemyStats.maxHp)
        {
            currentHP = enemyStats.maxHp;
        }
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        healthBar.fillAmount = currentHP / enemyStats.maxHp;
    }

    public void SetCombat(bool value)
    {
        inCombat = value;

        if (inCombat)
        {
            combatStartTime = timer;
        }
    }
}
