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
    [HideInInspector] public Shooting enemyShooting;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public EnemyThinker enemyThinker;

    
    private bool aiActive;


    void Awake()
    {
        enemyShooting = GetComponent<Shooting>();
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
            GetComponent<Renderer>().material.SetColor("_Color", currentState.sceneGizmoColor);
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        enemyThinker.stateTimeElapsed += Time.deltaTime;
        return (enemyThinker.stateTimeElapsed >= duration);
    }

    public bool CheckIfPeriodElapsed(float lastOccurance, float duration)
    {
        enemyThinker.stateTimeElapsed += Time.deltaTime;
        return (enemyThinker.stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        navMeshAgent.isStopped = true;
        if(enemyThinker.forwardRotationTarget != Vector3.zero)
        {
            enemyThinker.forwardRotationTarget = Vector3.zero;
            enemyThinker.leftRotationTarget = Vector3.zero;
            enemyThinker.rightRotationTarget = Vector3.zero;
            enemyThinker.numOfRotations = 0;
        }
        enemyThinker.stateTimeElapsed = 0;
    }
}