using System;
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
    public float MinGrappleDistance = 1.5f;
    public float GrappleSpeed = 7;

    /// <summary>
    /// If true, then a coroutine is running which is drawing the grapple.
    /// </summary>
    public bool IsDrawingGrapple { get; private set; } = false;
    public ParticleSystem GrappleParticleSystem;
    public Action LaunchGrappleSound;
    public Action ConnectGrappleSound;

    private Coroutine DrawThenSetGrappleCoroutine;
    private Rigidbody2D PlayerRigidBody;
    private LineRenderer LineRenderer;
    private HingeJoint2D Hinge;

    /// <summary>
    /// Enables or disables the HingeJoint2D component.
    /// </summary>
    public bool IsEnabled
    {
        get => Hinge.enabled;
        set
        {
            if (Hinge != null)
                Hinge.enabled = value;
        }
    }

    private void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.positionCount = 2;
        LineRenderer.enabled = false;

        Hinge = GetComponent<HingeJoint2D>();
        Hinge.enabled = false;
    }

    public void AttemptGrapple(Vector3 clickLocation)
    {
        RaycastHit2D hit = Physics2D.Raycast(clickLocation, Vector2.zero);
        if (hit.collider == null)
            return;

        GameObject node = hit.collider.gameObject;
        CustomNodeTag tags = GetNodeTag(node);

        var distanceToNode = Vector2.Distance(
                node.transform.position, 
                PlayerRigidBody.transform.position);

        if (!IsDrawingGrapple 
            && tags != null
            && tags.Tags.HasFlag(CustomNodeTag.TagTypes.Swingable)
            && distanceToNode >= MinGrappleDistance)
        {
            if (LaunchGrappleSound != null)
                LaunchGrappleSound();

            DrawThenSetGrappleCoroutine = StartCoroutine(
                DrawThenSetGrapple(node.transform.position));
        }
    }

    public void RemoveHinge()
    {
        DisableLineRenderer();
        Hinge.anchor = Vector2.zero;
        Hinge.enabled = false;
        if (LaunchGrappleSound != null)
            LaunchGrappleSound();
    }

    private void SetHinge(Vector2 hingePos)
    {
        Hinge.enabled = true;
        Hinge.anchor = PlayerRigidBody.transform.InverseTransformPoint(hingePos);
        Hinge.connectedAnchor = hingePos;
    }

    private IEnumerator DrawThenSetGrapple(Vector3 hingePos)
    {
        IsDrawingGrapple = true;
        LineRenderer.enabled = true;

        float time = 0;
        Vector3 startPos = PlayerRigidBody.transform.position;

        while (time < 1)
        {
            LineRenderer.SetPosition(1, Vector3.Lerp(startPos, hingePos, time));
            time += GrappleSpeed * Time.deltaTime;

            yield return null;
        }

        LineRenderer.SetPosition(1, hingePos);
        SetHinge(hingePos);
        PlayGrappleParticle(hingePos);
        if (ConnectGrappleSound != null)
            ConnectGrappleSound();

        IsDrawingGrapple = false;
    }

    /// <summary>
    /// Stops the coroutine which draws and sets the grapple.
    /// </summary>
    public void StopDrawingThenSetGrapple()
    {
        StopCoroutine(DrawThenSetGrappleCoroutine);
        IsDrawingGrapple = false;
    }

    public void UpdateLine()
    {
        if (LineRenderer.enabled)
            LineRenderer.SetPosition(0, PlayerRigidBody.transform.position);
    }

    private void PlayGrappleParticle(Vector3 position)
    {
        if (GrappleParticleSystem != null)
        {
            GrappleParticleSystem.transform.position = position;
            GrappleParticleSystem.Play();
        }
    }

    /// <summary>
    /// Returns Null if GameObject does not have a CustomNodeTag component.
    /// </summary>
    private CustomNodeTag GetNodeTag(GameObject gameObject)
    {
        CustomNodeTag tags;

        // we check if the objects parent has node tags too because of the dotted
        // line game object that exists around swingable nodes.
        if (gameObject.TryGetComponent(out tags)
            || gameObject.transform.parent.TryGetComponent(out tags))
        {
            return tags;
        } else
            return null;
    }

    /// <summary>
    /// It is recommended that you use this rather than set LineRenderer.enabled = false.
    /// </summary>
    public void DisableLineRenderer()
    {
        LineRenderer.enabled = false;
        LineRenderer.SetPositions(new[] { Vector3.zero, Vector3.zero });
    }
}
