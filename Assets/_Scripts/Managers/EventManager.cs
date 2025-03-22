using UnityEngine.Events;

public static class EventManager
{
    
    public static event UnityAction<int> OnSizeSliderValueChanged;
    public static event UnityAction OnMazeGenerationStart;
    public static event UnityAction OnMazeGenerationEnd;
    public static event UnityAction<int> OnDropdownMazeSelectionChanged;
    public static event UnityAction<float> OnSpeedModifierChange;
    
    
    public static void RaiseOnSizeSliderValueChanged(int value)
    {
        OnSizeSliderValueChanged?.Invoke(value);
    }

    public static void RaiseOnMazeGenerationStart()
    {
        OnMazeGenerationStart?.Invoke();
    }

    public static void RaiseOnMazeGenerationEnd()
    {
        OnMazeGenerationEnd?.Invoke();
    }
    
    public static void RaiseOnDropdownMazeSelectionChanged(int value)
    {
        OnDropdownMazeSelectionChanged?.Invoke(value);
    }

    public static void RaiseOnSpeedModifierChange(float currentGenerationMillisecondDelay)
    {
        OnSpeedModifierChange?.Invoke(currentGenerationMillisecondDelay);
    }
}
