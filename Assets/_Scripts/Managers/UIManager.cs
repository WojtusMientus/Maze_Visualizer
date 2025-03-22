using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : SingletonMonoBehaviour<UIManager>
{
    
    #region VARIABLES
    
    [SerializeField] private TextMeshProUGUI textCurrentMazeSize;
    [SerializeField] private Slider sliderMazeSize;
    
    [Header("Buttons")]
    [SerializeField] private Button buttonGenerateMaze;
    [SerializeField] private Button buttonGetRandomStartingPoint;
    [SerializeField] private Button buttonSliderArea;
    [SerializeField] private TMP_Dropdown dropdownAlgorithmSelection;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI textAlgorithmTitle;
    [SerializeField] private TextMeshProUGUI textAlgorithmDescriptionTitle;
    [SerializeField] private TextMeshProUGUI textCurrentSpeedModifier;
    
    [Header("Algo Description Elements")]
    [SerializeField] private TextMeshProUGUI textAlgorithmDescription;
    
    [Space(15)]
    [SerializeField] private AlgorithmsDescription algorithmDescription;
    
    #endregion
    
    
    protected override void Awake()
    {
        GenerateDropdown();
        base.Awake();
    }

    private void Start()
    {
        LoadSavedData();
        
        sliderMazeSize.onValueChanged.Invoke(sliderMazeSize.value);
        dropdownAlgorithmSelection.onValueChanged.Invoke(dropdownAlgorithmSelection.value);
    }
    
    private void OnEnable()
    {
        EventManager.OnSizeSliderValueChanged += UpdateMazeSizeText;
        EventManager.OnMazeGenerationStart += DisableUI;
        EventManager.OnMazeGenerationEnd += EnableUI;
        EventManager.OnSpeedModifierChange += UpdateSpeedModifierText;
    }
    
    private void OnDisable()
    {
        EventManager.OnSizeSliderValueChanged -= UpdateMazeSizeText;
        EventManager.OnMazeGenerationStart -= DisableUI;
        EventManager.OnMazeGenerationEnd -= EnableUI;
        EventManager.OnSpeedModifierChange -= UpdateSpeedModifierText;

    }

    #region METHODS
    
    private void GenerateDropdown()
    {
        List<string> algorithmsTitles = new List<string>(); 
        
        foreach (MazeAlgorithmDescription algoTitle in algorithmDescription.Descriptions)
            algorithmsTitles.Add(algoTitle.Title);
        
        dropdownAlgorithmSelection.ClearOptions();
        dropdownAlgorithmSelection.AddOptions(algorithmsTitles);

        UpdateTitleTexts(0);
    }

    private void UpdateTitleTexts(int value)
    {
        textAlgorithmTitle.text = algorithmDescription.Descriptions[value].Title;
        textAlgorithmDescriptionTitle.text = "How \"" + algorithmDescription.Descriptions[value].Title + "\" algorithm works";
        textAlgorithmDescription.text = algorithmDescription.Descriptions[value].AlgoDescription;
    }


    private void LoadSavedData()
    {
        if (DataManager.HAS_SAVED_SIZE())
            sliderMazeSize.value = DataManager.GET_SAVED_SIZE();
        
        if (DataManager.HAS_SAVED_ALGORITHM())
            dropdownAlgorithmSelection.value = DataManager.GET_SAVED_ALGORITHM();
    }
    
    private void UpdateMazeSizeText(int newSize)
    {
        textCurrentMazeSize.text = newSize.ToString();
    }

    public void OnSliderValueChange(Single value)
    {
        EventManager.RaiseOnSizeSliderValueChanged((int)value);
        
        DataManager.SAVE_MAZE_SIZE();
    }


    public void OnDropdownValueChange(int value)
    {
        UpdateTitleTexts(value);
        EventManager.RaiseOnDropdownMazeSelectionChanged(value);
        
        DataManager.SAVE_MAZE_ALGORITHM(value);
    }

    private void UpdateSpeedModifierText(float value)
    {
        textCurrentSpeedModifier.text = value + "x";
    }
    
    private void DisableUI()
    {
        SetUIEnabled(false);
    }

    private void EnableUI()
    {
        SetUIEnabled(true);
    }

    private void SetUIEnabled(bool isEnabled)
    {
        sliderMazeSize.interactable = isEnabled;
        buttonGenerateMaze.interactable = isEnabled;
        dropdownAlgorithmSelection.interactable = isEnabled;
        buttonGetRandomStartingPoint.interactable = isEnabled;
        buttonSliderArea.interactable = isEnabled;
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    
    #endregion
    
}
