using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private TextMeshProUGUI sizeText;
    [SerializeField] private Slider sliderMazeSize;
    
    [Header("Buttons")]
    [SerializeField] private Button buttonGenerateMaze;
    [SerializeField] private Button buttonGetRandomStartingPoint;
    [SerializeField] private TMP_Dropdown dropdownAlgorithmSelection;
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI speedModiferText;
    
    
    
    [Space(15)]
    [SerializeField] private AlgorithmsDescription algorithmDescription;
    
    
    protected override void Awake()
    {
        GenerateDropdown();
        base.Awake();
    }

    private void Start()
    {
        sliderMazeSize.onValueChanged.Invoke(sliderMazeSize.value);
    }


    private void OnEnable()
    {
        EventManager.OnSizeSliderValueChanged += UpdateSizeText;
        EventManager.OnMazeGenerationStart += DisableUI;
        EventManager.OnMazeGenerationEnd += EnableUI;
        EventManager.OnSpeedModifierChange += UpdateSpeedModifierText;
    }


    private void OnDisable()
    {
        EventManager.OnSizeSliderValueChanged -= UpdateSizeText;
        EventManager.OnMazeGenerationStart -= DisableUI;
        EventManager.OnMazeGenerationEnd -= EnableUI;
        EventManager.OnSpeedModifierChange -= UpdateSpeedModifierText;

    }

    private void GenerateDropdown()
    {
        List<string> algorithmsTitles = new List<string>(); 
        
        foreach (MazeAlgorithmDescription algoTitle in algorithmDescription.Descriptions)
            algorithmsTitles.Add(algoTitle.Title);
        
        dropdownAlgorithmSelection.ClearOptions();
        dropdownAlgorithmSelection.AddOptions(algorithmsTitles);
        
        titleText.text = algorithmDescription.Descriptions[0].Title;
    }

    
    private void UpdateSizeText(int newSize)
    {
        sizeText.text = newSize.ToString();
    }

    public void OnSliderValueChange(Single value)
    {
        EventManager.RaiseOnSizeSliderValueChanged((int)value);
    }


    public void OnDropdownValueChange(int value)
    {
        titleText.text = algorithmDescription.Descriptions[value].Title;
        EventManager.RaiseOnDropdownMazeSelectionChanged(value);
    }

    private void UpdateSpeedModifierText(float value)
    {
        speedModiferText.text = value + "x";
    }
    
    
    
    private void DisableUI()
    {
        SetUIEnabled(false);
    }

    private void EnableUI()
    {
        SetUIEnabled(true);
    }

    private void SetUIEnabled(bool enabled)
    {
        sliderMazeSize.enabled = enabled;
        buttonGenerateMaze.enabled = enabled;
        dropdownAlgorithmSelection.enabled = enabled;
        buttonGetRandomStartingPoint.enabled = enabled;
    }
}
