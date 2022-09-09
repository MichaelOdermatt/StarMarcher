using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerGrapple))]
[RequireComponent(typeof(PlayerCollisions))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerSounds))]
public class Player : MonoBehaviour
{
    private float NodeCollisionCameraShakeMagnitude = 0.025f;
    private float NodeCollisionCameraShakeDuration = 0.2f;

    public ParticleSystem ExplosionParticleSystem;

    private PlayerInput PlayerInput;
    private PlayerMovement PlayerMovement;
    private PlayerGrapple PlayerGrapple;
    private PlayerCollisions PlayerCollisions;
    private PlayerSounds PlayerSounds;
    private PlayerKillFloor PlayerKillFloor;
    private SceneNav SceneNav;

    private void Awake()
    {
        SceneNav = new SceneNav();

        if (TryGetComponent(out PlayerKillFloor))
            PlayerKillFloor.KillPlayer += OnDeath;

        PlayerSounds = GetComponent<PlayerSounds>();

        PlayerMovement = GetComponent<PlayerMovement>();

        PlayerGrapple = GetComponent<PlayerGrapple>();
        PlayerGrapple.IsEnabled = false;
        PlayerGrapple.LaunchGrappleSound += PlayerSounds.PlayLaunchGrapple;
        PlayerGrapple.ConnectGrappleSound += PlayerSounds.PlayGrappleConnect;

        PlayerCollisions = GetComponent<PlayerCollisions>();
        PlayerCollisions.CollisionWithNodeRotatable += OnCollisionWithNodeRotatable;
        PlayerCollisions.CollisionWithNodeSwingOnly += OnCollisionWithNodeSwingOnly;
        PlayerCollisions.ExitFromNodeSwingOnly += OnExitFromNodeSwingOnly;
        PlayerCollisions.CollisionWithObjective += OnCollisionWithObjective;
        PlayerCollisions.KillPlayer += OnDeath;

        PlayerInput = GetComponent<PlayerInput>();
        PlayerInput.clicked += OnClicked;
        PlayerInput.ResetScenePressed += OnResetScenePressed;
    }

    private void Update()
    {
        PlayerGrapple.UpdateLine();
    }

    private void OnCollisionWithNodeRotatable(Component collider)
    {
        if (PlayerMovement.AttachedNodeTransform != null)
            return;

        if (PlayerGrapple.IsEnabled)
            PlayerGrapple.RemoveHinge();

        if (PlayerGrapple.IsDrawingGrapple)
        {
            PlayerGrapple.StopDrawingThenSetGrapple();
            PlayerGrapple.DisableLineRenderer();
        }

        PlayerMovement.StartRotation(collider.gameObject.transform);
        PlayerSounds.PlayImpact();

        var cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            var shakeMagnitude = 
                NodeCollisionCameraShakeMagnitude * PlayerMovement.VelocityBeforeCollision.magnitude;
            StartCoroutine(cameraShake.Shake(shakeMagnitude , NodeCollisionCameraShakeDuration));
        }
    }

    private void OnCollisionWithNodeSwingOnly(Component collider)
    {
        INodeInteractions node;
        if (collider.gameObject.TryGetComponent(out node))
            node.OnCollisionWithPlayer();
    }

    private void OnExitFromNodeSwingOnly(Component collider)
    {
        INodeInteractions node;
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
        if (PlayerMovement.AttachedNodeTransform != null)
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
        // doesn't work because player is imediatly destroyed
        PlayerSounds.PlayDeath();
        PlayExplosionParticleSystem();
        Destroy(gameObject);
    }

    private void PlayExplosionParticleSystem()
    {
        if (ExplosionParticleSystem != null)
        {
            ExplosionParticleSystem.transform.position = transform.position;
            ExplosionParticleSystem.Play();
        }
    }

    private void OnResetScenePressed()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Substring(0, 5) == "Level")
            SceneNav.ResetCurrentScene();
    }

}
