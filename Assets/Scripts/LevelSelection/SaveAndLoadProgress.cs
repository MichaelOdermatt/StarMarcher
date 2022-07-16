using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveAndLoadProgress
{
    /// <summary>
    /// Saves the active scene as a marker for how far the player has progressed.
    /// If the active scene's level number is less than the previously saved one (i.e. you are replaying a level),
    /// the active scene's level wont be saved.
    /// </summary>
    public static void SaveGame()
    {
        string levelName = SceneManager.GetActiveScene().name;
        int completedLevelNumber = GetLevelNumber(levelName);
        int savedLevelNumber = LoadCurrentLevelNumber();

        if (completedLevelNumber > savedLevelNumber)
            PlayerPrefs.SetInt("CurrentLevel", completedLevelNumber);
    }

    public static int LoadCurrentLevelNumber()
    {
        return PlayerPrefs.GetInt("CurrentLevel", 0);
    }

    public static int GetLevelNumber(string levelName)
    {
        Regex pattern = new Regex(@"[0-9]+");
        string numbers = pattern.Match(levelName).Groups[0].Value;
        Int32.TryParse(numbers, out int levelNumber);

        return levelNumber;
    }
}
