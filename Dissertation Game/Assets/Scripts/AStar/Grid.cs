using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private Transform startPosition;
    public LayerMask coverMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float Distance;

    AStarNode[,] grid;
    public List<AStarNode> finalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private Pathfinding pathfinding;

    // Start is called before the first frame update
    void Awake()
    {
        gridWorldSize = new Vector2(GetComponent<Renderer>().bounds.size.x, GetComponent<Renderer>().bounds.size.z);
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        pathfinding = GetComponent<Pathfinding>();
    }

    public AStarNode NodeFromWorldPosition(Vector3 worldPos)
    {
        float xPoint = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float yPoint = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];
    }

    void CreateGrid()
    {
        grid = new AStarNode[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool cover = (Physics.CheckSphere(worldPoint, nodeRadius, coverMask));
                grid[i, j] = new AStarNode(cover, worldPoint, i, j);
            }
        }
    }

    public List<AStarNode> GetNeighbouringNodes(AStarNode node)
    {
        List<AStarNode> neighbouringNodes = new List<AStarNode>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                //if we are on the node tha was passed in, skip this iteration.
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int checkX = node.gridX + i;
                int checkY = node.gridY + j;

                //Make sure the node is within the grid.
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbouringNodes.Add(grid[checkX, checkY]); //Adds to the neighbours list.
                }

            }
        }

        return neighbouringNodes;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0f, gridWorldSize.y));

        if (grid != null)
        {
            foreach (AStarNode node in grid)
            {
                Gizmos.color = (!node.isCover) ? Color.white : Color.red;
                if (finalPath != null)
                {
                    if (finalPath.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    public int GetGridSizeX()
    {
        return gridSizeX;
    }

    public int GetGridSizeZ()
    {
        return gridSizeY;
    }

    public bool IsValid(int posX, int posY)
    {
        return !grid[posX, posY].isCover;
    }

    public Vector3 GetNodePosition(int posX, int posY)
    {
        return grid[posX, posY].position;
    }
}