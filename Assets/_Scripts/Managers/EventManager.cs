using System;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    
    public static event UnityAction<int> OnSizeSliderValueChanged;


    public static void RaiseOnSizeSliderValueChanged(int value)
    {
        OnSizeSliderValueChanged?.Invoke(value);
    }
}
