using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ShootNode : Node
{

    private NavMeshAgent agent;
    private EnemyAI ai;
    private Transform target;
    private Shooting shooting;
    private float wait;

    private Vector3 currentVelocity;
    private float smoothDamp;

    private GameObject bulletSpawnPoint;
    private GameObject bullet;

    public ShootNode(NavMeshAgent agent, EnemyAI ai, Transform target, GameObject bulletSpawnPoint, GameObject bullet, float wait, GameObject gameObject)
    {
        this.agent = agent;
        this.ai = ai;
        this.target = target;
        this.bulletSpawnPoint = bulletSpawnPoint;
        this.bullet = bullet;
        this.wait = wait;
        shooting = gameObject.AddComponent<Shooting>();
        //shooting = new Shooting();
        smoothDamp = 1f;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        ai.SetColor(Color.green);
        Vector3 direction = target.position - ai.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        ai.transform.rotation = rotation;

        shooting.Shoot(bulletSpawnPoint, bullet, wait, ai);

        return NodeState.RUNNING;
    }

}
