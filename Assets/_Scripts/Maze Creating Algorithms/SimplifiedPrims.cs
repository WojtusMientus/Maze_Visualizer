using System.Collections.Specialized;
using UnityEngine;

public class SimplifiedPrims : MazeGenerationBase
{
    private readonly OrderedDictionary unfinishedNodes = new OrderedDictionary();
    
    
    public override void GenerateMaze()
    {
        Vector2Int startingNode = MazeManager.Instance.GetRandomStartingPosition();
        
        unfinishedNodes.Add(startingNode, startingNode);

        while (unfinishedNodes.Count > 0)
        {
            int randomIndex = Random.Range(0, unfinishedNodes.Count);
            Vector2Int randomUnfinishedNode = (Vector2Int)unfinishedNodes[randomIndex];
            
            GetAvailableNeighbourNodes(randomUnfinishedNode);
            CheckNeighbours(ref randomUnfinishedNode);
        }
        
        CleanHelperDSA();
    }

    protected override void CleanHelperDSA()
    {
        base.CleanHelperDSA();
        unfinishedNodes.Clear();
    }

    protected override bool IsNeighbour(Vector2Int neighbour)
    {
        return !visitedNodes.Contains(neighbour) && !unfinishedNodes.Contains(neighbour) &&
               MazeManagerHelperFunction.IsInMazeBounds(neighbour);
    }

    private void CheckNeighbours(ref Vector2Int currentNode)
    {
        if (neighbours.Count == 0)
        {
            visitedNodes.Add(currentNode);
            unfinishedNodes.Remove(currentNode);
            return;
        }
        
        int randomIndex = Random.Range(0, neighbours.Count);
        unfinishedNodes.Add(neighbours[randomIndex], neighbours[randomIndex]);

        (MazeNode fromNode, MazeNode toNode) = MazeManagerHelperFunction.GetNodesFromVector2Int(currentNode, neighbours[randomIndex]);
        MazeManagerHelperFunction.UpdateNodesAndVisualNodes(fromNode, toNode);
    }
    
    
}
