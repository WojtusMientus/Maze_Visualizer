using System.Threading.Tasks;
using UnityEngine;

public class HuntAndKill: MazeGenerationBase
{
    
    private bool enterHuntMode;
    private bool stopExecution;
    private Vector2Int previousHuntStartPosition;
    
    
    #region METHODS
    
    public override async Task GenerateMaze()
    {
        Vector2Int startingNode = MazeFlowManager.Instance.StartingPosition;
        currentNode = startingNode;
        MazeManagerHelperFunction.MarkNodeAsVisited(currentNode);
        
        while (!stopExecution && MazeFlowManager.Instance.IsGeneratingMaze)
            await GenerateMazeStepWithDelay();
        
        OnMazeGenerationFinish();
        CleanHelperDataStructures();
    }
    
    protected override void GenerateMazeStep()
    {
        UpdatePreviousNodeValue();
        
        if (enterHuntMode)
            HuntForNextAvailableNode();
        else
            RandomWalk();
        
        UpdateVisualPreviousNode();
        MazeVisualizer.Instance.SetVisualTintCurrentNode(currentNode);
    }
    
    protected override void CleanHelperDataStructures()
    {
        base.CleanHelperDataStructures();
        previousHuntStartPosition = Vector2Int.zero;
        enterHuntMode = false;
        stopExecution = false;
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
            previousHuntStartPosition = currentNode;
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
        
        int randomUnvisitedNeighbourIndex = Random.Range(0, neighbours.Count);
        MazeManagerHelperFunction.UpdateNodesAndVisualNodes(node, neighbours[randomUnvisitedNeighbourIndex]);
        
        MazeManagerHelperFunction.MarkNodeAsVisited(currentNode);
        enterHuntMode = false;
    }

    private void GetToTheNextHuntNode()
    {
        currentNode.x++;
        
        if (currentNode.x >= MazeManager.Instance.CurrentMazeSize)
        {
            currentNode.x = 0;
            currentNode.y++;
            
            if (currentNode.y >= MazeManager.Instance.CurrentMazeSize)
            {
                currentNode = new Vector2Int(MazeManager.Instance.CurrentMazeSize - 1, MazeManager.Instance.CurrentMazeSize - 1);
                stopExecution = true;
            }
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
            enterHuntMode = true;
            currentNode = previousHuntStartPosition;
            return;
        }
        
        int randomUnvisitedNeighbourIndex = Random.Range(0, neighbours.Count);
        MazeManagerHelperFunction.UpdateNodesAndVisualNodes(node, neighbours[randomUnvisitedNeighbourIndex]);
        
        currentNode = neighbours[randomUnvisitedNeighbourIndex];
        MazeManagerHelperFunction.MarkNodeAsVisited(currentNode);
    }

    public override void OnMazeReset()
    {
        MazeVisualizer.Instance.SetVisualTintDefault(currentNode);
    }

    #endregion
    
}
