using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class RandomizedDFS: MazeGenerationBase
{
    private readonly Stack<Vector2Int> nodeStack = new Stack<Vector2Int>();
    
    public override async Task GenerateMaze()
    {
        Vector2Int startingNode = MazeFlowManager.Instance.StartingPosition;
        currentNode = startingNode;
        
        nodeStack.Push(startingNode);

        while (nodeStack.Count > 0 && !MazeFlowManager.Instance.StopMazeGeneration)
            await GenerateMazeStepWithDelay();

        OnMazeGenerationFinish();
        CleanHelperDataStructures();
    }
    
    protected override void GenerateMazeStep()
    {
        UpdatePreviousNodeValue();
        UpdateVisualPreviousNode();

        currentNode = nodeStack.Pop();
        
        MazeVisualizer.Instance.SetVisualTintCurrentNode(currentNode);
        MazeManagerHelperFunction.MarkNodeAsVisited(currentNode);  
        
        GetAvailableNeighbourNodes(currentNode, IsNeighbourInRandomWalk);
        CheckNeighbours(currentNode);
    }

    protected override void CleanHelperDataStructures()
    {
        base.CleanHelperDataStructures();
        nodeStack.Clear();
    }
    
    private bool IsNeighbourInRandomWalk(Vector2Int neighbour)
    {
        return MazeManagerHelperFunction.IsInMazeBounds(neighbour) && !MazeManagerHelperFunction.WasAlreadyInNode(neighbour);
    }

    private void CheckNeighbours(Vector2Int node)
    {
        if (neighbours.Count == 0)
            return;
        
        AddToUnfinishedNodes(node);
        
        int randomIndex = Random.Range(0, neighbours.Count);

        AddToUnfinishedNodes(neighbours[randomIndex]);

        (MazeNode fromNode, MazeNode toNode) = MazeManagerHelperFunction.GetNodesFromVector2Int(node, neighbours[randomIndex]);
        MazeManagerHelperFunction.UpdateNodesAndVisualNodes(fromNode, toNode);
    }
    
    protected override void AddToUnfinishedNodes(Vector2Int node)
    {
        nodeStack.Push(node);
    }
}
