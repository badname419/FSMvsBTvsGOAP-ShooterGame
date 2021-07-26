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
    public float targetProximityThreshold;


    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public EnemyShooting enemyShooting;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public EnemyThinker enemyThinker;

    [HideInInspector] public int nextWayPoint;  
    [HideInInspector] public float distanceToEnemy;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public float lastShotTime; 

    // For Chasing and searching
    [HideInInspector] public Transform closestEnemy;
    [HideInInspector] public int closestEnemyIndex;
    [HideInInspector] public Vector3 walkingTarget;
    [HideInInspector] public Vector3 lastKnownEnemyLoc;

    [HideInInspector] public int numOfRotations;
    [HideInInspector] public int totalRotations;
    [HideInInspector] public Vector3 forwardRotationTarget;
    [HideInInspector] public Vector3 rightRotationTarget;
    [HideInInspector] public Vector3 leftRotationTarget;
    [HideInInspector] public Vector3[] targetArray;


    private bool aiActive;


    void Awake()
    {
        enemyShooting = GetComponent<EnemyShooting>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyThinker = GetComponent<EnemyThinker>();
        currentHP = enemyStats.maxHp;
        numOfRotations = 0;
        totalRotations = enemyStats.maxRotations;
        targetArray = new Vector3[totalRotations];
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
            GetComponent<Renderer>().material.SetColor("_Color", currentState.sceneGizmoColor);
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
        if(forwardRotationTarget != Vector3.zero)
        {
            forwardRotationTarget = Vector3.zero;
            leftRotationTarget = Vector3.zero;
            rightRotationTarget = Vector3.zero;
            numOfRotations = 0;
        }  
        stateTimeElapsed = 0;
    }
}