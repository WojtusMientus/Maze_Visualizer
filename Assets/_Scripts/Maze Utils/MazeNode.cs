using UnityEngine;

public class MazeNode
{
    
    # region PROPERTIES
    public Vector2Int Position { get; private set; }
    public bool WasVisited { get; private set; }
    
    // Order of connections - Up, Right, Down, Left (Clockwise)
    public bool[] Connections { get; private set; }
    
    #endregion

    
    public MazeNode(int xPosition, int yPosition, bool isReachable = true)
    {
        Position = new Vector2Int(xPosition, yPosition);
        Connections = new bool[4];
    }
    
    
    # region METHODS
    public static void OpenConnectionBetweenNodes(MazeNode nodeFrom, MazeNode nodeTo)
    {
        int connectionIndex = DetermineConnection(nodeFrom, nodeTo);
        int nodeToConnectionIndex = (connectionIndex + 2) % 4;
        
        nodeFrom.CreateConnection(connectionIndex);
        nodeTo.CreateConnection(nodeToConnectionIndex);
    }

    private static int DetermineConnection(MazeNode nodeFrom, MazeNode nodeTo)
    {
        if (nodeFrom.Position.x == nodeTo.Position.x)
            return GetConnectionIndex( nodeFrom.Position.y - nodeTo.Position.y, 1);
        
        return GetConnectionIndex(nodeTo.Position.x - nodeFrom.Position.x, 4);
    }

    private static int GetConnectionIndex(int direction, int startingIndex) => (startingIndex + direction) % 4;

    private void CreateConnection(int index)
    {
        Connections[index] = true;
    }

    public int GetNumberOfConnections()
    {
        int counter = 0;
        
        foreach (bool connection in Connections)
            if (connection)
                counter++;

        return counter;
    }


    public void ResetNode()
    {
        for (int i = 0; i < Connections.Length; i++)
            Connections[i] = false;
        
        WasVisited = false;
    }

    public void MarkAsVisited()
    {
        WasVisited = true;
    }
    
    #endregion
}
