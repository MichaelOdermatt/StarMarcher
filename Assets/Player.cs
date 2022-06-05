using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private float RotationSpeed = 4f;
    // Radius must be a little bigger than the radius of the node
    // because a collision will occur with the node you are launching off
    // if they are the same size.
    private float Radius = 1.1f;

    private float launchForce = 250f;
    private Rigidbody2D PlayerRigidBody;
    public Transform nodeTransform;
    private float angle = 0;

    private void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (nodeTransform != null)
        {
            RotateAroundNode();
            if (Input.GetButtonDown("Fire1"))
                Launch();
        } 
    }

    private void RotateAroundNode()
    {
        angle += RotationSpeed * Time.deltaTime;

        var offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * Radius;
        transform.position = nodeTransform.position + offset;

        if (!PlayerRigidBody.isKinematic)
            PlayerRigidBody.isKinematic = true;
    }

    private void Launch()
    {
        Vector2 launchVector = CalculateLaunchVector();
        nodeTransform = null;
        PlayerRigidBody.isKinematic = false;
        PlayerRigidBody.AddForce(launchVector * launchForce); 
    }

    private Vector2 CalculateLaunchVector()
    {
        return PlayerRigidBody.transform.position - nodeTransform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (nodeTransform != null)
            return;

        nodeTransform = collision.collider.transform;
    }

}
