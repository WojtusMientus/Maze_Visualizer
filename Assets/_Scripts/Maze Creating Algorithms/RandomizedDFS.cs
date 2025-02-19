using System.Collections.Generic;
using UnityEngine;


public class RandomizedDFS: MazeGenerationBase
{
    private readonly Stack<Vector2Int> nodeStack = new Stack<Vector2Int>();

    public override void GenerateMaze()
    {
        Vector2Int startingNode = MazeManager.Instance.GetRandomStartingPosition();
        
        visitedNodes.Add(startingNode);
        nodeStack.Push(startingNode);

        while (nodeStack.Count > 0)
        {
            Vector2Int currentNode = nodeStack.Pop();
            GetAvailableNeighbourNodes(currentNode);
            CheckNeighbours(ref currentNode);
        }
        
        CleanHelperDSA();
    }

    protected override void CleanHelperDSA()
    {
        base.CleanHelperDSA();
        nodeStack.Clear();
    }

    protected override bool IsNeighbour(Vector2Int neighbour)
    {
        return !visitedNodes.Contains(neighbour) && MazeManagerHelperFunction.IsInMazeBounds(neighbour);
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
