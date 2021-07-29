using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletSpawnPoint;
    [SerializeField] private float waitTime;

    public GameObject bullet;
    public float xOffset;
    public float yOffset;
    public float zOffset;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
        Vector3 spawnPosition = bulletSpawnPoint.transform.TransformPoint(offset);
        Instantiate(bullet.transform, spawnPosition, this.transform.rotation);

    }
}
