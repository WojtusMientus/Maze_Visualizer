using System.Collections.Generic;
using UnityEngine;


public class RandomizedDST: MazeGenerationBase
{
    
    private readonly HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
    private readonly Stack<Vector2Int> nodeStack = new Stack<Vector2Int>();
    private readonly List<Vector2Int> neighbours = new List<Vector2Int>();

    public override void GenerateMaze()
    {
        Vector2Int startingNode = MazeManager.Instance.GetRandomStartingPosition();

        InitializeHelperDS();
        
        visitedNodes.Add(startingNode);
        nodeStack.Push(startingNode);

        while (nodeStack.Count > 0)
        {
            Vector2Int currentNode = nodeStack.Pop();
            GetAvailableNeighbourNodes(currentNode);
            CheckNeighbours(ref currentNode);
        }
    }

    private void InitializeHelperDS()
    {
        visitedNodes.Clear();
        nodeStack.Clear();
    }
    private void GetAvailableNeighbourNodes(Vector2Int currentNode)
    {
        neighbours.Clear();

        for (int x = -1; x <= 1; x += 2)
        {
            Vector2Int neighbour = new Vector2Int(x + currentNode.x, currentNode.y);
            if (!visitedNodes.Contains(neighbour) && MazeManagerHelperFunction.IsInMazeBounds(neighbour))
                neighbours.Add(neighbour);
        }
        
        for (int y = -1; y <= 1; y += 2)
        {
            Vector2Int neighbour = new Vector2Int(currentNode.x, y + currentNode.y);
            if (!visitedNodes.Contains(neighbour) && MazeManagerHelperFunction.IsInMazeBounds(neighbour))
                neighbours.Add(neighbour);
        }
        
    }
    
    private void CheckNeighbours(ref Vector2Int currentNode)
    {
        if (neighbours.Count == 0)
            return;
        
        nodeStack.Push(currentNode);
        
        int randomIndex = Random.Range(0, neighbours.Count);

        visitedNodes.Add(neighbours[randomIndex]);
        nodeStack.Push(neighbours[randomIndex]);

        (MazeNode fromNode, MazeNode toNode) = MazeManagerHelperFunction.GetNodesFromVector2Int(currentNode, neighbours[randomIndex]);
        MazeManagerHelperFunction.UpdateNodesAndVisualNodes(fromNode, toNode);
    }
}
