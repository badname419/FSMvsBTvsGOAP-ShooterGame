using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FindCoverSpot : MonoBehaviour
{
    [SerializeField] float radius;
<<<<<<< Updated upstream
    [SerializeField] LayerMask layerMask;
    public Collider[] hitColliders;
    public List<GameObject> waypoints;
=======
    [SerializeField] LayerMask waypointMask;
    [SerializeField] LayerMask coverMask;
    private int interval;
    private float minPrefRange;
    private float maxPrefRange;
    private float closeWallThreshold;
    [SerializeField] GameObject textObject;
    [SerializeField] Collider[] waypointColliders;
    [SerializeField] List<GameObject> waypoints;
    [SerializeField] GameObject bestCover;
    private int waypointNotSeenModifier;
    private int waypointInPrefRangeModifier;
    private int waypointWallCloseModifier;

    [SerializeField] bool seenModifier;
    [SerializeField] bool rangeModifier;
    [SerializeField] bool wallClosenessModifier;

    [SerializeField] bool wallGizmo;
    //Debug
    [SerializeField] bool updateWallsBounds;
    [SerializeField] FindCoverValues findCoverValues;

>>>>>>> Stashed changes
    private WaypointGizmo waypointGizmoScript;
    private Pathfinding pathfinding;
    private Grid grid;

    [SerializeField] List<int> colliderDistances;

    // Start is called before the first frame update

    private void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
<<<<<<< Updated upstream
=======
        fieldOfView = GetComponent<FieldOfView>();
        enemyAI = GetComponent<EnemyAI>();

        enemyPlayer = GameObject.FindGameObjectWithTag(playerTag);

        //Values from FindCoverValues scriptable object
        interval = findCoverValues.interval;
        interval = Mathf.RoundToInt(1.0f / Time.deltaTime);
        minPrefRange = findCoverValues.minPrefRange;
        maxPrefRange = findCoverValues.maxPrefRange;
        closeWallThreshold = findCoverValues.closeWallThreshold;

        waypointNotSeenModifier = findCoverValues.waypointNotSeenModifier;
        waypointInPrefRangeModifier = findCoverValues.waypointInPrefRangeModifier;
        waypointWallCloseModifier = findCoverValues.waypointWallCloseModifier;


        waypointList = new List<Waypoint>();
        enemyVisibleWaypoints = new List<Transform>();

        maxValue = 20;
        maxRadiusDistance = 30;      
>>>>>>> Stashed changes
    }
    void Start()
    {
        FindLayerObjets(layerMask.ToString());   
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        colliderDistances.Clear();
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

<<<<<<< Updated upstream
            waypointGizmoScript = hitObject.GetComponent<WaypointGizmo>();
            waypointGizmoScript.SetColor(Color.cyan);
=======
            Waypoint waypoint = new Waypoint
            {
                waypointCollider = nearbyWaypoint,
                distance = distance,
                value = value
            };

            waypointList.Add(waypoint);
        }
    }

    private int CalculateDistances(Collider waypoint)
    {
        Vector3 waypointPosition = waypoint.transform.position;
        EnemyThinker enemyThinker = GetComponent<EnemyThinker>();

        List<AStarNode> path = new List<AStarNode>();
        if (enemyThinker != null)
        {
            path = enemyThinker.pathfinding.FindPath(transform.position, waypointPosition);
        }
        else
        {
            path = pathfinding.FindPath(transform.position, waypointPosition);
        }

        return path.Count;
    }

    private int CalculateDistanceValue(int distance)
    {
            float fValue = (1 - (float)distance / (float)maxRadiusDistance) * (float)maxValue;
            return Mathf.RoundToInt(fValue);        
    }

    private bool IsWaypointSeen(Collider waypoint)
    {
        enemyVisibleWaypoints.Clear();
        enemyVisibleWaypoints =  fieldOfView.FindVisibleObjects(enemyAI.viewAngle, coverMask, waypointColliders, enemyVisibleWaypoints, enemyPlayer);
        
        for(int i=0; i<enemyVisibleWaypoints.Count; i++)
        {          
            string seenWaypointName = enemyVisibleWaypoints[i].name;
            string waypointName = waypoint.name;
>>>>>>> Stashed changes

            //colliderDistances.Add(pathfinding.GetPathLength());
            //Gizmos.color = Color.cyan;
            //Vector3 gizmoPosition = hitObject.transform.position;
            //gizmoPosition.y -= 1f;
            //Gizmos.DrawCube(gizmoPosition, new Vector3(1.0f, 1.0f, 1.0f));
            //hitObject.GetComponent<Renderer>().material.color = Color.cyan;
        }

        for(int i=0; i<hitColliders.Length; i++)
        {
            List<AStarNode> path = pathfinding.FindPath(transform.position, hitColliders[i].transform.position);
            colliderDistances.Add(path.Count);
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
