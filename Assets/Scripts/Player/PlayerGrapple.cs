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

        Hinge = GetComponent<HingeJoint2D>();
        Hinge.enabled = false;
    }

    public void UpdateGrapple(Vector3 clickLocation)
    {
        if (Hinge.enabled == true)
        {
            RemoveGrapple();
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(clickLocation, Vector2.zero);

        if (hit == false 
            || hit.collider.gameObject == gameObject
            || MinGrappleDistance >= Vector2.Distance(
                hit.collider.transform.position, 
                PlayerRigidBody.transform.position))
            return;

        SetGrapple(hit);
    }

    public void RemoveGrapple()
    {
        Hinge.anchor = Vector2.zero;
        Hinge.enabled = false;
    }

    public void SetGrapple(RaycastHit2D hit)
    {
        Hinge.enabled = true;
        var node = hit.collider.gameObject;
        Hinge.anchor = PlayerRigidBody.transform.InverseTransformPoint(node.transform.position);
    }

    public void UpdateLine()
    {
        LineRenderer.SetPosition(0, PlayerRigidBody.transform.position);
        LineRenderer.SetPosition(1, PlayerRigidBody.transform.TransformPoint(Hinge.anchor));
    }
}
