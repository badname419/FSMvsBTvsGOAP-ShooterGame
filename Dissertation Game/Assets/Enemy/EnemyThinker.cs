using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public Pathfinding pathfinding;
    public KnownEnemiesBlackboard knownEnemies;
    public EnemyStats enemyStats;

    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public float distanceToEnemy;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public float lastShotTime;

    #region Chasing and searching
    [HideInInspector] public Transform closestEnemy;
    [HideInInspector] public int closestEnemyIndex;
    [HideInInspector] public Vector3 walkingTarget;
    [HideInInspector] public Vector3 lastKnownEnemyLoc;
    #endregion

    #region Rotations
    [HideInInspector] public int numOfRotations;            //How many times the agent has rotated so far
    [HideInInspector] public int totalRotations;            //The maximum number of rotations the agent should conduct. Default 5 entails looking right and left.   
    [HideInInspector] public Vector3 forwardRotationTarget; //What forward point the agent should point when rotating
    [HideInInspector] public Vector3 rightRotationTarget;   //What point to the right of the agent they should rotate towards
    [HideInInspector] public Vector3 leftRotationTarget;    //What point to the left of the agent they should rotate towards
    [HideInInspector] public Vector3[] targetArray;         //And array storying the points the agent should rotate towards
    #endregion

    public void Setup(Transform spawnPoint, Pathfinding pathfinding, KnownEnemiesBlackboard knownEnemies)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        this.pathfinding = pathfinding;
        this.knownEnemies = knownEnemies;

        this.numOfRotations = 0;
        this.totalRotations = enemyStats.maxRotations;
        this.targetArray = new Vector3[totalRotations];
    }

    public void SetupUI(bool aiActivation, List<Transform> spawnPoints)
    {
        StateController stateController = GetComponent<StateController>();
        if (stateController != null)
        {
            stateController.SetupAI(aiActivation, spawnPoints);
        }
    }
}
