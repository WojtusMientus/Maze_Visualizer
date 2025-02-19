using System.Collections.Generic;
using UnityEngine;

public class MazeManager : SingletonMonoBehaviour<MazeManager>
{
    public Dictionary<Vector2Int, MazeNode> MazeNodes { get; private set; }
    public int CurrentMazeSize { get; private set; }
    private int previousMazeSize;
    
    private readonly int maxMazeSize = 50;

    private MazeGenerationBase mazeGeneration = new SimplifiedPrims();
    
    
    
    
    protected override void Awake()
    {
        base.Awake();
        
        CreateMazeNodes();
    }

    private void OnEnable()
    {
        EventManager.OnSizeSliderValueChanged += UpdateCurrentMazeSize;
    }

    private void OnDisable()
    {
        EventManager.OnSizeSliderValueChanged -= UpdateCurrentMazeSize;
    }

    private void CreateMazeNodes()
    {
        if (MazeNodes != null)
            return;
        
        MazeNodes = new Dictionary<Vector2Int, MazeNode>();
        
        for (int x = 0; x < maxMazeSize; x++)
        {
            for (int y = 0; y < maxMazeSize; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                MazeNodes.Add(position, new MazeNode(x, y));
                MazeVisualizer.Instance.CreateVisualNode(position);
            }
        }
    }
    
    private void UpdateCurrentMazeSize(int currentSize)
    {
        CurrentMazeSize = currentSize;
    }

    public void GenerateMaze()
    {
        ResetMaze();
        previousMazeSize = CurrentMazeSize;
        mazeGeneration.GenerateMaze();
    }

    private void ResetMaze()
    {
        for (int x = 0; x < previousMazeSize; x++)
        {
            for (int y = 0; y < previousMazeSize; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                MazeNodes[position].ResetNode();
                MazeVisualizer.Instance.ResetVisualNode(position);
            }
        }
    }
    
    public Vector2Int GetRandomStartingPosition()
    {
        int randomStartingX = Random.Range(0, CurrentMazeSize);
        int randomStartingY = Random.Range(0, CurrentMazeSize);

        return new Vector2Int(randomStartingX, randomStartingY);
    }
    
    
    

    // private void OnDrawGizmos()
    // {
    //     foreach (KeyValuePair<Vector2Int, MazeNode> kvp in MazeNodes)
    //     {
    //         Vector3 newPosition = new Vector3(kvp.Key.x, kvp.Key.y, 0);
    //         Gizmos.DrawWireSphere(newPosition, 0.1f);
    //         
    //         MazeNode node = kvp.Value;
    //
    //         if (node.Connections[0])
    //             Gizmos.DrawRay(newPosition, Vector3.up * .35f);
    //         
    //         if (node.Connections[1])
    //             Gizmos.DrawRay(newPosition, Vector3.right * .35f);
    //         
    //         if (node.Connections[2])
    //             Gizmos.DrawRay(newPosition, Vector3.down * .35f);
    //         
    //         if (node.Connections[3])
    //             Gizmos.DrawRay(newPosition, Vector3.left * .35f);
    //     }
    // }
}


