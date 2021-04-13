using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (EnemyAI))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyAI fow = (EnemyAI)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = DirFromAngle(-fow.viewAngle / 2, false, fow.transform);
        Vector3 viewAngleB = DirFromAngle(fow.viewAngle / 2, false, fow.transform);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        foreach(Transform visibleEnemy in fow.visibleEnemies)
        {
            Handles.DrawLine(fow.transform.position, visibleEnemy.position);
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
