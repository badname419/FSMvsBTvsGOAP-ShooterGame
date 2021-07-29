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

    public ShootNode(NavMeshAgent agent, EnemyAI ai, float wait, GameObject gameObject)
    {
        this.agent = agent;
        this.ai = ai;
        this.wait = wait;
        shooting = gameObject.GetComponent<Shooting>();
        //shooting = new Shooting();
        smoothDamp = 1f;
    }

    public override NodeState Evaluate()
    {
        Vector3 target = ai.fieldOfView.lastKnownEnemyPosition;
        agent.isStopped = true;
        ai.SetColor(Color.green);
        Vector3 direction = target - ai.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        ai.transform.rotation = rotation;

        shooting.Shoot(wait);
        //shooting.Shoot(bulletSpawnPoint, bullet, wait, ai);

        return NodeState.RUNNING;
    }

}
