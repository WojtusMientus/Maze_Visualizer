using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private TextMeshProUGUI sizeText;
    [SerializeField] private Slider mazeSizeSlider;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        mazeSizeSlider.onValueChanged.Invoke(mazeSizeSlider.value);
    }


    private void OnEnable()
    {
        EventManager.OnSizeSliderValueChanged += UpdateSizeText;
    }


    private void OnDisable()
    {
        EventManager.OnSizeSliderValueChanged -= UpdateSizeText;
    }

    private void UpdateSizeText(int newSize)
    {
        sizeText.text = newSize.ToString();
    }

    public void OnSliderValueChange(Single value)
    {
        EventManager.RaiseOnSizeSliderValueChanged((int)value);
    }
}
