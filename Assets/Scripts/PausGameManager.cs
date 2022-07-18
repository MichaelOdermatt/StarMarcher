using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PausGameManager 
{
    public static void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public static void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
}
