using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Complete;

public class StateController : MonoBehaviour
{

    public State currentState;
    public EnemyStats enemyStats;
    public Blackboard enemyBlackboard;
    public GameObject bulletSpawnPoint;
    public State remainState;
    public float currentHP;


    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public EnemyShooting enemyShooting;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform closestEnemy;
    [HideInInspector] public Vector3 lastKnownEnemyLoc;
    [HideInInspector] public float distanceToEnemy;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public EnemyThinker enemyThinker;
    [HideInInspector] public float lastShotTime;

    [HideInInspector] public Vector3 walkingTarget;
    private bool aiActive;


    void Awake()
    {
        enemyShooting = GetComponent<EnemyShooting>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyThinker = GetComponent<EnemyThinker>();
        currentHP = enemyStats.maxHp;
    }

    public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
    {
        //Waypoints list is useless because the tank doesn't patrol
        wayPointList = wayPointsFromTankManager;
        aiActive = aiActivationFromTankManager;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

    void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    void OnDrawGizmos()
    {
        //if (currentState != null && eyes != null)
        //{
            //Gizmos.color = currentState.sceneGizmoColor;
            //Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
        //}
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    public bool CheckIfPeriodElapsed(float lastOccurance, float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed - lastOccurance >= duration);
    }

    private void OnExitState()
    {
        navMeshAgent.isStopped = true;
        stateTimeElapsed = 0;
    }
}