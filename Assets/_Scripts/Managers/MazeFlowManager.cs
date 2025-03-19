using System;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(1)]
public class MazeFlowManager : SingletonMonoBehaviour<MazeFlowManager>
{
    private readonly int originalGenerationMillisecondDelay = 32;
    public int CurrentGenerationMillisecondDelay { get; private set; }
    public Vector2Int StartingPosition { get; private set; }
    public bool StopMazeGeneration { get; set; }
    

    private int currentSpeedModifierIndex = 4;
    private readonly float[] generationSpeedModifiers = new float[] { 1/16f, 1/8f, 1/4f, 1/2f, 1, 2, 4, 8 };
    
    protected override void Awake()
    {
        base.Awake();
        
        CurrentGenerationMillisecondDelay = originalGenerationMillisecondDelay;
    }

    private void Start()
    {
        SetRandomStartingPosition();
    }

    private void OnEnable()
    {
        EventManager.OnMazeGenerationStart += ResetStopMazeGenerationValue;
    }

    private void OnDisable()
    {
        EventManager.OnMazeGenerationStart -= ResetStopMazeGenerationValue;
    }

    public void SetRandomStartingPosition()
    {
        int randomStartingX = Random.Range(0, MazeManager.Instance.CurrentMazeSize);
        int randomStartingY = Random.Range(0, MazeManager.Instance.CurrentMazeSize);

        MazeVisualizer.Instance.SetVisualTintDefault(StartingPosition);
        StartingPosition = new Vector2Int(randomStartingX, randomStartingY);
        MazeVisualizer.Instance.SetVisualTintCurrentNode(StartingPosition);
    }

    public void DecreaseGenerationSpeed()
    {
        ChangeSpeedModifierIndex(1);
    }

    public void IncreaseGenerationSpeed()
    {
        ChangeSpeedModifierIndex(-1);
    }

    private void ChangeSpeedModifierIndex(int i)
    {
        currentSpeedModifierIndex += i;
        currentSpeedModifierIndex = Mathf.Clamp(currentSpeedModifierIndex, 0, generationSpeedModifiers.Length - 1);
        
        CurrentGenerationMillisecondDelay = (int)(originalGenerationMillisecondDelay * generationSpeedModifiers[currentSpeedModifierIndex]);
        EventManager.RaiseOnSpeedModifierChange(1 / generationSpeedModifiers[currentSpeedModifierIndex]);
    }
    
    private void ResetStopMazeGenerationValue()
    {
        StopMazeGeneration = false;
    }
}
