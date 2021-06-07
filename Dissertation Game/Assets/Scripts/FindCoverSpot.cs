using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FindCoverSpot : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask coverMask;
    [SerializeField] int interval;
    public List<Transform> visibleObjects;

    public GameObject textObject;
    public Collider[] hitColliders;
    public List<GameObject> waypoints;
    private WaypointGizmo waypointGizmoScript;
    private Pathfinding pathfinding;
    private FieldOfView fieldOfView;
    private EnemyAI enemyAI;
    private GameObject enemyPlayer;
    private string playerTag = "Player";

    [SerializeField] List<int> colliderDistances;
    private List<GameObject> floatingValues;
    private List<int> waypointValue;

    int maxValue;
    int maxRadiusDistance;

    // Start is called before the first frame update

    private void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        fieldOfView = GetComponent<FieldOfView>();
        enemyAI = GetComponent<EnemyAI>();
        enemyPlayer = GameObject.FindGameObjectWithTag(playerTag);
        interval = Mathf.RoundToInt(1.0f / Time.deltaTime);

        floatingValues = new List<GameObject>();
        visibleObjects = new List<Transform>();
        waypointValue = new List<int>();
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
            CalculateDistances();
            CalculateNearbyWaypointValue();
            CalculateCoverFromPrimary();
            DrawValues();
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

    private void CalculateDistances()
    {
        Debug.Log("Calculate distances");
        colliderDistances.Clear();
        for (int i = 0; i < hitColliders.Length; i++)
        {
            List<AStarNode> path = pathfinding.FindPath(transform.position, hitColliders[i].transform.position);
            colliderDistances.Add(path.Count);
        }
    }

    private void CalculateNearbyWaypointValue()
    {
        for(int i=0; i<hitColliders.Length; i++)
        {
            float fValue = (1 - (float)colliderDistances[i] / (float)maxRadiusDistance) * (float)maxValue;
            int value = Mathf.RoundToInt(fValue);
            waypointValue.Add(value);

        }
        
    }

    private void CalculateCoverFromPrimary()
    {
        visibleObjects.Clear();
        fieldOfView.FindVisibleObjects(enemyAI.viewAngle, coverMask, hitColliders, visibleObjects, enemyPlayer);
        
        for(int i=0; i<visibleObjects.Count; i++)
        {
            string seenWaypointName = visibleObjects[i].name;
            for(int j=0; j<hitColliders.Length; j++)
            {
                string waypointName = hitColliders[j].name;
                if (seenWaypointName.Equals(waypointName))
                {
                    waypointValue[j] -= 20;

                }
            }
        }

    }

    private void DrawValues()
    {
        foreach (GameObject floatingText in floatingValues)
        {
            Destroy(floatingText);
        }
        floatingValues.Clear();

        Vector3 offset = new Vector3(1f, 0.5f, 0f);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            Vector3 textPosition = hitColliders[i].transform.position + offset;

            GameObject text = Instantiate(textObject, textPosition, Quaternion.Euler(new Vector3(90, 0, 0)), hitColliders[i].transform);
            text.GetComponent<TextMesh>().text = waypointValue[i].ToString();
            floatingValues.Add(text);
        }

        waypointValue.Clear();
    }

}
