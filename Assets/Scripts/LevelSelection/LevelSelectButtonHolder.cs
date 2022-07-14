using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButtonHolder : MonoBehaviour
{
    public List<LevelSelectButton> levelSelectButtons = new List<LevelSelectButton>();

    void Start()
    {
        string completedLevelName = SaveAndLoadProgress.LoadCurrentLevel();
        int CompletedLevelNumber = SaveAndLoadProgress.GetLevelNumber(completedLevelName);
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
}
