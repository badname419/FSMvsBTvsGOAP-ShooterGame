using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (EnemyThinker))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {

        EnemyThinker enemyThinker = (EnemyThinker)target;
        EnemyStats enemyStats = enemyThinker.enemyStats;
        Handles.color = Color.white;
        Handles.DrawWireArc(enemyThinker.transform.position, Vector3.up, Vector3.forward, 360, enemyStats.viewRadius);
        Vector3 viewAngleA = DirFromAngle(-enemyStats.viewAngle / 2, false, enemyThinker.transform);
        Vector3 viewAngleB = DirFromAngle(enemyStats.viewAngle / 2, false, enemyThinker.transform);

        Handles.DrawLine(enemyThinker.transform.position, enemyThinker.transform.position + viewAngleA * enemyStats.viewRadius);
        Handles.DrawLine(enemyThinker.transform.position, enemyThinker.transform.position + viewAngleB * enemyStats.viewRadius);

        /*Handles.color = Color.red;
        foreach(Transform visibleEnemy in fow.visibleEnemies)
        {
            Handles.DrawLine(fow.transform.position, visibleEnemy.position);
        }*/
    }

    private Vector3 DirFromAngle(float angleInDegress, bool angleIsGlobal, Transform target)
    {
        if (!angleIsGlobal)
        {
            angleInDegress += target.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
    }
}
