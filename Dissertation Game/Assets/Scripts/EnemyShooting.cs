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
		Vector3 rot = bulletSpawnPoint.transform.rotation.eulerAngles;
		rot = new Vector3(rot.x, rot.y + 90, rot.z);

		Vector3 pos = bulletSpawnPoint.transform.position;
		pos = new Vector3(pos.x, pos.y, pos.z);
		Instantiate(bullet.transform, pos, Quaternion.Euler(rot));
	}
}