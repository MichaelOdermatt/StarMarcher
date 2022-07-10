using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public bool IsCollected = false;
    // Delegate used to get the objective manager to
    // check if all the objectives are collected.
    public Action CheckObjectives;
    public SpriteRenderer SpriteRenderer;
    public SpriteRenderer ShadowSpriteRenderer;

    public void Collect()
    {
        if (IsCollected) 
            return;

        IsCollected = true;
        SpriteRenderer.enabled = false;
        ShadowSpriteRenderer.enabled = false;

        CheckObjectives();
    }
}
