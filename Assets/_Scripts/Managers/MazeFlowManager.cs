using UnityEngine;

[DefaultExecutionOrder(1)]
public class MazeFlowManager : SingletonMonoBehaviour<MazeFlowManager>
{
    
    #region PROPERTIES
    public int CurrentGenerationMillisecondDelay { get; private set; }
    public Vector2Int StartingPosition { get; private set; }
    public bool IsGeneratingMaze { get; set; }
    public bool IsGenerationPaused { get; private set; }

    #endregion
    
    private readonly int originalGenerationMillisecondDelay = 32;
    
    private int currentSpeedModifierIndex = 4;
    private readonly float[] generationSpeedModifiers = { 1/16f, 1/8f, 1/4f, 1/2f, 1, 2, 4, 8 };
    
    
    protected override void Awake()
    {
        base.Awake();
        
        CurrentGenerationMillisecondDelay = originalGenerationMillisecondDelay;
    }

    private void Start()
    {
        if (DataManager.HAS_SAVED_GENERATION_SPEED())
            currentSpeedModifierIndex = DataManager.GET_SAVED_GENERATION_SPEED();
        
        UpdateCurrentSpeedModifier();
        
        SetRandomStartingPosition();
    }

    private void OnEnable()
    {
        EventManager.OnMazeGenerationStart += OnStartMazeGenerationResetHelperValues;
        EventManager.OnMazeGenerationEnd += OnEndMazeGenerationResetHelperValues;
    }

    private void OnDisable()
    {
        EventManager.OnMazeGenerationStart -= OnStartMazeGenerationResetHelperValues;
        EventManager.OnMazeGenerationEnd -= OnEndMazeGenerationResetHelperValues;
    }

    
    #region METHODS
    
    public void SetRandomStartingPosition()
    {
        int randomStartingX = Random.Range(0, MazeManager.Instance.CurrentMazeSize);
        int randomStartingY = Random.Range(0, MazeManager.Instance.CurrentMazeSize);

        VisualizeStartingPoint(new Vector2Int(randomStartingX, randomStartingY));
    }

    public void VisualizeStartingPoint(Vector2Int newStartingPosition)
    {
        MazeVisualizer.Instance.SetVisualTintDefault(StartingPosition);
        StartingPosition = newStartingPosition;
        MazeVisualizer.Instance.SetVisualTintCurrentNode(StartingPosition);
    }
    
    public void DecreaseGenerationSpeed()
    {
        ChangeSpeedModifierIndexByValue(1);
    }

    public void IncreaseGenerationSpeed()
    {
        ChangeSpeedModifierIndexByValue(-1);
    }

    private void ChangeSpeedModifierIndexByValue(int index)
    {
        currentSpeedModifierIndex += index;
        currentSpeedModifierIndex = Mathf.Clamp(currentSpeedModifierIndex, 0, generationSpeedModifiers.Length - 1);

        UpdateCurrentSpeedModifier();
        
        DataManager.SAVE_MAZE_GENERATION_SPEED(currentSpeedModifierIndex);
    }

    private void UpdateCurrentSpeedModifier()
    {
        CurrentGenerationMillisecondDelay = (int)(originalGenerationMillisecondDelay * generationSpeedModifiers[currentSpeedModifierIndex]);
        EventManager.RaiseOnSpeedModifierChange(1 / generationSpeedModifiers[currentSpeedModifierIndex]);
    }
    
    private void OnStartMazeGenerationResetHelperValues()
    {
        CheckIfCurrentStartingPositionInBounds();
        
        IsGeneratingMaze = true;
        IsGenerationPaused = false;
    }

    private void CheckIfCurrentStartingPositionInBounds()
    {
        if (!MazeManagerHelperFunction.IsInMazeBounds(StartingPosition))
            SetRandomStartingPosition();
    }

    private void OnEndMazeGenerationResetHelperValues()
    {
        IsGeneratingMaze = false;
    }
    
    public void PauseOrResumeMazeGeneration()
    {
        if (IsGeneratingMaze)
            IsGenerationPaused = !IsGenerationPaused;
    }

    #endregion
    
}
