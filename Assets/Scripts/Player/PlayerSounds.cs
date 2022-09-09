using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource AudioSource;
    public float volume = 0.5f;
    [SerializeField]
    private AudioClip[] ImpactSounds;
    [SerializeField]
    private AudioClip[] LaunchGrappleSounds;
    [SerializeField]
    private AudioClip[] ReleaseGrappleSounds;
    [SerializeField]
    private AudioClip[] GrappleConnectSounds;
    [SerializeField]
    private AudioClip[] DeathSounds;

    public void PlayLaunchGrapple() 
    {
        if (AudioSource == null || LaunchGrappleSounds.Length == 0)
            return;

        AudioClip launchGrappleSound = LaunchGrappleSounds[Random.Range(0, LaunchGrappleSounds.Length)]; ;
        AudioSource.clip = launchGrappleSound;
        AudioSource.Play();
    }

    public void PlayReleaseGrapple() 
    {
        if (AudioSource == null || ReleaseGrappleSounds.Length == 0)
            return;

        AudioClip releaseGrappleSound = ReleaseGrappleSounds[Random.Range(0, ReleaseGrappleSounds.Length)]; ;
        AudioSource.clip = releaseGrappleSound;
        AudioSource.Play();
    }

    public void PlayGrappleConnect() 
    {
        if (AudioSource == null || GrappleConnectSounds.Length == 0)
            return;

        AudioClip grappleConnectSound = GrappleConnectSounds[Random.Range(0, GrappleConnectSounds.Length)]; ;
        AudioSource.clip = grappleConnectSound;
        AudioSource.Play();
    }

    public void PlayImpact() 
    {
        if (AudioSource == null || ImpactSounds.Length == 0)
            return;

        AudioClip impactSound = ImpactSounds[Random.Range(0, ImpactSounds.Length)]; ;
        AudioSource.clip = impactSound;
        AudioSource.Play();
    }

    public void PlayDeath() 
    {
        if (AudioSource == null || DeathSounds.Length == 0)
            return;

        AudioClip deathSound = DeathSounds[Random.Range(0, DeathSounds.Length)]; ;
        AudioSource.clip = deathSound;
        AudioSource.Play();
    }
}
