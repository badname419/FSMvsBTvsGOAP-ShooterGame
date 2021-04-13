using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public int gridX;
    public int gridY;

    public bool isCover;
    public Vector3 position;

    public AStarNode parent;

    public int gCost;
    public int hCost;
    public int fCost {  get { return gCost + hCost; } }

    public AStarNode(bool isCover, Vector3 position, int gridX, int gridY)
    {
        this.isCover = isCover;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}
