using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RandomizedDFS: MazeGenerationBase
{
    
    private readonly Stack<Vector2Int> nodeStack = new Stack<Vector2Int>();
    
    
    #region METHODS
    
    public override async Task GenerateMaze()
    {
        Vector2Int startingNode = MazeFlowManager.Instance.StartingPosition;
        currentNode = startingNode;
        
        AddToUnfinishedNodes(startingNode);

        while (nodeStack.Count > 0 && MazeFlowManager.Instance.IsGeneratingMaze)
            await GenerateMazeStepWithDelay();
        
        OnMazeGenerationFinish();
        CleanHelperDataStructures();
    }
    
    protected override void GenerateMazeStep()
    {
        UpdatePreviousNodeValue();

        currentNode = nodeStack.Pop();
        
        GetAvailableNeighbourNodes(currentNode, IsNeighbourInRandomWalk);
        CheckNeighbours(currentNode);

        if (!nodeStack.TryPeek(out currentNode))
            return;
            
        UpdateVisualPreviousNode();
        MazeVisualizer.Instance.SetVisualTintCurrentNode(currentNode);
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

        MazeManagerHelperFunction.UpdateNodesAndVisualNodes(node, neighbours[randomIndex]);
    }
    
    protected override void AddToUnfinishedNodes(Vector2Int node)
    {
        nodeStack.Push(node);
        MazeManagerHelperFunction.MarkNodeAsVisited(node);  
    }
    
    #endregion
    
}
