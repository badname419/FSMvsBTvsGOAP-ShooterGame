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
    private Transform bestCoverSpot;
    private NavMeshAgent agent;

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

        ConstructBehaviourTree();
    }

    private void Start()
    {
        _currentHealth = enemyStats.maxHp;
        knownEnemiesBlackboard = enemyThinker.knownEnemies;    
    }

    private void ConstructBehaviourTree()
    {
        
        
        HealthThresholdNode hpThresholdNode = new HealthThresholdNode(this, enemyStats.hpThreshold);
        LessHealthThanEnemyNode lessHealthNode = new LessHealthThanEnemyNode(this);
        IsCoveredNode isCoveredNode = new IsCoveredNode(this);
        GoToLastKnownPositionNode goToLastPositionNode = new GoToLastKnownPositionNode(agent, this);
        RangeNode chasingRangeNode = new RangeNode(enemyStats.viewRadius, this, Target.Enemy);
        RangeNode shootingRangeNode = new RangeNode(enemyStats.shootingRange, this, Target.Enemy);
        ShootNode shootNode = new ShootNode(agent, this, enemyStats.shootingWaitTime, gameObject);
        IsEnemyVisibleNode enemyVisibleNode = new IsEnemyVisibleNode(this);
        IsLastEnemyPositionKnownNode lastKnownPositionNode = new IsLastEnemyPositionKnownNode(gameObject);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, goToLastPositionNode });
        Sequence shootSequence = new Sequence(new List<Node> { enemyVisibleNode, shootingRangeNode, shootNode });

        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(this);
        GoToNode goToCoverNode = new GoToNode(this, Target.Cover);
        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvailableNode, goToCoverNode });

        RangeNode kitInRangeNode = new RangeNode(enemyStats.kitGrabingRange, this, Target.Kit);
        GoToNode goToKit = new GoToNode(this, Target.Kit);
        Sequence grabKitSequence = new Sequence(new List<Node> { kitInRangeNode, goToKit });

        Sequence isCoveredSequence = new Sequence(new List<Node> { isCoveredNode, shootSequence});

        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, shootSequence });

        Selector tryToTakeCover = new Selector(new List<Node> { isCoveredSequence, findCoverSelector});

        Selector tryToHealSelector = new Selector(new List<Node> {grabKitSequence, tryToTakeCover });

        Sequence healSequence = new Sequence(new List<Node> { hpThresholdNode, lessHealthNode, tryToHealSelector});
        Selector seesEnemySelector = new Selector(new List<Node> { healSequence, shootSequence, chaseSequence});

        Sequence seesEnemySequence = new Sequence(new List<Node> { enemyVisibleNode, seesEnemySelector});
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

        topNode = new Selector(new List<Node> { /*mainCoverSequence,*/ seesEnemySequence /*shootSequence, chaseSequence/*, searchForEnemySelector*/});
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
