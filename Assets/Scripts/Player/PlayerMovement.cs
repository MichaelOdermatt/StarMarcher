using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float RotationSpeed = 0f;
    public float RotationSpeedMultiplier = 0.8f;
    public float DecelerationAmount = 0.015f;
    public float BaseLaunchForce = 200f;
    public float LaunchMultiplier = 25f;
    public Vector3 VelocityBeforeCollision;

    // Radius must be a little bigger than the radius of the node
    // because a collision will occur with the node you are launching off
    // if they are the same size.
    private float RotationRadius = 1.1f;
    private float Angle = 0;
    private Rigidbody2D PlayerRigidBody;

    /// <summary>
    /// The transform of the node the player is rotating around, if null it means the
    /// player is not attached to any nodes.
    /// </summary>
    public Transform AttachedNodeTransform { get; private set; }

    private void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        VelocityBeforeCollision = PlayerRigidBody.velocity;
    }

    private void FixedUpdate()
    {
        if (AttachedNodeTransform != null)
            RotateAroundNode();
    }

    private void RotateAroundNode()
    {
        if (!PlayerRigidBody.isKinematic)
            PlayerRigidBody.velocity = Vector3.zero;
            PlayerRigidBody.isKinematic = true;

        Angle += RotationSpeed * Time.deltaTime;
        if (RotationSpeed > 0)
            RotationSpeed -= DecelerationAmount;
        else if (RotationSpeed < 0)
            RotationSpeed += DecelerationAmount;

        var offset = new Vector3(Mathf.Sin(Angle), Mathf.Cos(Angle)) * RotationRadius;
        PlayerRigidBody.transform.position = AttachedNodeTransform.position + offset;
    }

    public void StartRotation(Transform nodeTransform)
    {
        AttachedNodeTransform = nodeTransform;

        Vector2 playerVelocity = VelocityBeforeCollision.normalized;
        Vector2 fromNodeToPlayer = CalculateLaunchVector().normalized;

        var rotationDir = CalculateRotationDirection(playerVelocity, fromNodeToPlayer);

        RotationSpeed = (VelocityBeforeCollision.magnitude * RotationSpeedMultiplier) * rotationDir;

        var playerVector = AttachedNodeTransform.InverseTransformPoint(PlayerRigidBody.transform.position);
        Angle = Mathf.Deg2Rad * Angle360(playerVector, Vector2.up);
    }

    public void Launch()
    {
        Vector2 launchVector = CalculateLaunchVector().normalized;
        AttachedNodeTransform = null;
        PlayerRigidBody.isKinematic = false;
        var launchForce = BaseLaunchForce + (Mathf.Abs(RotationSpeed) * LaunchMultiplier);
        PlayerRigidBody.AddForce(launchVector * launchForce);
    }

    public void DisablePlayerMovement()
    {
        PlayerRigidBody.velocity = Vector2.zero;
        PlayerRigidBody.angularVelocity = 0f;
        PlayerRigidBody.isKinematic = true;
    }

    private Vector2 CalculateLaunchVector()
    {
        return PlayerRigidBody.transform.position - AttachedNodeTransform.position;
    }

    private static int CalculateRotationDirection(Vector2 vec1, Vector2 vec2)
    {
        var angle = Angle360(vec1, vec2);
        return angle > 180 ? -1 : 1;
    }

    // https://answers.unity.com/questions/1164731/need-help-getting-angles-to-work-in-360-degrees.html
    public static float Angle360(Vector2 from, Vector2 to)
    {
        float angle = Vector2.SignedAngle(from, to);
        return angle < 0 ? 360 - angle * -1 : angle;
    }
}
