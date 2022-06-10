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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Node"))
            CollisionWithNode(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Component component;

        if (collider.TryGetComponent(typeof(Objective), out component))
            CollisionWithObjective(collider);
    }
}
