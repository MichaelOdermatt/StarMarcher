using UnityEngine;

[RequireComponent(typeof(PlayerGrapple))]
[RequireComponent(typeof(PlayerCollisions))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float RotationSpeed = 0f;
    public float RotationSpeedMultiplier = 0.8f;
    public float DecelerationAmount = 0.015f;
    public float BaseLaunchForce = 200f;
    public float LaunchMultiplier = 25f;
    // Radius must be a little bigger than the radius of the node
    // because a collision will occur with the node you are launching off
    // if they are the same size.
    private float RotationRadius = 1.1f;
    private float Angle = 0;
    private Vector3 VelocityBeforeCollision = Vector3.zero;

    private float NodeCollisionCameraShakeMagnitude = 0.025f;
    private float NodeCollisionCameraShakeDuration = 0.2f;

    private Rigidbody2D PlayerRigidBody;
    public Transform NodeTransform;

    private PlayerInput PlayerInput;
    private PlayerGrapple PlayerGrapple;
    private PlayerCollisions PlayerCollisions;

    private void Awake()
    {
        PlayerGrapple = GetComponent<PlayerGrapple>();
        PlayerGrapple.IsEnabled = false;

        PlayerCollisions = GetComponent<PlayerCollisions>();
        PlayerCollisions.CollisionWithNode += OnCollisionWithNode;
        PlayerCollisions.CollisionWithObjective += OnCollisionWithObjective;

        PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.clicked += OnClicked;

        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        VelocityBeforeCollision = PlayerRigidBody.velocity;
        PlayerGrapple.UpdateLine();
    }

    private void FixedUpdate()
    {
        if (NodeTransform != null)
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
        PlayerRigidBody.transform.position = NodeTransform.position + offset;
    }

    private void Launch()
    {
        Vector2 launchVector = CalculateLaunchVector().normalized;
        NodeTransform = null;
        PlayerRigidBody.isKinematic = false;
        var launchForce = BaseLaunchForce + (Mathf.Abs(RotationSpeed) * LaunchMultiplier);
        PlayerRigidBody.AddForce(launchVector * launchForce);
    }

    private Vector2 CalculateLaunchVector()
    {
        return PlayerRigidBody.transform.position - NodeTransform.position;
    }

    private void OnCollisionWithNode(Component collider)
    {
        if (NodeTransform != null)
            return;

        if (PlayerGrapple.IsEnabled)
            PlayerGrapple.RemoveHinge();

        if (PlayerGrapple.IsDrawingGrapple)
        {
            PlayerGrapple.StopDrawThenSetGrapple();
            PlayerGrapple.DisableLineRenderer();
        }

        NodeTransform = collider.gameObject.transform;

        Vector2 playerVelocity = VelocityBeforeCollision.normalized;
        Vector2 fromNodeToPlayer = CalculateLaunchVector().normalized;

        var rotationDir = CalculateRotationDirection(playerVelocity, fromNodeToPlayer);

        RotationSpeed = (VelocityBeforeCollision.magnitude * RotationSpeedMultiplier) * rotationDir;

        var playerVector = NodeTransform.InverseTransformPoint(PlayerRigidBody.transform.position);
        Angle = Mathf.Deg2Rad * Angle360(playerVector, Vector2.up);

        var cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            var shakeMagnitude = NodeCollisionCameraShakeMagnitude * VelocityBeforeCollision.magnitude;
            StartCoroutine(cameraShake.Shake(shakeMagnitude , NodeCollisionCameraShakeDuration));
        }
    }

    private void OnCollisionWithObjective(Collider2D collider)
    {
        var objective = collider.gameObject.GetComponent<Objective>();

        if (objective != null)
            objective.Collect();
    }

    private void OnClicked(Vector3 clickLocation)
    {
        if (NodeTransform != null)
            Launch();
        else
        {
            if (PlayerGrapple.IsEnabled)
                PlayerGrapple.RemoveHinge();
            else
                PlayerGrapple.AttemptGrapple(clickLocation);
        }
    }

    private static int CalculateRotationDirection(Vector2 vec1, Vector2 vec2)
    {
        vec1.Normalize();        
        vec2.Normalize();        

        float dotProduct = Vector2.Dot(vec1, vec2);
        float determinant = vec1.x * vec2.y + vec1.y * vec2.x;

        return (int)Mathf.Sign(Mathf.Atan2(determinant, dotProduct)) * -1;
    }

    // https://answers.unity.com/questions/1164731/need-help-getting-angles-to-work-in-360-degrees.html
    public static float Angle360(Vector2 from, Vector2 to)
    {
        float angle = Vector2.SignedAngle(from, to);
        return angle < 0 ? 360 - angle * -1 : angle;
    }
}
