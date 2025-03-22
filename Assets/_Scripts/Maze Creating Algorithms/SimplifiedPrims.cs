using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;

public class SimplifiedPrims : MazeGenerationBase
{
    
    private readonly HashSet<Vector2Int> finishedNodes = new HashSet<Vector2Int>();
    private readonly OrderedDictionary unfinishedNodes = new OrderedDictionary();
    
    
    #region METHODS
    
    public override async Task GenerateMaze()
    {
        Vector2Int startingNode = MazeFlowManager.Instance.StartingPosition;
        currentNode = startingNode;
        
        AddToUnfinishedNodes(startingNode);
        
        while (unfinishedNodes.Count > 0 && MazeFlowManager.Instance.IsGeneratingMaze)
            await GenerateMazeStepWithDelay();
        
        OnMazeGenerationFinish();
        CleanHelperDataStructures();
    }

    protected override void GenerateMazeStep()
    {
        UpdatePreviousNodeValue();
        
        int randomIndex = Random.Range(0, unfinishedNodes.Count);
        currentNode = (Vector2Int)unfinishedNodes[randomIndex];
        
        MazeVisualizer.Instance.SetVisualTintCurrentNode(currentNode);
        UpdateVisualPreviousNode();
        
        GetAvailableNeighbourNodes(currentNode, IsNeighbour);
        CheckNeighbours(currentNode);
    }
    
    protected override void CleanHelperDataStructures()
    {
        base.CleanHelperDataStructures();
        finishedNodes.Clear();
        unfinishedNodes.Clear();
    }


    private bool IsNeighbour(Vector2Int neighbour)
    {
        return !finishedNodes.Contains(neighbour) && !unfinishedNodes.Contains(neighbour) &&
               MazeManagerHelperFunction.IsInMazeBounds(neighbour);
    }

    private void CheckNeighbours(Vector2Int node)
    {
        if (neighbours.Count == 0)
        {
            AddToFinishedNodesAndRemoveFromUnfinished(node);
            return;
        }
        
        int randomIndex = Random.Range(0, neighbours.Count);
        AddToUnfinishedNodes(neighbours[randomIndex]);
        
        MazeManagerHelperFunction.UpdateNodesAndVisualNodes(node, neighbours[randomIndex]);
    }

    protected override void AddToUnfinishedNodes(Vector2Int node)
    {
        unfinishedNodes.Add(node, node);
        
        MazeVisualizer.Instance.SetVisualTintVisited(node);
        MazeManagerHelperFunction.MarkNodeAsVisited(node);
    }
    protected override void AddToFinishedNodesAndRemoveFromUnfinished(Vector2Int node)
    {
        finishedNodes.Add(node);
        unfinishedNodes.Remove(node);
    }
    
    #endregion
}
