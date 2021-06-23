using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cover/FindCoverValues")]
public class FindCoverValues : ScriptableObject
{
    public int interval;
    public float minPrefRange;
    public float maxPrefRange;
    public float closeWallThreshold;

    public int waypointNotSeenModifier;
    public int waypointInPrefRangeModifier;
    public int waypointWallCloseModifier;
}
