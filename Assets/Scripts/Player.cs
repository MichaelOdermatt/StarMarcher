using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(HingeJoint2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float MinGrappleDistance = 1.1f;
    public float RotationSpeed = 4f;
    // Radius must be a little bigger than the radius of the node
    // because a collision will occur with the node you are launching off
    // if they are the same size.
    private float Radius = 1.1f;

    private float LaunchForce = 250f;
    private Rigidbody2D PlayerRigidBody;
    public Transform NodeTransform;
    public Camera Camera;
    private float Angle = 0;

    private LineRenderer LineRenderer;
    private HingeJoint2D Hinge;

    private void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.positionCount = 2;
        Hinge = GetComponent<HingeJoint2D>();
        Hinge.enabled = false;
    }

    private void Update()
    {
        bool clicked = Input.GetButtonDown("Fire1");

        if (clicked)
        {
            if (NodeTransform != null)
                Launch();
            else
                UpdateHinge();
        }

        UpdateLine();
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

        var offset = new Vector3(Mathf.Sin(Angle), Mathf.Cos(Angle)) * Radius;
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

    private void UpdateHinge()
    {
        if (Hinge.enabled == true)
        {
            RemoveHinge();
            return;
        }

        var clickLocation = Camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickLocation, Vector2.zero);

        if (hit == false 
            || hit.collider.gameObject == gameObject
            || MinGrappleDistance >= Vector2.Distance(
                hit.collider.transform.position, 
                PlayerRigidBody.transform.position))
            return;

        SetHinge(hit);
    }

    private void RemoveHinge()
    {
        Hinge.anchor = Vector2.zero;
        Hinge.enabled = false;
    }

    private void SetHinge(RaycastHit2D hit)
    {
        Hinge.enabled = true;
        var node = hit.collider.gameObject;
        Hinge.anchor = PlayerRigidBody.transform.InverseTransformPoint(node.transform.position);
    }

    private void UpdateLine()
    {
        LineRenderer.SetPosition(0, PlayerRigidBody.transform.position);
        LineRenderer.SetPosition(1, PlayerRigidBody.transform.TransformPoint(Hinge.anchor));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Node"))
            OnCollisionWithNode(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Component component;

        if (collider.TryGetComponent(typeof(Objective), out component))
            OnCollisionWithObjective((Objective)component);
    }

    private void OnCollisionWithNode(Component collider)
    {
        if (NodeTransform != null)
            return;

        if (Hinge.enabled)
            RemoveHinge();

        NodeTransform = collider.gameObject.transform;

        var playerVector = NodeTransform.InverseTransformPoint(PlayerRigidBody.transform.position);
        Angle = Mathf.Deg2Rad * Angle360(playerVector, Vector2.up);
    }

    private void OnCollisionWithObjective(Objective objective)
    {
        objective.Collect();
    }

    // https://answers.unity.com/questions/1164731/need-help-getting-angles-to-work-in-360-degrees.html
    public static float Angle360(Vector2 from, Vector2 to)
    {
        float angle = Vector2.SignedAngle(from, to);
        return angle < 0 ? 360 - angle * -1 : angle;
    }
}
