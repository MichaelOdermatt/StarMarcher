using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    public Action<Vector3> clicked;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            clicked(Camera.ScreenToWorldPoint(Input.mousePosition));
    }
}
