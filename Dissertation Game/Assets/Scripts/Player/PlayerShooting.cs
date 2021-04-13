using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletSpawnPoint;
    [SerializeField] private float waitTime;

    public GameObject bullet;

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
        Instantiate(bullet.transform, bulletSpawnPoint.transform.position, this.transform.rotation);

    }
}
