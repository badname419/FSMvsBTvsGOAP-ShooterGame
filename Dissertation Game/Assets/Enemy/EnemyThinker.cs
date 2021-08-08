using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public Pathfinding pathfinding;
    [HideInInspector] public KnownEnemiesBlackboard knownEnemiesBlackboard;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Shooting shooting;
    [HideInInspector] public FieldOfView fieldOfView;
    [HideInInspector] public CoverSystem coverSystem;
    [HideInInspector] public SensingSystem sensingSystem;
    [HideInInspector] public Rigidbody rigidBody;

    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public float currentHP;
    [HideInInspector] public float timer;

    #region Prefab elements;
    [HideInInspector] public GameObject swordObject;
    [HideInInspector] public GameObject pistolObject;
    #endregion

    #region Combat
    [HideInInspector] public bool inCombat;
    [HideInInspector] public float combatStartTime;
    [HideInInspector] public float meleeAttackTime;
    [HideInInspector] public float lastShotTime;
    #endregion

    #region Chasing and searching
    [HideInInspector] public Transform closestEnemyTransform;
    [HideInInspector] public GameObject closestEnemyObject;
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
    [HideInInspector] public Vector3 aiRotatingPosition;
    #endregion

    private IEnumerator coroutine;
    private Image healthBar;
    private GameManager gameManager;
    private Transform bestCoverSpot;
    public EnemyStats enemyStats;
    public bool lookingAtTarget;


    private void Awake()
    {
        healthBar = transform.Find("Canvas/HealthBG/HealthBar").GetComponent<Image>();
        swordObject = transform.Find("Sword").gameObject;
        pistolObject = transform.Find("PistolHolder").gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        shooting = GetComponent<Shooting>();
        fieldOfView = GetComponent<FieldOfView>();
        coverSystem = GetComponent<CoverSystem>();
        sensingSystem = GetComponent<SensingSystem>();
        rigidBody = GetComponent<Rigidbody>();
        searchPoints = new List<Transform>();
        searchPoints = gameManager.searchPoints;
        maximumSearchPoints = searchPoints.Count;
        randomizedRoute = new List<int>();
        currentHP = enemyStats.maxHp;
        isDashing = false;
        lookingAtTarget = false;

        timer = 0f;
        dashEndTime = 0f;
        dashStartTime = 0f;
        meleeAttackTime = 0f;
        currentSearchPoint = 0;

        coroutine = HealEverySecond(1.0f);
        StartCoroutine(coroutine);
    }

    private void Start()
    {     
        
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
        this.knownEnemiesBlackboard = knownEnemies;

        this.numOfRotations = 0;
        this.totalRotations = enemyStats.maxRotations;
        this.targetArray = new Vector3[totalRotations];
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

    private IEnumerator HealEverySecond(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            RestoreHP((int)enemyStats.hpPerSecond);
        }
    }

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }
}
