using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LevelSelectButtonHolder : MonoBehaviour
{
    public List<LevelSelectButton> levelSelectButtons = new List<LevelSelectButton>();
    private string CompletedLevelName;

    void Start()
    {
        CompletedLevelName = PlayerPrefs.GetString("CurrentLevel", "Level1");
        int CompletedLevelNumber = GetLevelNumber(CompletedLevelName);
        int nextLevelNumber = CompletedLevelNumber + 1;

        for (int i = 0; i < levelSelectButtons.Count - 1; i++)
        {
            if (levelSelectButtons[i].LevelNumber < nextLevelNumber)
                levelSelectButtons[i].CompletionStatus = LevelCompletionStatus.Completed;
            else if (levelSelectButtons[i].LevelNumber == nextLevelNumber)
                levelSelectButtons[i].CompletionStatus = LevelCompletionStatus.NextLevel;
            else if (levelSelectButtons[i].LevelNumber > nextLevelNumber)
                levelSelectButtons[i].CompletionStatus = LevelCompletionStatus.NotCompleted;
        }
    }

    private int GetLevelNumber(string levelName)
    {
        Regex pattern = new Regex(@"[0-9]+");
        string numbers = pattern.Match(levelName).Groups[0].Value;
        Int32.TryParse(numbers, out int levelNumber);

        return levelNumber;
    }
}
