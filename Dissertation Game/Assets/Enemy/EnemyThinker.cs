using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public Pathfinding pathfinding;
    public KnownEnemiesBlackboard knownEnemies;
    public EnemyStats enemyStats; 

    public void Setup(Transform spawnPoint, Pathfinding pathfinding, KnownEnemiesBlackboard knownEnemies)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        this.pathfinding = pathfinding;
        this.knownEnemies = knownEnemies;
    }

    public void SetupUI(bool aiActivation, List<Transform> spawnPoints)
    {
        StateController stateController = GetComponent<StateController>();
        stateController.SetupAI(aiActivation, spawnPoints);
    }
}
