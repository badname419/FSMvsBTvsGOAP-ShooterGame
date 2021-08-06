using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackNode : Node
{
    private EnemyAI ai;
    private EnemyThinker enemyThinker;
    private KnownEnemiesBlackboard knownEnemiesBlackboard;
    private EnemyStats enemyStats;

    public MeleeAttackNode(EnemyAI ai)
    {
        this.ai = ai;
        this.enemyThinker = ai.enemyThinker;
        this.knownEnemiesBlackboard = ai.knownEnemiesBlackboard;
        this.enemyStats = ai.enemyStats;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = ai.transform.position;
        enemyThinker.pistolObject.gameObject.SetActive(false);
        enemyThinker.swordObject.gameObject.SetActive(true);

        GameObject closestEnemy = knownEnemiesBlackboard.DetermineTheClosestEnemyObject(aiPosition);

        PlayerLogic playerLogic = closestEnemy.GetComponent<PlayerLogic>();

        if(playerLogic != null)
        {
            playerLogic.LowerHP(enemyStats.meleeDamage);
        }
        else
        {
            EnemyThinker thinker = closestEnemy.GetComponent<EnemyThinker>();
            thinker.LowerHP(enemyStats.meleeDamage);
        }

        enemyThinker.meleeAttackTime = enemyThinker.timer;
        return NodeState.SUCCESS;
    }

}
