using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveAndLoadProgress
{
    /// <summary>
    /// Saves the current scene as a marker for how far the player has progressed.
    /// If the current scene's level number is less than the previously saved one (i.e. you are replaying a level),
    /// the current scene's level wont be saved.
    /// </summary>
    public static void SaveGame()
    {
        string levelName = SceneManager.GetActiveScene().name;
        int completedLevelNumber = GetLevelNumber(levelName);
        int savedLevelNumber = GetLevelNumber(LoadCurrentLevel());

        // perhaps I should just save the int rather than the whole level name.
        if (completedLevelNumber > savedLevelNumber)
            PlayerPrefs.SetString("CurrentLevel", levelName);
    }

    public static string LoadCurrentLevel()
    {
        return PlayerPrefs.GetString("CurrentLevel", "Level1");
    }

    public static int GetLevelNumber(string levelName)
    {
        Regex pattern = new Regex(@"[0-9]+");
        string numbers = pattern.Match(levelName).Groups[0].Value;
        Int32.TryParse(numbers, out int levelNumber);

        return levelNumber;
    }
}
