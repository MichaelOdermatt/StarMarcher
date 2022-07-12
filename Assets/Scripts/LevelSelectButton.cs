using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public int LevelNumber;

    public void OnClick()
    {
        SceneManager.LoadScene($"Level{LevelNumber}");
    }
}
