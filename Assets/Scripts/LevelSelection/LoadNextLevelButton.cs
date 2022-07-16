using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class LoadNextLevelButton : MonoBehaviour
{
    public Image Image;
    public Button Button;
    private SceneNav SceneNav;

    public void Awake()
    {
        SceneNav = new SceneNav();
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

        SceneNav.LoadScene(nextLevelName);
    }
}
