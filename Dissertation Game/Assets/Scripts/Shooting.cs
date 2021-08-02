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
				ai.LowerHP((int)damage);
			}
		}
	}

	public void Shoot(float wait, int damage, Transform transform)
    {
		if (timePassed - wait >= timeShot)
		{
			Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
			Vector3 spawnPosition = bulletSpawnPoint.transform.TransformPoint(offset);
			Transform bulletTransform = Instantiate(bullet.transform, spawnPosition, this.transform.rotation);
			Bullet bulletScript = bulletTransform.GetComponent<Bullet>();
			bulletScript.SetBulletDamage(damage);
			bulletScript.SetBulletOwner(transform);

			timeShot = timePassed;
		}
    }

	public void Shoot(GameObject bulletSpawnPoint, GameObject bullet, float wait, EnemyAI ai)
	{
		if (timePassed - wait >= timeShot)
		{
			Transform bulletObject = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, ai.transform.rotation);
			timeShot = timePassed;
		}
	}
}