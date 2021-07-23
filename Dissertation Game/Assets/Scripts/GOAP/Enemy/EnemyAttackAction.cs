using UnityEngine;
using System.Collections;

public class EnemyAttackAction : GOAPAction
{

	private bool attacked = false;
	private Transform closestVisibleEnemy;
	private float closestDistance;
	private float lastAttack;
	private GOAPAgent goapAgent;

	public EnemyAttackAction()
	{
		AddEffect("attackPlayer", true);
		cost = 100f;
	}

	public override void ResetGA()
	{
		attacked = false;
		target = null;
	}

	public override bool IsActionFinished()
	{
		return attacked;
	}

	public override bool NeedsToBeInRange()
	{
		return true;
	}

	public override bool CheckPrecondition(GameObject agent)
	{
		Enemy currEnemy = agent.GetComponent<Enemy>();
		target = currEnemy.gameObject;
		return LookForEnemies(currEnemy);
	}

	public override bool PerformAction(GameObject agent)
	{
		Debug.Log("Perform attack");
		EnemyShooting shooting = agent.GetComponent<EnemyShooting>();
		goapAgent = agent.GetComponent<GOAPAgent>();
		Enemy currEnemy = agent.GetComponent<Enemy>();
		if (goapAgent.timer - lastAttack >= currEnemy.enemyStats.attackRate)
		{
			lastAttack = goapAgent.timer;
			shooting.Shoot();
			//currWolf.animator.Play("wolfAttack");

			/*
			int damage = currWolf.strength;
			if (currWolf.player.isBlocking)
			{
				damage -= currWolf.player.defense;
			}

			currWolf.player.health -= damage;
			*/
			currEnemy.stamina -= cost;
			
			attacked = true;
			Debug.Log("Perform attack true");
			return true;
		}
		else
		{
			Debug.Log("Perform attack false");
			return false;
		}
	}

	private bool LookForEnemies(Enemy currEnemy)
	{
		Vector3 position = transform.position;
		EnemyStats enemyStats = currEnemy.enemyStats;
		Collider[] enemiesInViewRadius = Physics.OverlapSphere(position, enemyStats.viewRadius, enemyStats.enemyLayer);

		closestVisibleEnemy = null;
		closestDistance = Mathf.Infinity;

		for (int i = 0; i < enemiesInViewRadius.Length; i++)
		{
			Transform enemy = enemiesInViewRadius[i].transform;
			Vector3 dirToEnemy = (enemy.position - position).normalized;
			if (Vector3.Angle(transform.forward, dirToEnemy) < enemyStats.viewAngle / 2)
			{
				float distToEnemy = Vector3.Distance(transform.position, enemy.position);

				if (!Physics.Raycast(transform.position, dirToEnemy, distToEnemy, enemyStats.coverMask))
				{
					if(distToEnemy < closestDistance)
                    {
						closestDistance = distToEnemy;
						closestVisibleEnemy = enemy;
                    }
				}
			}
		}

		return closestVisibleEnemy != null;
	}
}
