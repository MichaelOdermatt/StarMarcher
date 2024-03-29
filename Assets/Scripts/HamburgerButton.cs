using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamburgerButton : MonoBehaviour
{
    public GameObject Menu;

    public void ToggleMenu()
    {
        if (Menu == null)
            return;

        Menu.SetActive(!Menu.activeSelf);

        if (Menu.activeSelf == true)
            PausGameManager.PauseGame();
        else
            PausGameManager.UnpauseGame();
    }
}
