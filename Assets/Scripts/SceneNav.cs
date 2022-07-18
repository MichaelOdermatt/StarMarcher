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
    }
    
    // TODO remove the function of the same name from the main camera prefab.
    public void ResetCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
