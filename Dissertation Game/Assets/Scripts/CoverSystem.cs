using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CoverSystem : MonoBehaviour
{
    [SerializeField] float radius;
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


    private WaypointGizmo waypointGizmoScript;
    private Pathfinding pathfinding;
    private FieldOfView fieldOfView;
    private EnemyThinker enemyThinker;
    private EnemyAI enemyAI;
    private EnemyStats enemyStats;
    private GameObject enemyPlayer;
    private string playerTag = "Player";
    private string wallTag = "Wall";
    private int maxValue;
    private int maxRadiusDistance;
    private List<Waypoint> waypointList;
    private List<Transform> enemyVisibleWaypoints;
    private GameObject[] walls;
    private List<Bounds> wallsBounds = new List<Bounds>();


    private class Waypoint
    {
        public Collider waypointCollider { set; get; }
        public int distance { set; get; }
        public int value { set; get; }
        public GameObject floatingText { set; get; }
    }

    private void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        fieldOfView = GetComponent<FieldOfView>();
        enemyAI = GetComponent<EnemyAI>();
        enemyThinker = GetComponent<EnemyThinker>();
        enemyStats = enemyThinker.enemyStats;

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
    }
    void Start()
    {
        FindLayerObjects(waypointMask.ToString());
        walls = FindTagObjects(wallTag);
        CalculateExpandedWallBounds();
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

            //Debug
            if (updateWallsBounds)
            {
                CalculateExpandedWallBounds();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
        Gizmos.color = Color.yellow;
        if (enemyPlayer != null && rangeModifier)
        {
            Gizmos.DrawWireSphere(enemyPlayer.transform.position, minPrefRange);
            Gizmos.DrawWireSphere(enemyPlayer.transform.position, maxPrefRange);
        }

        if (wallGizmo)
        {
            Gizmos.color = Color.blue;
            if (wallsBounds.Count != 0)
            {
                foreach (Bounds wallBound in wallsBounds)
                {
                    Gizmos.DrawWireCube(wallBound.center, wallBound.size);
                }
            }
        }
    }

    void FindLayerObjects(string layerName)
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

    private GameObject[] FindTagObjects(string tagName)
    {
        return GameObject.FindGameObjectsWithTag(tagName);
    }

    private void CalculateExpandedWallBounds()
    {
        Collider m_Collider;
        Vector3 m_Center;
        Vector3 m_Size;

        wallsBounds.Clear();

        foreach (GameObject wall in walls)
        {
            m_Collider = wall.GetComponent<Collider>();
            m_Center = m_Collider.bounds.center;
            m_Size = m_Collider.bounds.size;

            Bounds b = new Bounds(m_Center, m_Size);
            b.Expand(closeWallThreshold);

            wallsBounds.Add(b);
        }
    }

    public GameObject FindBestCover()
    {
        waypointList.Clear();
        FindNearbyWaypoints();

        int highestValue = -10000;
        int targetIndex = 0;

        //TO DO: Check if this spot has already been taken by someone else
        for (int i = 0; i < waypointList.Count; i++)
        {
            int value = waypointList[i].value;
            if (value > highestValue)
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

        int k = 0;
        foreach (Collider nearbyWaypoint in waypointColliders)
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
            if (wallClosenessModifier)
            {
                if (IsCloseToWall(nearbyWaypoint))
                {
                    value += waypointWallCloseModifier;
                }
            }
            Waypoint waypoint = new Waypoint
            {
                waypointCollider = nearbyWaypoint,
                distance = distance,
                value = value
            };

            waypointList.Add(waypoint);
            k++;
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
        enemyVisibleWaypoints = fieldOfView.FindVisibleObjects(enemyStats.viewAngle, coverMask, waypointColliders, enemyVisibleWaypoints, enemyPlayer);

        for (int i = 0; i < enemyVisibleWaypoints.Count; i++)
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
            if (waypointCollider.gameObject.Equals(bestCover))
            {
                text.GetComponent<TextMesh>().color = Color.red;
            }
            waypoint.floatingText = text;
        }
    }

    private void DestroyFloatingText()
    {
        foreach (Waypoint waypoint in waypointList)
        {
            Destroy(waypoint.floatingText);
        }
    }

    private bool IsCloseToWall(Collider waypoint)
    {
        Vector3 position = waypoint.transform.position;

        //Convert from Transform to Collider
        List<Collider> seenWaypoints = new List<Collider>();
        foreach (Transform visibleWaypoint in enemyVisibleWaypoints)
        {
            seenWaypoints.Add(visibleWaypoint.GetComponent<Collider>());
        }

        //Check if the given waypoint not seen by the enemy
        if (!seenWaypoints.Contains(waypoint))
        {
            foreach (Bounds wallBound in wallsBounds)
            {
                if (wallBound.Contains(position))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private List<Collider> FindHiddenWaypoints()
    {
        List<Collider> seenWaypoints = new List<Collider>();
        foreach (Transform visibleWaypoint in enemyVisibleWaypoints)
        {
            seenWaypoints.Add(visibleWaypoint.GetComponent<Collider>());
        }

        return waypointColliders.Except(seenWaypoints).ToList();

    }

}