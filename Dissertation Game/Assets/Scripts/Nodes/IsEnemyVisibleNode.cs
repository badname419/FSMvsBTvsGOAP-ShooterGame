using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnemyVisibleNode : Node
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask enemyMask;
    public LayerMask coverMask;

    public List<Transform> visibleEnemies = new List<Transform>();

    private FieldOfView fieldOfView;
    private GameObject ai;

    public IsEnemyVisibleNode(float viewRadius, float viewAngle, LayerMask enemyMask, LayerMask coverMask, List<Transform> visibleEnemies, GameObject gameObject)
    {
        this.viewRadius = viewRadius;
        this.viewAngle = viewAngle;
        this.enemyMask = enemyMask;
        this.coverMask = coverMask;
        this.visibleEnemies = visibleEnemies;
        this.ai = gameObject;
        fieldOfView = gameObject.AddComponent<FieldOfView>();
    }

    public override NodeState Evaluate()
    {
        visibleEnemies = fieldOfView.FindVisibleEnemies(viewRadius, viewAngle, enemyMask, coverMask, visibleEnemies, ai);
        Debug.Log(visibleEnemies.Count);
        return (visibleEnemies.Count != 0) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
