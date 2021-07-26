using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public abstract class GeneralEnemy : MonoBehaviour, IGOAP
{
	public PlayerMovement player;
	public EnemyStats enemyStats;

	private EnemyThinker enemyThinker;
	private NavMeshAgent navMeshAgent;
	private List<Transform> visibleEnemiesList;

	public bool interrupt = false;

	public int health;
	public int strength;
	public int speed;
	public float stamina;
	public float regenRate;
	protected float terminalSpeed;
	protected float initialSpeed;
	protected float acceleration;
	protected float minDist = 1.5f;
	protected float aggroDist = 5f;
	protected bool loop = false;
	protected float maxStamina;

	// Use this for initialization

	void Awake()
    {
		enemyThinker = GetComponent<EnemyThinker>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		visibleEnemiesList = new List<Transform>();
	}
	void Start()
	{
		
	}

	// Update is called once per frame
	public virtual void Update()
	{
		if (stamina <= maxStamina)
		{
			Invoke("passiveRegen", 1.0f);
		}
		else
		{
			stamina = maxStamina;
		}
	}

	public abstract void passiveRegen();

	public HashSet<KeyValuePair<string, object>> GetWorldState()
	{
		HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
		worldData.Add(new KeyValuePair<string, object>("attackPlayer", false)); //to-do: change player's state for world data here
		worldData.Add(new KeyValuePair<string, object>("seePlayer", false)); //to-do: change player's state for world data here
		worldData.Add(new KeyValuePair<string, object>("inShootingRange", false)); //to-do: change player's state for world data here
		worldData.Add(new KeyValuePair<string, object>("evadePlayer", false));
		return worldData;
	}

	public abstract HashSet<KeyValuePair<string, object>> CreateGoalState();

	public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
	{

	}

	public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> action)
	{

	}

	public void AllActionsFinished()
	{

	}

	public void AbortPlan(GOAPAction failedAction)
	{
		GetComponent<GOAPAgent>().GetDataProviderInterface().AllActionsFinished();
		failedAction.ResetGA();
		failedAction.ResetAction();
	}

	public void setSpeed(float val)
	{
		terminalSpeed = val / 10;
		initialSpeed = (val / 10) / 2;
		acceleration = (val / 10) / 4;
		return;
	}

	public virtual bool MoveAgentToAction(GOAPAction nextAction)
	{
		float dist = Vector3.Distance(transform.position, nextAction.target);

		if(dist < enemyStats.aggroDistance)
        {
			GetComponent<NavMeshAgent>().isStopped = false;
			GetComponent<NavMeshAgent>().SetDestination(nextAction.target);//Let the nav mesh do the work
			Vector3 v3LookDirection = nextAction.target - transform.position;//Look at the target
			v3LookDirection.y = 0;
			Quaternion qRotation = Quaternion.LookRotation(v3LookDirection);
			transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, 0.005f);
		}

		if (interrupt)
		{
			GetComponent<GOAPAgent>().GetDataProviderInterface().AbortPlan(nextAction);

			AbortPlan(nextAction);
			interrupt = false;

			return true;
		}

		if (dist <= enemyStats.arrivalDistance)//If I have arrived
		{
			nextAction.SetInRange(true);
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool LookForEnemies()
    {
		Vector3 position = transform.position;
		Collider[] enemiesInViewRadius = Physics.OverlapSphere(position, enemyStats.viewRadius, enemyStats.enemyLayer);

		visibleEnemiesList.Clear();

		for (int i = 0; i < enemiesInViewRadius.Length; i++)
		{
			Transform enemy = enemiesInViewRadius[i].transform;
			Vector3 dirToEnemy = (enemy.position - position).normalized;
			if (Vector3.Angle(transform.forward, dirToEnemy) < enemyStats.viewAngle / 2)
			{
				float distToEnemy = Vector3.Distance(transform.position, enemy.position);

				if (!Physics.Raycast(transform.position, dirToEnemy, distToEnemy, enemyStats.coverMask))
				{
					visibleEnemiesList.Add(enemy);
					enemyThinker.knownEnemies.UpdateEnemyList(enemy);
				}
			}
		}

		if (visibleEnemiesList.Count != 0)
		{
			//controller.closestEnemy = ChooseTarget(controller, visibleEnemiesList);
			//controller.walkingTarget = controller.closestEnemy.position;
			return true;
		}
		else
		{
			return false;
		}
	}
}
