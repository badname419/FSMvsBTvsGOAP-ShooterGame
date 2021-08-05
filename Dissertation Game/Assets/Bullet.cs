using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string bulletTag = "Bullet";

    [SerializeField] private float bulletVelocity = 10f;
    [SerializeField] private float maxDistance;

    private GameObject triggeringEnemy;
    private Rigidbody rigidbody;
    private int damage;
    private Transform bulletOwner;

    //Debug
    private Vector3 hitPoint;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        hitPoint = Vector3.zero;
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
        Debug.Log("Hit1");
        if(other.tag == "Enemy")
        {
            Debug.Log("HIT");
            triggeringEnemy = other.gameObject;
            triggeringEnemy.GetComponent<EnemyThinker>().LowerHP(damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string collisionTag = collision.transform.tag;

        if (!collisionTag.Equals(bulletOwner.tag))
        {
            if (collisionTag.Equals(enemyTag))
            {
                triggeringEnemy = collision.collider.gameObject;
                triggeringEnemy.GetComponent<EnemyThinker>().LowerHP(damage);
                triggeringEnemy.GetComponent<SensingSystem>().RegisterHit(bulletOwner);
            }
            else if (collisionTag.Equals(playerTag))
            {
                triggeringEnemy = collision.collider.gameObject;
                triggeringEnemy.GetComponent<PlayerLogic>().LowerHP(damage);
            }
        }
        Destroy(this.gameObject);
    }

    public void SetBulletDamage(int value)
    {
        damage = value;
    }

    public void SetBulletOwner(Transform transform)
    {
        bulletOwner = transform;
    }
}
