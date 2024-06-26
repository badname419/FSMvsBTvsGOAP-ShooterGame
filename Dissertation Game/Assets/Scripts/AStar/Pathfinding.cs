using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid grid;
    private Transform startPosition;
    private Transform targetPosition;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        //FindPath(startPosition.position, targetPosition.position);
    }

    public List<AStarNode> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        AStarNode startNode = grid.NodeFromWorldPosition(startPos);
        AStarNode targetNode = grid.NodeFromWorldPosition(targetPos);

        List<AStarNode> openList = new List<AStarNode>();
        HashSet<AStarNode> closedList = new HashSet<AStarNode>();

        openList.Add(startNode);

        while(openList.Count > 0)
        {
            AStarNode currentNode = openList[0];
            for(int i=1; i<openList.Count; i++)
            {
                if(openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hcost < currentNode.hcost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
            }

            foreach(AStarNode neighbourNode in grid.GetNeighbouringNodes(currentNode))
            {
                if(!neighbourNode.isCover || closedList.Contains(neighbourNode))
                {
                    continue;
                }

                int moveCost = currentNode.gCost + getManhattanDistance(currentNode, neighbourNode);

                if(moveCost < neighbourNode.gCost || !openList.Contains(neighbourNode))
                {
                    neighbourNode.gCost = moveCost;
                    neighbourNode.hcost = getManhattanDistance(neighbourNode, targetNode);
                    neighbourNode.parent = currentNode;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        return grid.finalPath;
    }

    private int getManhattanDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int iX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return iX + iY;
    }

    private void GetFinalPath(AStarNode startNode, AStarNode targetNode)
    {
        List<AStarNode> finalPath = new List<AStarNode>();
        AStarNode currentNode = targetNode;

        while (currentNode != startNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        finalPath.Reverse();

        grid.finalPath = finalPath;
    }

    public int GetPathLength()
    {
        return grid.finalPath.Count;
    }
}
