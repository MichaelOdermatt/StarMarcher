using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButtonHolder : MonoBehaviour
{
    public List<LevelSelectButton> levelSelectButtons = new List<LevelSelectButton>();
    private string CurrentLevelName;

    void Start()
    {
        CurrentLevelName = PlayerPrefs.GetString("CurrentLevel", "Level1");

        bool isCurrentLevelFound = false;

        for (int i = 0; i < levelSelectButtons.Count - 1; i++)
        {
            if (CurrentLevelName == levelSelectButtons[i].LevelName)
            {
                isCurrentLevelFound = true;
                levelSelectButtons[i].CompletionStatus = LevelCompletionStatus.Completed;
                levelSelectButtons[i + 1].CompletionStatus = LevelCompletionStatus.NextLevel;
                i++;
                continue;
            }

            if (isCurrentLevelFound)
                levelSelectButtons[i].CompletionStatus = LevelCompletionStatus.NotCompleted;
            else
                levelSelectButtons[i].CompletionStatus = LevelCompletionStatus.Completed;
        }
    }
}
