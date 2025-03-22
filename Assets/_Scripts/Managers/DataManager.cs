using UnityEngine;

public static class DataManager 
{

    public static void SAVE_MAZE_SIZE()
    {
        PlayerPrefs.SetInt("SAVE_SIZE", MazeManager.Instance.CurrentMazeSize);
    }
    
    public static void SAVE_MAZE_ALGORITHM(int currentMazeIndex)
    {
        PlayerPrefs.SetInt("SAVE_ALGORITHM", currentMazeIndex);
    }
    
    public static void SAVE_MAZE_GENERATION_SPEED(int currentSpeedIndex)
    {
        PlayerPrefs.SetInt("SAVE_SPEED", currentSpeedIndex);
    }
    
    public static int GET_SAVED_SIZE() => PlayerPrefs.GetInt("SAVE_SIZE");

    public static int GET_SAVED_ALGORITHM() => PlayerPrefs.GetInt("SAVE_ALGORITHM");
    
    public static int GET_SAVED_GENERATION_SPEED() => PlayerPrefs.GetInt("SAVE_SPEED");
    
    
    public static bool HAS_SAVED_SIZE() => PlayerPrefs.HasKey("SAVE_SIZE");

    public static bool HAS_SAVED_ALGORITHM() => PlayerPrefs.HasKey("SAVE_ALGORITHM");
    
    public static bool HAS_SAVED_GENERATION_SPEED() => PlayerPrefs.HasKey("SAVE_SPEED");
    
}
