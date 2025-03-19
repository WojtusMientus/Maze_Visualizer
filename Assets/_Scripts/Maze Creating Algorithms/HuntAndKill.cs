using System.Threading.Tasks;
using UnityEngine;

public class HuntAndKill: MazeGenerationBase
{
    private bool shouldHunt;
    private Vector2Int previousStartHuntPosition;
    
    public override async Task GenerateMaze()
    {
        Vector2Int startingNode = MazeFlowManager.Instance.StartingPosition;
        currentNode = startingNode;
        MazeManagerHelperFunction.MarkNodeAsVisited(currentNode);
        
        while (MazeManagerHelperFunction.IsOutOfCurrentMazeBound(currentNode) && !MazeFlowManager.Instance.StopMazeGeneration)
            await GenerateMazeStepWithDelay();

        OnMazeGenerationFinish();
        CleanHelperDataStructures();
    }

    protected override void CleanHelperDataStructures()
    {
        base.CleanHelperDataStructures();
        previousStartHuntPosition = Vector2Int.zero;
        shouldHunt = false;
    }

    protected override void GenerateMazeStep()
    {
        UpdatePreviousNodeValue();
        
        if (shouldHunt)
            HuntForNextAvailableNode();
        else
            RandomWalk();
        
        UpdateVisualPreviousNode();
        MazeVisualizer.Instance.SetVisualTintCurrentNode(currentNode);
    }

    private void HuntForNextAvailableNode()
    {
        if (!MazeManagerHelperFunction.WasAlreadyInNode(currentNode))
        {
            GetAvailableNeighbourNodes(currentNode, IsNeighbourInHunt);
            CheckNeighboursInHunt(currentNode); 
        }
        else
        {
            GetToTheNextHuntNode();
            previousStartHuntPosition = currentNode;
        }
    }

    
    private bool IsNeighbourInHunt(Vector2Int neighbour)
    {
        return MazeManagerHelperFunction.IsInMazeBounds(neighbour) && MazeManagerHelperFunction.WasAlreadyInNode(neighbour);
    }
    
    private void CheckNeighboursInHunt(Vector2Int node)
    {
        if (neighbours.Count == 0)
        {
            GetToTheNextHuntNode();
            return;
        }
        
        int randomIndex = Random.Range(0, neighbours.Count);
        
        (MazeNode fromNode, MazeNode toNode) = MazeManagerHelperFunction.GetNodesFromVector2Int(node, neighbours[randomIndex]);
        MazeManagerHelperFunction.UpdateNodesAndVisualNodes(fromNode, toNode);
        
        MazeManagerHelperFunction.MarkNodeAsVisited(currentNode);
        shouldHunt = false;
    }

    private void GetToTheNextHuntNode()
    {
        currentNode.x++;

        if (currentNode.x >= MazeManager.Instance.CurrentMazeSize)
        {
            currentNode.x = 0;
            currentNode.y++;
        }
    }

    private void RandomWalk()
    {       
        GetAvailableNeighbourNodes(currentNode, IsNeighbourInRandomWalk);
        CheckNeighbours(currentNode); 
    }
    
    private bool IsNeighbourInRandomWalk(Vector2Int neighbour)
    {
        return MazeManagerHelperFunction.IsInMazeBounds(neighbour) && !MazeManagerHelperFunction.WasAlreadyInNode(neighbour);
    }

    private void CheckNeighbours(Vector2Int node)
    {
        if (neighbours.Count == 0)
        {
            shouldHunt = true;
            currentNode = previousStartHuntPosition;
            return;
        }
        
        int randomIndex = Random.Range(0, neighbours.Count);
        
        (MazeNode fromNode, MazeNode toNode) = MazeManagerHelperFunction.GetNodesFromVector2Int(node, neighbours[randomIndex]);
        MazeManagerHelperFunction.UpdateNodesAndVisualNodes(fromNode, toNode);
        
        currentNode = neighbours[randomIndex];
        MazeManagerHelperFunction.MarkNodeAsVisited(currentNode);
    }
}
