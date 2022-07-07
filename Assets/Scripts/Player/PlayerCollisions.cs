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
    public Action<Collider2D> CollisionWithNode;
    public Action<Collider2D> CollisionWithNodeSwingOnly;
    public Action<Collider2D> ExitFromNodeSwingOnly;
    public Action KillPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CustomTag tags;
        if (!collision.collider.TryGetComponent(out tags))
            return;

        if (tags.Tags.HasFlag(CustomTag.TagTypes.Rotatable))
        {
            CollisionWithNode(collision.collider);
        }

        if (tags.Tags.HasFlag(CustomTag.TagTypes.KillsPlayerOnContact))
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

        if (IsNodeSwingOnly (collider.gameObject))
            CollisionWithNodeSwingOnly(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (IsNodeSwingOnly(collider.gameObject))
            ExitFromNodeSwingOnly(collider);
    }

    private bool IsNodeSwingOnly(GameObject gameObject)
    {
        if (gameObject.CompareTag("Node"))
        {
            CustomTag tags;
            if (gameObject.TryGetComponent(out tags))
                if (!tags.Tags.HasFlag(CustomTag.TagTypes.Rotatable))
                    return true;
        }

        return false;
    }
}
