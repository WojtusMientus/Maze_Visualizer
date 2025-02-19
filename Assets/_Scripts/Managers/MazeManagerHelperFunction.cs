using UnityEngine;

public static class MazeManagerHelperFunction
{

    private static bool IsInCurrentMaze(Vector2Int neighbour) => neighbour.x < MazeManager.Instance.CurrentMazeSize &&
                                                         neighbour.y < MazeManager.Instance.CurrentMazeSize;
    public static bool IsInMazeBounds(Vector2Int neighbour) => neighbour.x >= 0 && neighbour.y >= 0 && IsInCurrentMaze(neighbour);

    public static (MazeNode fromNode, MazeNode toNode) GetNodesFromVector2Int(Vector2Int from, Vector2Int to) => (MazeManager.Instance.MazeNodes[from], MazeManager.Instance.MazeNodes[to]);
    
    public static void UpdateNodesAndVisualNodes(MazeNode fromNode, MazeNode toNode)
    {
        MazeNode.OpenConnectionBetweenNodes(fromNode, toNode);
        MazeVisualizer.Instance.CreateVisualConnection(fromNode, toNode);
    }
}
