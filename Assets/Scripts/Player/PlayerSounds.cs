using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource AudioSource;
    public float volume = 0.5f;
    public AudioClip[] ImpactSounds;

    public void PlayImpact() 
    {
        if (AudioSource == null || ImpactSounds.Length == 0)
            return;

        AudioClip impactSound = ImpactSounds[Random.Range(0, ImpactSounds.Length)]; ;
        AudioSource.clip = impactSound;
        AudioSource.Play();
    }
}
