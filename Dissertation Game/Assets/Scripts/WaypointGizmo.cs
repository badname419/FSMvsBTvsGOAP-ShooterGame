using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGizmo:MonoBehaviour
{

    private Color color = Color.yellow;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        //Gizmos.DrawSphere(transform.position, 1.0f);
        Vector3 gizmoPosition = transform.position;
        gizmoPosition.y -= 1f;
        Gizmos.DrawCube(gizmoPosition, new Vector3(1.0f, 1.0f, 1.0f));
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }
}
