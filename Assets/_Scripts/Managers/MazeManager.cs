using System.Collections.Generic;
using UnityEngine;
using Vector2Int = UnityEngine.Vector2Int;

[DefaultExecutionOrder(0)]
public class MazeManager : SingletonMonoBehaviour<MazeManager>
{
    
    #region PROPERTIES
    public Dictionary<Vector2Int, MazeNode> MazeNodes { get; private set; }
    public List<Vector2Int> VisitedNodes { get; private set; } = new List<Vector2Int>();
    public int CurrentMazeSize { get; private set; }
    
    #endregion
    
    
    private readonly int maxMazeSize = 45;

    private readonly MazeGenerationScripts mazeGenerationScripts = new MazeGenerationScripts();

    private MazeGenerationBase currentMazeGenerationAlgo;
    
    
    protected override void Awake()
    {
        base.Awake();
        
        currentMazeGenerationAlgo = mazeGenerationScripts.GetAlgoAtIndex(0);
        
        InitializeMazeNodes();
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
    
    
    #region METHODS
    
    private void InitializeMazeNodes()
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
        ResetMaze();
    }

    public void GenerateMaze()
    {
        ResetMazeDictionary();
        
        EventManager.RaiseOnMazeGenerationStart();
        currentMazeGenerationAlgo.GenerateMaze();
    }


    private void ResetMazeDictionary()
    {
        foreach (Vector2Int vector in VisitedNodes)
        {
            MazeNodes[vector].ResetNode();
            MazeVisualizer.Instance.ResetVisualNode(vector);
        }
        
        VisitedNodes.Clear();
    }

    public void ResetMaze()
    {
        ResetMazeDictionary();
        currentMazeGenerationAlgo.OnMazeReset();
        MazeFlowManager.Instance.VisualizeStartingPoint(MazeFlowManager.Instance.StartingPosition);
    }
    
    public void ResetMazeAndSetRandomStartingPosition()
    {
        ResetMazeDictionary();
        MazeFlowManager.Instance.SetRandomStartingPosition();
    }
    
    private void UpdateCurrentGenerationAlgo(int index)
    {
        currentMazeGenerationAlgo = mazeGenerationScripts.GetAlgoAtIndex(index);
    }

    #endregion
    
}


