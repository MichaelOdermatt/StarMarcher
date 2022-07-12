using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public int LevelNumber;
    public LevelCompletionStatus CompletionStatus;
    private Image Image;

    public Sprite LevelCompletedTexture;
    public Sprite LevelNotCompletedTexture;
    public Sprite NextLevelTexture;

    public void Start()
    {
        Image = GetComponent<Image>();

        switch (CompletionStatus)
        {
            case LevelCompletionStatus.Completed:
                Image.sprite = LevelCompletedTexture;
                break;
            case LevelCompletionStatus.NotCompleted:
                Image.sprite = LevelNotCompletedTexture;
                break;
            case LevelCompletionStatus.NextLevel:
                Image.sprite  = NextLevelTexture;
                break;
        }
    }

    public void OnClick()
    {
        SceneManager.LoadScene($"Level{LevelNumber}");
    }
}
