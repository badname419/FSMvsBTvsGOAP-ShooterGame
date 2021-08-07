using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector] public EnemyThinker enemyThinker;
    [HideInInspector] public EnemyStats enemyStats;

    private Material material;
    private Node topNode;

    public GameObject closestEnemyObject;
    public float timer;

    public enum Target { Enemy, Kit, Cover, SearchPoint, Around};
    public enum EventType { Dash, Melee};

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        enemyThinker = GetComponent<EnemyThinker>();
        enemyStats = enemyThinker.enemyStats;
    }

    private void Start()
    {
        ConstructBehaviourTree();
        timer = 0f;  
    }

    private void ConstructBehaviourTree()
    { 
        HealthThresholdNode hpThresholdNode = new HealthThresholdNode(enemyThinker);
        LessHealthThanEnemyNode lessHealthNode = new LessHealthThanEnemyNode(enemyThinker);
        IsCoveredNode isCoveredNode = new IsCoveredNode(enemyThinker);
        GoToLastKnownPositionNode goToLastPositionNode = new GoToLastKnownPositionNode(enemyThinker);
        RangeNode inViewRangeNode = new RangeNode(enemyThinker, enemyStats.viewRadius, Target.Enemy);
        RangeNode shootingRangeNode = new RangeNode(enemyThinker, enemyStats.shootingRange, Target.Enemy);
        ShootNode shootNode = new ShootNode(enemyThinker);
        IsEnemyVisibleNode enemyVisibleNode = new IsEnemyVisibleNode(enemyThinker);
        IsLastEnemyPositionKnownNode lastKnownPositionNode = new IsLastEnemyPositionKnownNode(enemyThinker);
        LookAtNode lookAtEnemyNode = new LookAtNode(enemyThinker, Target.Enemy);
        AnyEnemiesSeenNode anyEnemiesSeenNode = new AnyEnemiesSeenNode(enemyThinker);
        IsDashingNode isDashingNode = new IsDashingNode(enemyThinker);
        GoToNode goToSearchPointNode = new GoToNode(enemyThinker, Target.SearchPoint);
        GoToNode goToLastEnemyLocation = new GoToNode(enemyThinker, Target.Enemy);

        Sequence chaseSequence = new Sequence(new List<Node> { anyEnemiesSeenNode, goToLastEnemyLocation/*, lookAroundSequence*/});
        Sequence shootSequence = new Sequence(new List<Node> { enemyVisibleNode, shootingRangeNode, lookAtEnemyNode, shootNode });

        #region Chasing
        AtRotationPositionNode atRotationPositionNode = new AtRotationPositionNode(enemyThinker);
        //IsAtLastKnownLocationNode atLastKnownLocationNode = new IsAtLastKnownLocationNode(enemyThinker);
        LookAtNode lookAroundNode = new LookAtNode(enemyThinker, Target.Around);
        EstablishLookAroundNode establishLookAroundNode = new EstablishLookAroundNode(enemyThinker);
        IsLookAroundEstablishedNode isLookEstablishedNode = new IsLookAroundEstablishedNode(enemyThinker);
        Selector isLookAroundEstablishedSelector = new Selector(new List<Node> {isLookEstablishedNode, establishLookAroundNode});
        Sequence lookAroundSequence = new Sequence(new List<Node> { isLookAroundEstablishedSelector, lookAroundNode});

        Selector chaseSelector = new Selector(new List<Node> {goToLastEnemyLocation, lookAroundSequence });
       
        Selector shouldChaseSelector = new Selector(new List<Node> { anyEnemiesSeenNode, atRotationPositionNode});

        Sequence chaseDecisionSequence = new Sequence(new List<Node> { shouldChaseSelector, chaseSelector});
        #endregion

        #region Attack
        CooldownNode meleeCooldownNode = new CooldownNode(enemyThinker, EventType.Melee);
        RangeNode inMeleeRange = new RangeNode(enemyThinker, enemyStats.meleeRange, Target.Enemy);
        MeleeAttackNode meleeAttackNode = new MeleeAttackNode(enemyThinker);
        Sequence meleeSequence = new Sequence(new List<Node> { inMeleeRange, meleeCooldownNode, enemyVisibleNode, lookAtEnemyNode, meleeAttackNode });

        RangeNode inDashRangeNode = new RangeNode(enemyThinker, enemyStats.dashRange, Target.Enemy);
        CooldownNode dashCooldownNode = new CooldownNode(enemyThinker, EventType.Dash);
        
        DashNode dashNode = new DashNode(enemyThinker, Target.Enemy);

        Sequence dashMeleeSequence = new Sequence(new List<Node> { inDashRangeNode, dashCooldownNode, lookAtEnemyNode, enemyVisibleNode, dashNode});

        Selector meleeDashSelector = new Selector(new List<Node> { meleeSequence, dashMeleeSequence});

        Selector attackSelector = new Selector(new List<Node> { meleeDashSelector, /*grenade*/ shootSequence });
        #endregion

        #region SeesEnemySelector
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(enemyThinker);
        GoToNode goToCoverNode = new GoToNode(enemyThinker, Target.Cover);
        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvailableNode, goToCoverNode });

        RangeNode kitInRangeNode = new RangeNode(enemyThinker, enemyStats.kitDetectionRange, Target.Kit);
        GoToNode goToKit = new GoToNode(enemyThinker, Target.Kit);
        Sequence grabKitSequence = new Sequence(new List<Node> { kitInRangeNode, goToKit });

        Sequence isCoveredSequence = new Sequence(new List<Node> { isCoveredNode, attackSelector });

        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, shootSequence });

        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredSequence, findCoverSelector });

        Selector tryToHealSelector = new Selector(new List<Node> { grabKitSequence, tryToTakeCoverSelector });

        Sequence healSequence = new Sequence(new List<Node> { hpThresholdNode, lessHealthNode, tryToHealSelector });
        Selector combatChoiceSelector = new Selector(new List<Node> {healSequence, attackSelector, chaseDecisionSequence });

        Sequence seesEnemySequence = new Sequence(new List<Node> { enemyVisibleNode, combatChoiceSelector });
        #endregion

        #region Search
        RandomizeSearchRoute randomizeRouteNode = new RandomizeSearchRoute(enemyThinker);
        IsRouteAvailable isRouteAvailableNode = new IsRouteAvailable(enemyThinker);
        Selector establishRouteSelector = new Selector(new List<Node> { isRouteAvailableNode, randomizeRouteNode});
        Sequence randomSearchSequence = new Sequence(new List<Node> { establishRouteSelector, goToSearchPointNode});

        Selector shouldSearchSelector = new Selector(new List<Node> { healSequence, chaseDecisionSequence, randomSearchSequence});

        IsInCombatNode inCombatNode = new IsInCombatNode(enemyThinker);
        IsEnemyAlive enemyAliveNode = new IsEnemyAlive(enemyThinker);
        Sequence inCombatSequence = new Sequence(new List<Node> {inCombatNode, enemyAliveNode, combatChoiceSelector});

        Selector searchSelector = new Selector(new List<Node> { inCombatSequence, shouldSearchSelector});
        #endregion

        topNode = new Selector(new List<Node> { isDashingNode, seesEnemySequence, searchSelector });
    }

    private void Update()
    {
        enemyThinker.navMeshAgent.isStopped = true;
        timer += Time.deltaTime;

        topNode.Evaluate();
        if(topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            enemyThinker.navMeshAgent.isStopped = true;
        }

        if (enemyThinker.swordObject.activeSelf)
        {
            if(enemyThinker.timer - enemyThinker.meleeAttackTime > enemyStats.meleeAttackDuration)
            {
                enemyThinker.swordObject.SetActive(false);
                enemyThinker.pistolObject.SetActive(true);
            }
        }

    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(transform.position), new Vector3(1f, 1f, 1f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(enemyThinker.knownEnemiesBlackboard.GetClosestCurrentPosition(transform.position), new Vector3(1f, 1f, 1f));
    }
}
