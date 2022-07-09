using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks for player collisions.
/// </summary>
public class PlayerCollisions : MonoBehaviour
{
    public Action<Collider2D> CollisionWithObjective;
    public Action<Collider2D> CollisionWithNodeRotatable;
    public Action<Collider2D> CollisionWithNodeSwingOnly;
    public Action<Collider2D> ExitFromNodeSwingOnly;
    public Action KillPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CustomNodeTag tags;
        if (!collision.collider.TryGetComponent(out tags))
            return;

        if (tags.Tags.HasFlag(CustomNodeTag.TagTypes.KillsPlayerOnContact))
        {
            KillPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Component component;
        if (collider.TryGetComponent(typeof(Objective), out component))
        {
            CollisionWithObjective(collider);
            return;
        }

        CustomNodeTag tags;
        if (!collider.TryGetComponent(out tags))
            return;

        if (!tags.Tags.HasFlag(CustomNodeTag.TagTypes.Rotatable))
            CollisionWithNodeSwingOnly(collider);
        else if (tags.Tags.HasFlag(CustomNodeTag.TagTypes.Rotatable))
            CollisionWithNodeRotatable(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        CustomNodeTag tags;
        if (!collider.TryGetComponent(out tags))
            return;

        if (!tags.Tags.HasFlag(CustomNodeTag.TagTypes.Rotatable))
            ExitFromNodeSwingOnly(collider);
    }
}
