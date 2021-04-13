using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FindCoverSpot : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask layerMask;
    [SerializeField] int interval;
    public Collider[] hitColliders;
    public List<GameObject> waypoints;
    private WaypointGizmo waypointGizmoScript;
    private Pathfinding pathfinding;

    [SerializeField] List<int> colliderDistances;

    // Start is called before the first frame update

    private void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        interval = Mathf.RoundToInt(1.0f / Time.deltaTime);
    }
    void Start()
    {
        colliderDistances = new List<int>();
        FindLayerObjets(layerMask.ToString());           
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Collider hitObject in hitColliders)
        {
            waypointGizmoScript = hitObject.GetComponent<WaypointGizmo>();
            waypointGizmoScript.SetColor(Color.yellow);
            //Gizmos.color = Color.yellow;
            //Vector3 gizmoPosition = hitObject.transform.position;
            //gizmoPosition.y -= 1f;
            //Gizmos.DrawCube(gizmoPosition, new Vector3(1.0f, 1.0f, 1.0f));
            //hitObject.GetComponent<Renderer>().material.color = Color.yellow;
        }

        hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius, layerMask);
        

        foreach (Collider hitObject in hitColliders)
        {

            waypointGizmoScript = hitObject.GetComponent<WaypointGizmo>();
            waypointGizmoScript.SetColor(Color.cyan);

            //colliderDistances.Add(pathfinding.GetPathLength());
            //Gizmos.color = Color.cyan;
            //Vector3 gizmoPosition = hitObject.transform.position;
            //gizmoPosition.y -= 1f;
            //Gizmos.DrawCube(gizmoPosition, new Vector3(1.0f, 1.0f, 1.0f));
            //hitObject.GetComponent<Renderer>().material.color = Color.cyan;
        }

        
        if (Time.frameCount % interval == 0)
        {
            Debug.Log("Calculate distances");
            colliderDistances.Clear();
            for (int i = 0; i < hitColliders.Length; i++)
            {
                List<AStarNode> path = pathfinding.FindPath(transform.position, hitColliders[i].transform.position);
                colliderDistances.Add(path.Count);
            }
        }
        

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }

    void FindLayerObjets(string layerName)
    {

        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gos)
        {
            if (go.layer.Equals(layerName))
            {
                waypoints.Add(go);
            }
        }
    }

}
