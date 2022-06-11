using UnityEngine;

[RequireComponent(typeof(PlayerGrapple))]
[RequireComponent(typeof(PlayerCollisions))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float RotationSpeed = 4f;
    // Radius must be a little bigger than the radius of the node
    // because a collision will occur with the node you are launching off
    // if they are the same size.
    private float RotationRadius = 1.1f;
    private float LaunchForce = 250f;
    private float Angle = 0;

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

        var offset = new Vector3(Mathf.Sin(Angle), Mathf.Cos(Angle)) * RotationRadius;
        PlayerRigidBody.transform.position = NodeTransform.position + offset;
    }

    private void Launch()
    {
        Vector2 launchVector = CalculateLaunchVector();
        NodeTransform = null;
        PlayerRigidBody.isKinematic = false;
        PlayerRigidBody.AddForce(launchVector * LaunchForce);
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

        NodeTransform = collider.gameObject.transform;

        var playerVector = NodeTransform.InverseTransformPoint(PlayerRigidBody.transform.position);
        Angle = Mathf.Deg2Rad * Angle360(playerVector, Vector2.up);
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
            PlayerGrapple.UpdateGrapple(clickLocation);
    }

    // https://answers.unity.com/questions/1164731/need-help-getting-angles-to-work-in-360-degrees.html
    public static float Angle360(Vector2 from, Vector2 to)
    {
        float angle = Vector2.SignedAngle(from, to);
        return angle < 0 ? 360 - angle * -1 : angle;
    }
}
