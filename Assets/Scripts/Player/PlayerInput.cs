using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks for player input.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    private Camera Camera;
    public Action<Vector3> clicked;

    private void Awake()
    {
        Camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            clicked(Camera.ScreenToWorldPoint(Input.mousePosition));
    }
}
