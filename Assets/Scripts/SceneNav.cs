using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNav : MonoBehaviour
{
    public void GoToLevelSelect()
    {
        string levelName = "LevelSelect";
        LoadScene(levelName);
    }

    public void GoToMainMenu()
    {
        string levelName = "MainMenu";
        LoadScene(levelName);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
            SceneManager.LoadScene(sceneName);

        // unpause the game in case it was previously paused
        PausGameManager.UnpauseGame();
    }
    
    public void ResetCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // unpause the game in case it was previously paused
        PausGameManager.UnpauseGame();
    }
}
