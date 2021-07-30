using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    //Vision and ranges
    public float lookRange = 40f;
    public float shootingRange = 30f;
    public float viewRadius = 72f;
    public float viewAngle = 108f;
    public float kitGrabingRange = 30f;

    public int shootingDamage = 30;
    public float attackRate = 1f;
    public float shootingWaitTime = 0.45f;

    //Health
    public float maxHp = 100f;
    public float hpThreshold = 0.3f;
    public float hpRestoreRate = 1f;

    //Masks
    public LayerMask enemyLayer;
    public LayerMask coverMask;

    //Rotations
    public float rotationSpeed = 10f;
    public float rotationAngle = 120f;
    public int maxRotations = 5;

    public float aggroDistance = 1000f;
    public float arrivalDistance = 1.5f;

   

}
