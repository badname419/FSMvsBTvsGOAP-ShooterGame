using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (EnemyAI))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyAI fov = (EnemyAI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
        Vector3 viewAngleA = DirFromAngle(-fov.viewAngle / 2, false, fov.transform);
        Vector3 viewAngleB = DirFromAngle(fov.viewAngle / 2, false, fov.transform);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

        Handles.color = Color.red;
        foreach(Transform visibleEnemy in fov.visibleEnemies)
        {
            Handles.DrawLine(fov.transform.position, visibleEnemy.position);
        }
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
