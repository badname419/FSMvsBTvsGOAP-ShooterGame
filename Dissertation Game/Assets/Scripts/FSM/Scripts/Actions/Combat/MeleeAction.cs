using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Melee")]
public class MeleeAction : Action
{
    public override void Act(StateController controller)
    {
        MeleeAttack(controller);
    }

    private void MeleeAttack(StateController controller)
    {
        EnemyThinker enemyThinker = controller.enemyThinker;

        if (enemyThinker.lookingAtTarget)
        {
            EnemyStats enemyStats = enemyThinker.enemyStats;
            Vector3 aiPosition = enemyThinker.transform.position;
            enemyThinker.pistolObject.gameObject.SetActive(false);
            enemyThinker.swordObject.gameObject.SetActive(true);

            GameObject closestEnemy = enemyThinker.knownEnemiesBlackboard.DetermineTheClosestEnemyObject(aiPosition);

            if (closestEnemy.TryGetComponent<PlayerLogic>(out PlayerLogic playerLogic))
            {
                playerLogic.LowerHP(enemyStats.meleeDamage);
            }
            else
            {
                EnemyThinker thinker = closestEnemy.GetComponent<EnemyThinker>();
                thinker.LowerHP(enemyStats.meleeDamage);
            }

            enemyThinker.meleeAttackTime = enemyThinker.timer;
        }
    }
}
