using UnityEngine;

[RequireComponent(typeof(PlayerGrapple))]
[RequireComponent(typeof(PlayerCollisions))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    private float NodeCollisionCameraShakeMagnitude = 0.025f;
    private float NodeCollisionCameraShakeDuration = 0.2f;

    private PlayerInput PlayerInput;
    private PlayerMovement PlayerMovement;
    private PlayerGrapple PlayerGrapple;
    private PlayerCollisions PlayerCollisions;

    private void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();

        PlayerGrapple = GetComponent<PlayerGrapple>();
        PlayerGrapple.IsEnabled = false;

        PlayerCollisions = GetComponent<PlayerCollisions>();
        PlayerCollisions.CollisionWithNode += OnCollisionWithNode;
        PlayerCollisions.CollisionWithNodeNonSwingable += OnCollisionWithNodeNonSwingable;
        PlayerCollisions.ExitWithNodeNonSwingable += OnExitWithNodeNonSwingable;
        PlayerCollisions.CollisionWithObjective += OnCollisionWithObjective;
        PlayerCollisions.KillPlayer += OnDeath;

        PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.clicked += OnClicked;
    }

    private void Update()
    {
        PlayerGrapple.UpdateLine();
    }

    private void OnCollisionWithNode(Component collider)
    {
        if (PlayerMovement.NodeTransform != null)
            return;

        if (PlayerGrapple.IsEnabled)
            PlayerGrapple.RemoveHinge();

        if (PlayerGrapple.IsDrawingGrapple)
        {
            PlayerGrapple.StopDrawThenSetGrapple();
            PlayerGrapple.DisableLineRenderer();
        }

        PlayerMovement.StartRotation(collider.gameObject.transform);

        var cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            var shakeMagnitude = 
                NodeCollisionCameraShakeMagnitude * PlayerMovement.VelocityBeforeCollision.magnitude;
            StartCoroutine(cameraShake.Shake(shakeMagnitude , NodeCollisionCameraShakeDuration));
        }
    }

    private void OnCollisionWithNodeNonSwingable(Component collider)
    {
        NodeNonSwingable node;
        if (collider.gameObject.TryGetComponent(out node))
            node.OnCollisionWithPlayer();
    }

    private void OnExitWithNodeNonSwingable(Component collider)
    {
        NodeNonSwingable node;
        if (collider.gameObject.TryGetComponent(out node))
            node.OnExitWithPlayer();
    }

    private void OnCollisionWithObjective(Collider2D collider)
    {
        var objective = collider.gameObject.GetComponent<Objective>();

        if (objective != null)
            objective.Collect();
    }

    private void OnClicked(Vector3 clickLocation)
    {
        if (PlayerMovement.NodeTransform != null)
            PlayerMovement.Launch();
        else
        {
            if (PlayerGrapple.IsEnabled)
                PlayerGrapple.RemoveHinge();
            else
                PlayerGrapple.AttemptGrapple(clickLocation);
        }
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

}
