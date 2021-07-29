using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
	[SerializeField] private LayerMask mask;

	[SerializeField] private GameObject bulletSpawnPoint;
	[SerializeField] private GameObject bullet;

	[SerializeField] private float damage;

	[SerializeField] private float xOffset;
	[SerializeField] private float yOffset;
	[SerializeField] private float zOffset;

	private float timePassed;
	private float timeShot;

	private void Awake()
    {
		timePassed = 0f;
		timeShot = -10f;
    }

    private void Update()
	{
		timePassed += Time.deltaTime;
	}

	private void Shoot()
    {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, mask))
		{
			EnemyAI ai = hit.collider.GetComponent<EnemyAI>();
			if (ai != null)
			{
				ai.TakeDamage(damage);
			}
		}
	}

	public void Shoot(float wait)
    {
		if (timePassed - wait >= timeShot)
		{
			Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
			Vector3 spawnPosition = bulletSpawnPoint.transform.TransformPoint(offset);
			Instantiate(bullet.transform, spawnPosition, this.transform.rotation);

			timeShot = timePassed;
		}
    }

	public void Shoot(GameObject bulletSpawnPoint, GameObject bullet, float wait, EnemyAI ai)
	{
		if (timePassed - wait >= timeShot)
		{
			Instantiate(bullet.transform, bulletSpawnPoint.transform.position, ai.transform.rotation);
			timeShot = timePassed;
		}
	}
}