using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class LoadNextLevelButton : MonoBehaviour
{
    public Image Image;
    public Button Button;

    public void Awake()
    {
        Image = GetComponent<Image>();
        Button = GetComponent<Button>();

        Image.enabled = false;
        Button.enabled = false;
    }

    public void ShowButton()
    {
        Image.enabled = true;
        Button.enabled = true;
    }

    public void OnClick()
    {
        string levelName = SceneManager.GetActiveScene().name;
        int levelNumber = SaveAndLoadProgress.GetLevelNumber(levelName);
        string nextLevelName = $"Level{levelNumber + 1}";

        if (Application.CanStreamedLevelBeLoaded(nextLevelName))
            SceneManager.LoadScene(nextLevelName);
    }
}
