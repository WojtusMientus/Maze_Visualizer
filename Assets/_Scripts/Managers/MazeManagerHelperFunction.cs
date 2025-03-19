using UnityEngine;

public static class MazeManagerHelperFunction
{

    public static bool IsOutOfCurrentMazeBound(Vector2Int neighbour) => neighbour.x < MazeManager.Instance.CurrentMazeSize &&
                                                         neighbour.y < MazeManager.Instance.CurrentMazeSize;
    public static bool IsInMazeBounds(Vector2Int neighbour) => neighbour.x >= 0 && neighbour.y >= 0 && IsOutOfCurrentMazeBound(neighbour);

    public static (MazeNode fromNode, MazeNode toNode) GetNodesFromVector2Int(Vector2Int from, Vector2Int to) => (MazeManager.Instance.MazeNodes[from], MazeManager.Instance.MazeNodes[to]);
    
    public static void UpdateNodesAndVisualNodes(MazeNode fromNode, MazeNode toNode)
    {
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
