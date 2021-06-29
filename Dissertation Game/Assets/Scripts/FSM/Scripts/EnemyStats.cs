using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float lookRange = 40f;

    public float shootingRange = 30f;
    public float attackRate = 1f;
    public int shootingDamage = 30;
    public float viewRadius = 68f;
    public float viewAngle = 108f;
    public float maxHp = 100f;
    public float hpThreshold = 30f;

    public LayerMask enemyLayer;
    public LayerMask coverMask;

    public float rotationSpeed = 10f;
    public float rotationAngle = 120f;

}
