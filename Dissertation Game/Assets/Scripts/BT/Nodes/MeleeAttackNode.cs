using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackNode : Node
{
    private EnemyThinker enemyThinker;
    private EnemyStats enemyStats;

    public MeleeAttackNode(EnemyThinker enemyThinker)
    {
        this.enemyThinker = enemyThinker;
        this.enemyStats = enemyThinker.enemyStats;
    }

    public override NodeState Evaluate()
    {
        Vector3 aiPosition = enemyThinker.transform.position;
        enemyThinker.pistolObject.gameObject.SetActive(false);
        enemyThinker.swordObject.gameObject.SetActive(true);

        GameObject closestEnemy = enemyThinker.knownEnemiesBlackboard.DetermineTheClosestEnemyObject(aiPosition);

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
