using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : GeneralEnemy
{

	public bool seesEnemy = false;

	// Use this for initialization
	void Start()
	{
		//Physics2D.IgnoreCollision (GetComponent<Collider2D>(), GameObject.Find ("Player").GetComponent<Collider2D>());
		health = 300;
		speed = 30;
		strength = 15;
		//regenRate = 3f;
		stamina = 500f;
		maxStamina = 500f;

		minDist = 10f;

		setSpeed(speed);

		player = GameObject.Find("Player").GetComponent<PlayerMovement>();
	}

	public override void passiveRegen()
	{
		stamina += regenRate;
	}

	public override HashSet<KeyValuePair<string, object>> CreateGoalState()
	{
		HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
		goal.Add(new KeyValuePair<string, object>("findPlayer", true));
		goal.Add(new KeyValuePair<string, object>("attackPlayer", true));
		goal.Add(new KeyValuePair<string, object>("stayAlive", true));

		return goal;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{

	}
}