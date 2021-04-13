using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FindCoverSpot : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask layerMask;
    [SerializeField] int interval;

    public GameObject textObject;
    public Collider[] hitColliders;
    public List<GameObject> waypoints;
    private WaypointGizmo waypointGizmoScript;
    private Pathfinding pathfinding;

    [SerializeField] List<int> colliderDistances;
    private List<GameObject> floatingValues;

    int maxValue;
    int maxRadiusDistance;

    // Start is called before the first frame update

    private void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        interval = Mathf.RoundToInt(1.0f / Time.deltaTime);
        floatingValues = new List<GameObject>();
        maxValue = 20;
        maxRadiusDistance = 30;
    }
    void Start()
    {
        colliderDistances = new List<int>();
        FindLayerObjets(layerMask.ToString());           
    }

    // Update is called once per frame
    void Update()
    {
        FindNearbyWaypoints();

        
        if (Time.frameCount % interval == 0)
        {
            Debug.Log("Calculate distances");
            colliderDistances.Clear();
            for (int i = 0; i < hitColliders.Length; i++)
            {
                List<AStarNode> path = pathfinding.FindPath(transform.position, hitColliders[i].transform.position);
                colliderDistances.Add(path.Count);
            }
            CalculateNearbyWaypointValue();
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

    private void FindNearbyWaypoints()
    {
        foreach (Collider hitObject in hitColliders)
        {
            waypointGizmoScript = hitObject.GetComponent<WaypointGizmo>();
            waypointGizmoScript.SetColor(Color.yellow);
        }

        hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius, layerMask);


        foreach (Collider hitObject in hitColliders)
        {

            waypointGizmoScript = hitObject.GetComponent<WaypointGizmo>();
            waypointGizmoScript.SetColor(Color.cyan);
        }
    }

    private void CalculateNearbyWaypointValue()
    {
        foreach(GameObject floatingText in floatingValues)
        {
            Destroy(floatingText);
        }
        floatingValues.Clear();

        for(int i=0; i<hitColliders.Length; i++)
        {
            float fValue = (1 - (float)colliderDistances[i] / (float)maxRadiusDistance) * (float)maxValue;
            int value = Mathf.RoundToInt(fValue);

            Vector3 offset = new Vector3(1f, 0.5f, 0f);
            Vector3 textPosition = hitColliders[i].transform.position + offset;

            GameObject text = Instantiate(textObject, textPosition, Quaternion.Euler(new Vector3(90, 0, 0)), hitColliders[i].transform);
            text.GetComponent<TextMesh>().text = value.ToString();
            floatingValues.Add(text);
        }
        
    }

}
