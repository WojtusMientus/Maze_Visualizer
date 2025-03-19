using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class MazeManager : SingletonMonoBehaviour<MazeManager>
{
    public Dictionary<Vector2Int, MazeNode> MazeNodes { get; private set; }
    
    public List<Vector2Int> VisitedNodes { get; private set; } = new List<Vector2Int>();
    
    public int CurrentMazeSize { get; private set; }
    
    private int previousMazeSize;
    private readonly int maxMazeSize = 50;

    private readonly MazeGenerationScripts mazeGenerationScripts = new MazeGenerationScripts();

    private MazeGenerationBase currentMazeGenerationAlgo;
    
    
    protected override void Awake()
    {
        base.Awake();
        
        currentMazeGenerationAlgo = mazeGenerationScripts.GetAlgoAtIndex(0);
        
        CreateMazeNodes();
    }

    private void OnEnable()
    {
        EventManager.OnSizeSliderValueChanged += UpdateCurrentMazeSize;
        EventManager.OnDropdownMazeSelectionChanged += UpdateCurrentGenerationAlgo;
    }

    private void OnDisable()
    {
        EventManager.OnSizeSliderValueChanged -= UpdateCurrentMazeSize;
        EventManager.OnDropdownMazeSelectionChanged -= UpdateCurrentGenerationAlgo;
    }
    
    private void CreateMazeNodes()
    {        
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
        EventManager.RaiseOnMazeGenerationStart();
        currentMazeGenerationAlgo.GenerateMaze();
    }

    private void ResetMaze()
    {
        Debug.Log(VisitedNodes.Count);
        
        foreach (Vector2Int vec in VisitedNodes)
        {
            MazeNodes[vec].ResetNode();
            MazeVisualizer.Instance.ResetVisualNode(vec);
        }
        
        VisitedNodes.Clear();
    }
    
    public void ResetMazeAndSetRandomStartingPosition()
    {
        ResetMaze();
        MazeFlowManager.Instance.SetRandomStartingPosition();
    }
    
    private void UpdateCurrentGenerationAlgo(int index)
    {
        currentMazeGenerationAlgo = mazeGenerationScripts.GetAlgoAtIndex(index);
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


