using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector] public EnemyThinker enemyThinker;
    [HideInInspector] public EnemyStats enemyStats;
    [HideInInspector] public CoverSystem coverSystem;
    [HideInInspector] public FieldOfView fieldOfView;
    [HideInInspector] public KnownEnemiesBlackboard knownEnemiesBlackboard;
    [HideInInspector] public Shooting shooting;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] availableCovers;

    public List<Transform> visibleEnemies = new List<Transform>();

    private IEnumerator coroutine;
    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;
    private GameManager gameManager;
    public bool isDashing;
    public float dashStartTime;
    public float dashEndTime;
    public float timer;
    public bool inCombat;
    public float combatStartTime;
    public GameObject closestEnemy;

    private Node topNode;

    private float _currentHealth;
    public enum Target { Enemy, Kit, Cover};

    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, enemyStats.maxHp); }
    }

    public Transform BestCoverSpot { get => bestCoverSpot; set => bestCoverSpot = value; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponent<MeshRenderer>().material;
        enemyThinker = GetComponent<EnemyThinker>();
        enemyStats = enemyThinker.enemyStats;
        coverSystem = GetComponent<CoverSystem>();
        fieldOfView = GetComponent<FieldOfView>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        knownEnemiesBlackboard = gameManager.knownEnemies;
        shooting = GetComponent<Shooting>();
    }

    private void Start()
    {
        ConstructBehaviourTree();

        coroutine = HealEverySecond(1.0f);
        StartCoroutine(coroutine);
        _currentHealth = enemyStats.maxHp;
        
        isDashing = false;
        dashStartTime = 0f;
        dashEndTime = 0f;
        timer = 0f;
        inCombat = false;
        combatStartTime = 0f;
        
    }

    private void ConstructBehaviourTree()
    { 
        HealthThresholdNode hpThresholdNode = new HealthThresholdNode(this, enemyStats.hpThreshold);
        LessHealthThanEnemyNode lessHealthNode = new LessHealthThanEnemyNode(this);
        IsCoveredNode isCoveredNode = new IsCoveredNode(this);
        GoToLastKnownPositionNode goToLastPositionNode = new GoToLastKnownPositionNode(this);
        RangeNode inViewRangeNode = new RangeNode(enemyStats.viewRadius, this, Target.Enemy);
        RangeNode shootingRangeNode = new RangeNode(enemyStats.shootingRange, this, Target.Enemy);
        ShootNode shootNode = new ShootNode(this);
        IsEnemyVisibleNode enemyVisibleNode = new IsEnemyVisibleNode(this);
        IsLastEnemyPositionKnownNode lastKnownPositionNode = new IsLastEnemyPositionKnownNode(this);
        LookAtNode lookAtEnemyNode = new LookAtNode(this, Target.Enemy);
        AnyEnemiesSeenNode anyEnemiesSeenNode = new AnyEnemiesSeenNode(this);
        IsDashingNode isDashingNode = new IsDashingNode(this);


        Sequence chaseSequence = new Sequence(new List<Node> { anyEnemiesSeenNode, goToLastPositionNode, lookAtEnemyNode});
        Sequence shootSequence = new Sequence(new List<Node> { enemyVisibleNode, shootingRangeNode, lookAtEnemyNode, shootNode });

        #region Attack
        RangeNode inMeleeRange = new RangeNode(enemyStats.meleeRange, this, Target.Enemy);
        MeleeAttackNode meleeNode = new MeleeAttackNode(agent, this, enemyStats.shootingWaitTime, gameObject);
        Sequence meleeSequence = new Sequence(new List<Node> { inMeleeRange, meleeNode });

        RangeNode inDashRangeNode = new RangeNode(enemyStats.dashRange, this, Target.Enemy);
        DashCooldownNode dashCooldownNode = new DashCooldownNode(this);
        DashNode dashNode = new DashNode(this, Target.Enemy);

        Sequence dashMeleeSequence = new Sequence(new List<Node> { inDashRangeNode, dashCooldownNode, lookAtEnemyNode, enemyVisibleNode, dashNode, meleeSequence });

        Selector attackSelector = new Selector(new List<Node> { dashMeleeSequence, /*grenade*/ shootSequence });
        #endregion

        #region SeesEnemySelector
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(this);
        GoToNode goToCoverNode = new GoToNode(this, Target.Cover);
        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvailableNode, goToCoverNode });

        RangeNode kitInRangeNode = new RangeNode(enemyStats.kitGrabingRange, this, Target.Kit);
        GoToNode goToKit = new GoToNode(this, Target.Kit);
        Sequence grabKitSequence = new Sequence(new List<Node> { kitInRangeNode, goToKit });

        Sequence isCoveredSequence = new Sequence(new List<Node> { isCoveredNode, attackSelector });

        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, shootSequence });

        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredSequence, findCoverSelector });

        Selector tryToHealSelector = new Selector(new List<Node> { grabKitSequence, tryToTakeCoverSelector });

        Sequence healSequence = new Sequence(new List<Node> { hpThresholdNode, lessHealthNode, tryToHealSelector });
        Selector combatChoiceSelector = new Selector(new List<Node> {healSequence, attackSelector, chaseSequence });

        Sequence seesEnemySequence = new Sequence(new List<Node> { enemyVisibleNode, combatChoiceSelector });
        #endregion

        #region Search
        Selector shouldSearchSelector = new Selector(new List<Node> { healSequence, chaseSequence});

        IsInCombatNode inCombatNode = new IsInCombatNode(this);
        IsEnemyAlive enemyAliveNode = new IsEnemyAlive(this);
        Sequence inCombatSequence = new Sequence(new List<Node> {inCombatNode, enemyAliveNode, combatChoiceSelector});

        Selector searchSelector = new Selector(new List<Node> { inCombatSequence, shouldSearchSelector});
        #endregion

        //RangeNode kitInRangeNode = new RangeNode(enemyStats.kitGrabingRange, this, (int)Target.Kit);

        //Sequence grabAKit = new Sequence(new List<Node> {kitInRangeNode }); //

        //Selector tryToHeal = new Selector(new List<Node> { }); //

        //Sequence healSequence = new Sequence(new List<Node> { hpThresholdNode, lessHealthNode, })

        //Selector seeEnemySelector = new Selector(new List<Node> { });




        //Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
        //Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        //Sequence mainCoverSequence = new Sequence(new List<Node> { hpThresholdNode, tryToTakeCoverSelector });

        //Sequence isLastPositionKnown = new Sequence(new List<Node> { lastKnownPositionNode, goToLastPositionNode });
        //Selector searchForEnemySelector = new Selector(new List<Node> { isLastPositionKnown});

        topNode = new Selector(new List<Node> { /*mainCoverSequence,*/isDashingNode, seesEnemySequence, searchSelector /*shootSequence, chaseSequence/*, searchForEnemySelector*/});
    }

    private void Update()
    {
        timer += Time.deltaTime;

        topNode.Evaluate();
        if(topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            agent.isStopped = true;
        }

        Debug.Log(enemyThinker.isDashing);
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }

    public void SetCombat(bool inCombat)
    {
        this.inCombat = inCombat;
        if (inCombat)
        {
            combatStartTime = timer;
        }
    }

    //To be removed
    public void LowerHP(int value)
    {
        currentHealth -= value;
    }

    //To be removed
    private void RestoreHP(int value)
    {
        enemyThinker.currentHP += value;
        if(currentHealth > enemyStats.maxHp)
        {
            currentHealth = enemyStats.maxHp;
        }
    }

    private IEnumerator HealEverySecond(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            enemyThinker.RestoreHP((int)enemyStats.hpPerSecond);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(knownEnemiesBlackboard.GetClosestPreviousPosition(transform.position), new Vector3(1f, 1f, 1f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(knownEnemiesBlackboard.GetClosestCurrentPosition(transform.position), new Vector3(1f, 1f, 1f));
    }
}
