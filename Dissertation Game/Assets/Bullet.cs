using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float bulletVelocity = 10f;
    [SerializeField] private float maxDistance;

    private GameObject triggeringEnemy;
    private Rigidbody rigidbody;
    private float damage;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = transform.forward * bulletVelocity;
        //transform.Translate(Vector3.forward * Time.deltaTime * bulletVelocity);

        maxDistance += 1 * Time.deltaTime;

        if(maxDistance >= 5)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            triggeringEnemy = other.gameObject;
            triggeringEnemy.GetComponent<EnemyAI>().TakeDamage(damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        Destroy(this.gameObject);
    }
}
