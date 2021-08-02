using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Vision and Ranges")]
    public float lookRange = 40f;
    public float shootingRange = 30f;
    public float hearingRange = 10f;
    public float dashRange = 15f;
    public float viewRadius = 72f;
    public float viewAngle = 108f;
    public float kitGrabingRange = 30f;
    public float meleeRange = 5f;

    [Header("Combat")]
    public int shootingDamage = 5;
    public float attackRate = 1f;
    public float shootingWaitTime = 0.45f;
    public float combatDuration = 10f;

    [Header("Dashing")]
    public float dashForce = 78f;
    public float dashDuration = 0.23f;
    public float dashCooldown = 10f;

    [Header("Health")]
    public float maxHp = 100f;
    public float hpThreshold = 0.3f;
    public float hpRestoreRate = 1f;

    [Header("Masks")]
    public LayerMask enemyLayer;
    public LayerMask coverMask;

    [Header("Rotations")]
    public float rotationSpeed = 10f;
    public float rotationAngle = 120f;
    public int maxRotations = 5;

    public float aggroDistance = 1000f;
    public float arrivalDistance = 1.5f;

    [Header("Healing")]
    public float hpPerSecond = 1f;
    public float hpFromKit = 40f;

   

}
