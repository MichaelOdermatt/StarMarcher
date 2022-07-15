using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    public void OnClick()
    {
        string levelName = SceneManager.GetActiveScene().name;
        int levelNumber = SaveAndLoadProgress.GetLevelNumber(levelName);
        string nextLevelName = $"Level{levelNumber + 1}";

        if (Application.CanStreamedLevelBeLoaded(nextLevelName))
            SceneManager.LoadScene(nextLevelName);
    }
}
