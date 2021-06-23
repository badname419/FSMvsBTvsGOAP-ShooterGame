using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
	[SerializeField]
	private float damage;
	private float timePassed;
	private float timeShot;

	[SerializeField] GameObject bulletSpawnPoint;
	[SerializeField] GameObject bullet;

    private void Awake()
    {
		//timePassed = 0f;
		//timeShot = -10f;
    }

    private void Update()
	{
		//timePassed += Time.deltaTime;

	}

	public void Shoot()
	{
			Instantiate(bullet.transform, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
	}
}