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
    public Camera Camera;
    private float angle = 0;

    public HingeJoint2D hinge;

    private void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        hinge.enabled = false;
    }

    // TODO used fixed update instead
    private void Update()
    {
        bool clicked = Input.GetButtonDown("Fire1");

        if (nodeTransform != null)
        {
            RotateAroundNode();
            if (clicked)
                Launch();
        }
        else
        {
            if (clicked)
                HingeToNode();
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

    private void HingeToNode()
    {
        if (hinge.enabled == true)
        {
            RemoveHinge();
            return;
        }

        var clickLocation = Camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickLocation, Vector2.zero);

        if (hit == false)
            return;

        SetHinge(hit);
    }

    private void RemoveHinge()
    {
        hinge.anchor = Vector2.zero;
        hinge.enabled = false;
    }

    private void SetHinge(RaycastHit2D hit)
    {
        hinge.enabled = true;
        var node = hit.collider.gameObject;
        hinge.anchor = transform.InverseTransformPoint(node.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (nodeTransform != null)
            return;

        if (hinge.enabled)
            RemoveHinge();

        nodeTransform = collision.collider.transform;
    }
}
