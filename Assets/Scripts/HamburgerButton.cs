using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamburgerButton : MonoBehaviour
{
    public GameObject Menu;

    public void ToggleMenu()
    {
        if (Menu != null)
            Menu.SetActive(!Menu.activeSelf);
    }
}
