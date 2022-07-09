using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NodeSwingOnly : MonoBehaviour
{
    private SpriteRenderer Renderer;

    public void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void OnCollisionWithPlayer()
    {
        SetOpacity(0.5f);
    }

    public void OnExitWithPlayer()
    {
        SetOpacity(1f);
    }

    private void SetOpacity(float alpha)
    {
        var currentColor = Renderer.color;
        currentColor.a = alpha;
        Renderer.color = currentColor;
    }
}
