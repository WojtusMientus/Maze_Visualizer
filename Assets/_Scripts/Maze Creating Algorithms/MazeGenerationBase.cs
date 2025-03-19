using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MazeGenerationBase
{

    protected readonly List<Vector2Int> neighbours = new List<Vector2Int>();
    
    protected Vector2Int previousNode;
    protected Vector2Int currentNode;
    
    
    public abstract Task GenerateMaze();

    protected virtual void CleanHelperDataStructures()
    {
        neighbours.Clear();
    }

    protected abstract void GenerateMazeStep();
    
    protected async Task GenerateMazeStepWithDelay()
    {
        GenerateMazeStep();
        await Task.Delay(MazeFlowManager.Instance.CurrentGenerationMillisecondDelay);
    }

    protected void UpdateVisualPreviousNode()
    {
        if (!MazeManagerHelperFunction.WasAlreadyInNode(previousNode))
        {
            MazeVisualizer.Instance.SetVisualTintDefault(previousNode);
            return;
        }
        
        if (neighbours.Count != 0)
            MazeVisualizer.Instance.SetVisualTintVisited(previousNode);
        else
            MazeVisualizer.Instance.SetVisualTintFinished(previousNode);
    }

    protected void UpdatePreviousNodeValue()
    {
        previousNode = currentNode;
    }
    
    protected virtual void GetAvailableNeighbourNodes(Vector2Int node, Func<Vector2Int, bool> IsNeighbourFunction)
    {
        neighbours.Clear();

        for (int x = -1; x <= 1; x += 2)
        {
            Vector2Int neighbour = new Vector2Int(x + node.x, node.y);
            if (IsNeighbourFunction(neighbour))
                neighbours.Add(neighbour);
        }
        
        for (int y = -1; y <= 1; y += 2)
        {
            Vector2Int neighbour = new Vector2Int(node.x, y + node.y);
            if (IsNeighbourFunction(neighbour))
                neighbours.Add(neighbour);
        }
    }
    
    protected void OnMazeGenerationFinish()
    {
        EventManager.RaiseOnMazeGenerationEnd();
    }

    protected virtual void AddToUnfinishedNodes(Vector2Int node)
    {
    }

    protected virtual void AddToFinishedNodesAndRemoveFromUnfinished(Vector2Int node)
    {
    }
}
