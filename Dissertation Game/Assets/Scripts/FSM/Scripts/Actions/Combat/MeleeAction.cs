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
            Vector3 aiPosition = enemyThinker.transform.position;
            GameObject closestEnemy = enemyThinker.knownEnemiesBlackboard.DetermineTheClosestEnemyObject(aiPosition);

            if (closestEnemy != null)
            {

                EnemyStats enemyStats = enemyThinker.enemyStats;

                enemyThinker.pistolObject.gameObject.SetActive(false);
                enemyThinker.swordObject.gameObject.SetActive(true);

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
}
