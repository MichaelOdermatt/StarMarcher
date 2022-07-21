using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws the outline of the box based on the collider.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class DrawBoxOutline : MonoBehaviour
{
    private LineRenderer LineRenderer;
    private BoxCollider2D BoxCollider;

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.positionCount = 4;
        LineRenderer.loop = true;
    }

    private void Start()
    {
        SetPoints();
    }

    private void SetPoints()
    {
        var bounds = BoxCollider.bounds.extents;
        Vector3 vertrex1Pos = new Vector3(-bounds.x, bounds.y) + transform.position;
        Vector3 vertrex2Pos = new Vector3(bounds.x, bounds.y) + transform.position;
        Vector3 vertrex3Pos = new Vector3(bounds.x, -bounds.y) + transform.position;
        Vector3 vertrex4Pos = new Vector3(-bounds.x, -bounds.y) + transform.position;
        LineRenderer.SetPosition(0, vertrex1Pos);
        LineRenderer.SetPosition(1, vertrex2Pos);
        LineRenderer.SetPosition(2, vertrex3Pos);
        LineRenderer.SetPosition(3, vertrex4Pos);
    }
}
