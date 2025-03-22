using UnityEngine;

public static class MazeManagerHelperFunction
{

    private static bool IsOutOfCurrentMazeBound(Vector2Int neighbour) => neighbour.x < MazeManager.Instance.CurrentMazeSize &&
                                                         neighbour.y < MazeManager.Instance.CurrentMazeSize;
    public static bool IsInMazeBounds(Vector2Int neighbour) => neighbour.x >= 0 && neighbour.y >= 0 && IsOutOfCurrentMazeBound(neighbour);

    private static (MazeNode fromNode, MazeNode toNode) GetNodesFromVector2Int(Vector2Int fromNode, Vector2Int toNode) => (MazeManager.Instance.MazeNodes[fromNode], MazeManager.Instance.MazeNodes[toNode]);
    
    public static void UpdateNodesAndVisualNodes(Vector2Int fromNodePosition, Vector2Int toNodePosition)
    {
        (MazeNode fromNode, MazeNode toNode) = GetNodesFromVector2Int(fromNodePosition, toNodePosition);
        MazeNode.OpenConnectionBetweenNodes(fromNode, toNode);
        MazeVisualizer.Instance.CreateVisualConnection(fromNode, toNode);
    }
    public static bool WasAlreadyInNode(Vector2Int node) => MazeManager.Instance.MazeNodes[node].WasVisited;

    public static void MarkNodeAsVisited(Vector2Int node)
    {
        if (!MazeManager.Instance.MazeNodes[node].WasVisited)
        {
            MazeManager.Instance.MazeNodes[node].MarkAsVisited();
            MazeManager.Instance.VisitedNodes.Add(node);
        }
    }
    
}
