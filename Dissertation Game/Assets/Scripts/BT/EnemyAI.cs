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

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] availableCovers;

    public List<Transform> visibleEnemies = new List<Transform>();

    private Material material;
    public Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;

    private float _currentHealth;
    public enum Target { Enemy, Kit};

    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, enemyStats.maxHp); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponent<MeshRenderer>().material;
        enemyThinker = GetComponent<EnemyThinker>();
        enemyStats = enemyThinker.enemyStats;
        coverSystem = GetComponent<CoverSystem>();
        fieldOfView = GetComponent<FieldOfView>();

        ConstructBehaviourTree();
    }

    private void Start()
    {
        _currentHealth = enemyStats.maxHp;
        knownEnemiesBlackboard = enemyThinker.knownEnemies;    
    }

    private void ConstructBehaviourTree()
    {
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthThresholdNode hpThresholdNode = new HealthThresholdNode(this, enemyStats.hpThreshold);
        LessHealthThanEnemyNode lessHealthNode = new LessHealthThanEnemyNode(this);
        IsCoveredNode isCoveredNode = new IsCoveredNode(this);
        GoToLastKnownPositionNode goToLastPositionNode = new GoToLastKnownPositionNode(agent, this);
        RangeNode chasingRangeNode = new RangeNode(enemyStats.viewRadius, this, (int)Target.Enemy);
        RangeNode shootingRangeNode = new RangeNode(enemyStats.shootingRange, this, (int)Target.Enemy);
        ShootNode shootNode = new ShootNode(agent, this, enemyStats.shootingWaitTime, gameObject);
        IsEnemyVisibleNode enemyVisibleNode = new IsEnemyVisibleNode(this);
        IsLastEnemyPositionKnownNode lastKnownPositionNode = new IsLastEnemyPositionKnownNode(gameObject);

        RangeNode kitInRangeNode = new RangeNode(enemyStats.kitGrabingRange, this, (int)Target.Kit);

        Sequence grabAKit = new Sequence(new List<Node> {kitInRangeNode }); //

        Selector tryToHeal = new Selector(new List<Node> { }); //

        //Sequence healSequence = new Sequence(new List<Node> { hpThresholdNode, lessHealthNode, })

        Selector seeEnemySelector = new Selector(new List<Node> { });

        Sequence chaseSequence = new Sequence(new List<Node> { enemyVisibleNode, chasingRangeNode, goToLastPositionNode });
        Sequence shootSequence = new Sequence(new List<Node> { enemyVisibleNode, shootingRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvailableNode, goToCoverNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { hpThresholdNode, tryToTakeCoverSelector });

        Sequence isLastPositionKnown = new Sequence(new List<Node> { lastKnownPositionNode, goToLastPositionNode });
        Selector searchForEnemySelector = new Selector(new List<Node> { isLastPositionKnown});

        topNode = new Selector(new List<Node> { mainCoverSequence, shootSequence/*, /*chaseSequence/*, searchForEnemySelector*/});
    }

    private void Update()
    {
        topNode.Evaluate();
        if(topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            agent.isStopped = true;
        }
        currentHealth += Time.deltaTime * enemyStats.hpRestoreRate;
    }

    private void OnMouseDown()
    {
        currentHealth -= 10f;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
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
}
