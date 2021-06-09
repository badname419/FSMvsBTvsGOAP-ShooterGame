using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FindCoverSpot : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask waypointMask;
    [SerializeField] LayerMask coverMask;
    [SerializeField] int interval;
    [SerializeField] float minPrefRange;
    [SerializeField] float maxPrefRange;
    [SerializeField] GameObject textObject;
    [SerializeField] Collider[] waypointColliders;
    [SerializeField] List<GameObject> waypoints;
    [SerializeField] GameObject bestCover;
    [SerializeField] int waypointNotSeenModifier;
    [SerializeField] int waypointInPrefRangeModifier;

    [SerializeField] bool seenModifier;
    [SerializeField] bool rangeModifier;


    private WaypointGizmo waypointGizmoScript;
    private Pathfinding pathfinding;
    private FieldOfView fieldOfView;
    private EnemyAI enemyAI;
    private GameObject enemyPlayer;
    private string playerTag = "Player";
    private int maxValue;
    private int maxRadiusDistance;
    private List<Waypoint> waypointList;

    private class Waypoint
    {
        public Collider waypointCollider { set; get; }
        public int distance { set; get; }
        public int value { set; get; }
        public GameObject floatingText { set; get; }
    }

    // Start is called before the first frame update

    private void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        fieldOfView = GetComponent<FieldOfView>();
        enemyAI = GetComponent<EnemyAI>();
        enemyPlayer = GameObject.FindGameObjectWithTag(playerTag);
        interval = Mathf.RoundToInt(1.0f / Time.deltaTime);

        waypointList = new List<Waypoint>();

        maxValue = 20;
        maxRadiusDistance = 30;
    }
    void Start()
    {
        FindLayerObjets(waypointMask.ToString());           
    }

    // Update is called once per frame
    void Update()
    {
        //Make it so that the list of waypoints the player sees at the time is stored globally and can be accessed by the enemies at all times.  
        if (Time.frameCount % interval == 0)
        {
            DestroyFloatingText();
            bestCover = FindBestCover();
            DrawFloatingText();
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemyPlayer.transform.position, minPrefRange);
        Gizmos.DrawWireSphere(enemyPlayer.transform.position, maxPrefRange);
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

    private GameObject FindBestCover()
    {
        waypointList.Clear();
        FindNearbyWaypoints();

        int highestValue = -10000;
        int targetIndex = 0;

        //TO DO: Check if this spot has already been taken by someone else
        for(int i=0; i<waypointList.Count; i++)
        {
            int value = waypointList[i].value;
            if(value > highestValue)
            {
                highestValue = value;
                targetIndex = i;
            }
        }

        return waypointList[targetIndex].waypointCollider.gameObject;
    }

    private void FindNearbyWaypoints()
    {
        foreach (Collider nearbyWaypoint in waypointColliders)
        {
            waypointGizmoScript = nearbyWaypoint.GetComponent<WaypointGizmo>();
            waypointGizmoScript.SetColor(Color.yellow);
        }

        waypointColliders = Physics.OverlapSphere(gameObject.transform.position, radius, waypointMask);

        foreach(Collider nearbyWaypoint in waypointColliders)
        {
            waypointGizmoScript = nearbyWaypoint.GetComponent<WaypointGizmo>();
            waypointGizmoScript.SetColor(Color.cyan);

            int distance = CalculateDistances(nearbyWaypoint);
            int value = CalculateDistanceValue(distance);

            //Debug
            if (seenModifier)
            {
                if (IsWaypointSeen(nearbyWaypoint))
                {
                    value -= waypointNotSeenModifier;
                }
            }
            if (rangeModifier)
            {
                if (IsInPrefRange(nearbyWaypoint))
                {
                    value += waypointInPrefRangeModifier;
                }
            }


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
        List<AStarNode> path = pathfinding.FindPath(transform.position, waypointPosition);

        return path.Count;
    }

    private int CalculateDistanceValue(int distance)
    {
            float fValue = (1 - (float)distance / (float)maxRadiusDistance) * (float)maxValue;
            return Mathf.RoundToInt(fValue);        
    }

    private bool IsWaypointSeen(Collider waypoint)
    {
        List<Transform> enemyVisibleWaypoints = new List<Transform>();
        enemyVisibleWaypoints =  fieldOfView.FindVisibleObjects(enemyAI.viewAngle, coverMask, waypointColliders, enemyVisibleWaypoints, enemyPlayer);
        
        for(int i=0; i<enemyVisibleWaypoints.Count; i++)
        {          
            string seenWaypointName = enemyVisibleWaypoints[i].name;
            string waypointName = waypoint.name;

            if (seenWaypointName.Equals(waypointName))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsInPrefRange(Collider waypoint)
    {
        Vector3 waypointPosition = waypoint.transform.position;
        Vector3 enemyPosition = enemyPlayer.transform.position;
        float dist = Vector3.Distance(waypointPosition, enemyPosition);

        return (dist <= maxPrefRange && dist >= minPrefRange);
    }

    private void DrawFloatingText()
    {
        Vector3 offset = new Vector3(1f, 0.5f, 0f);

        foreach (Waypoint waypoint in waypointList)
        {
            Collider waypointCollider = waypoint.waypointCollider;
            Vector3 textPosition = waypointCollider.transform.position + offset;
            GameObject text = Instantiate(textObject, textPosition, Quaternion.Euler(new Vector3(90, 0, 0)), waypointCollider.transform);
            text.GetComponent<TextMesh>().text = waypoint.value.ToString();
            waypoint.floatingText = text;
        }
    }

    private void DestroyFloatingText()
    {
        foreach(Waypoint waypoint in waypointList)
        {
            Destroy(waypoint.floatingText);
        }
    }

    

}
