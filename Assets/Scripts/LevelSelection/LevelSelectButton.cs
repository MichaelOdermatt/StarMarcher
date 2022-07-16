using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelSelectButton : MonoBehaviour
{
    public int LevelNumber;
    public LevelCompletionStatus CompletionStatus;
    private Image Image;
    private Button Button;
    private SceneNav SceneNav;
    private string LevelName;

    public Sprite LevelCompletedTexture;
    public Sprite LevelNotCompletedTexture;
    public Sprite NextLevelTexture;

    private void Awake()
    {
        Button = GetComponent<Button>();
        Image = GetComponent<Image>();
        SceneNav = new SceneNav();
        LevelName = $"Level{LevelNumber}";
    }

    public void InitializeButton()
    {
        Shadow shadow;

        switch (CompletionStatus)
        {
            case LevelCompletionStatus.Completed:
                Image.sprite = LevelCompletedTexture;

                break;
            case LevelCompletionStatus.NotCompleted:
                Image.sprite = LevelNotCompletedTexture;
                Button.enabled = false;
                if (TryGetComponent(out shadow))
                    shadow.enabled = false;

                break;
            case LevelCompletionStatus.NextLevel:
                Image.sprite = NextLevelTexture;

                break;
        }
    }

    public void OnClick()
    {
        SceneNav.LoadScene(LevelName);
    }
}
