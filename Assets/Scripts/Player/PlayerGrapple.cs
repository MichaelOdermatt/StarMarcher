using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all code for creating and rendering the Grapple.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(HingeJoint2D))]
public class PlayerGrapple : MonoBehaviour
{
    public float MinGrappleDistance = 1.1f;
    public float GrappleSpeed = 7;

    private Rigidbody2D PlayerRigidBody;
    private LineRenderer LineRenderer;
    private HingeJoint2D Hinge;

    /// <summary>
    /// Enables or disables the HingeJoint2D component.
    /// </summary>
    public bool IsEnabled { get => Hinge.enabled; set => Hinge.enabled = value; }

    private void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.positionCount = 2;
        LineRenderer.enabled = false;

        Hinge = GetComponent<HingeJoint2D>();
        Hinge.enabled = false;
    }

    public void UpdateGrapple(Vector3 clickLocation)
    {
        if (Hinge.enabled == true)
        {
            RemoveHinge();
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(clickLocation, Vector2.zero);

        if (hit == false 
            || hit.collider.gameObject == gameObject
            || MinGrappleDistance >= Vector2.Distance(
                hit.collider.transform.position, 
                PlayerRigidBody.transform.position))
            return;

        StartCoroutine(DrawGrapple(hit.collider.gameObject.transform.position, hit));
    }

    public void RemoveHinge()
    {
        DisableLineRenderer();
        Hinge.anchor = Vector2.zero;
        Hinge.enabled = false;
    }

    public void SetHinge(GameObject node)
    {
        Hinge.enabled = true;
        Hinge.anchor = PlayerRigidBody.transform.InverseTransformPoint(node.transform.position);
    }

    public IEnumerator DrawGrapple(Vector3 point, RaycastHit2D hit)
    {
        LineRenderer.enabled = true;

        float time = 0;
        Vector3 startPos = PlayerRigidBody.transform.position;

        while (time < 1)
        {
            LineRenderer.SetPosition(1, Vector3.Lerp(startPos, point, time));
            time += GrappleSpeed * Time.deltaTime;

            yield return null;
        }

        // TODO fix bug when the user clicks on a node again before the line is fully drawn
        // TODO fix bug where user hits a node before the line is fully drawn
        LineRenderer.SetPosition(1, point);
        SetHinge(hit.collider.gameObject);
    }

    public void UpdateLine()
    {
        if (LineRenderer.enabled)
            LineRenderer.SetPosition(0, PlayerRigidBody.transform.position);
    }

    /// <summary>
    /// It is reccomended that you use this rather than set LineRenderer.enabled = false.
    /// </summary>
    public void DisableLineRenderer()
    {
        LineRenderer.enabled = false;
        LineRenderer.SetPositions(new[] { Vector3.zero, Vector3.zero });
    }
}
