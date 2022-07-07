using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NodeNonSwingable : MonoBehaviour
{
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
        var renderer = GetComponent<SpriteRenderer>();
        var currentColor = renderer.color;
        currentColor.a = alpha;
        renderer.color = currentColor;
    }
}
