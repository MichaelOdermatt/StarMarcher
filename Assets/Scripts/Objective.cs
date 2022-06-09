using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Objective : MonoBehaviour
{
    public bool IsCollected = false;
    private SpriteRenderer SpriteRenderer;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    public void Collect()
    {
        if (IsCollected) 
            return;

        IsCollected = true;
        SpriteRenderer.enabled = false;
    }
}
