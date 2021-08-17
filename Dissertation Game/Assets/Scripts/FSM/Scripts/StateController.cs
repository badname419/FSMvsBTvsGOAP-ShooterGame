using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Complete;

public class StateController : MonoBehaviour
{
    [HideInInspector] public EnemyThinker enemyThinker;
    [HideInInspector] public EnemyStats enemyStats;

    public State currentState;
    public State remainState;

    public enum Target { Enemy, Kit, Cover, SearchPoint, Around };
    public Target walkingTargetEnum;

    void Awake()
    {
        enemyThinker = GetComponent<EnemyThinker>();
        enemyStats = enemyThinker.enemyStats;
    }

    void Update()
    {
        currentState.UpdateState(this);

        if (enemyThinker.swordObject.activeSelf)
        {
            if (enemyThinker.timer - enemyThinker.meleeAttackTime > enemyStats.meleeAttackDuration)
            {
                enemyThinker.swordObject.SetActive(false);
                enemyThinker.pistolObject.SetActive(true);
            }
        }

        //enemyThinker.logWriting.AddLine("State: " + currentState.ToString());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(enemyThinker.knownEnemiesBlackboard.GetClosestPreviousPosition(transform.position), new Vector3(1f, 1f, 1f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(enemyThinker.knownEnemiesBlackboard.GetClosestCurrentPosition(transform.position), new Vector3(1f, 1f, 1f));
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            //GetComponent<Renderer>().material.SetColor("_Color", currentState.sceneGizmoColor);
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
        enemyThinker.navMeshAgent.isStopped = true;
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