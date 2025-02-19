using System.Collections.Generic;
using UnityEngine;

public abstract class MazeGenerationBase
{
    protected readonly HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
    protected readonly List<Vector2Int> neighbours = new List<Vector2Int>();
    
    public abstract void GenerateMaze();

    protected virtual void CleanHelperDSA()
    {
        visitedNodes.Clear();
    }

    protected virtual void GetAvailableNeighbourNodes(Vector2Int currentNode)
    {
        neighbours.Clear();

        for (int x = -1; x <= 1; x += 2)
        {
            Vector2Int neighbour = new Vector2Int(x + currentNode.x, currentNode.y);
            if (IsNeighbour(neighbour))
                neighbours.Add(neighbour);
        }
        
        for (int y = -1; y <= 1; y += 2)
        {
            Vector2Int neighbour = new Vector2Int(currentNode.x, y + currentNode.y);
            if (IsNeighbour(neighbour))
                neighbours.Add(neighbour);
        }
    }

    protected virtual bool IsNeighbour(Vector2Int neighbour)
    {
        return true;
    }
        
    
}
