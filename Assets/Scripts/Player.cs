using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(HingeJoint2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private float RotationSpeed = 4f;
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
        transform.position = NodeTransform.position + offset;
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

        if (hit == false || hit.collider.gameObject == gameObject)
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
        Hinge.anchor = transform.InverseTransformPoint(node.transform.position);
    }

    private void UpdateLine()
    {
        LineRenderer.SetPosition(0, transform.position);
        LineRenderer.SetPosition(1, transform.TransformPoint(Hinge.anchor));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (NodeTransform != null)
            return;

        if (Hinge.enabled)
            RemoveHinge();

        NodeTransform = collision.collider.transform;
    }
}
