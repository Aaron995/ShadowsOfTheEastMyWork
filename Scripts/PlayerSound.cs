using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Componented used for Player Sounds.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    [Header("Audio clips")]
    [SerializeField] private AudioClip gettingHitSound;
    [SerializeField] private AudioClip lightAttackSound;
    [SerializeField] private AudioClip heavyAttackSound;
    [SerializeField] private AudioClip dyingSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays the player getting hit sound.
    /// </summary>
    public void OnGettingHit()
    {
        if (audioSource.isPlaying && audioSource.clip != dyingSound)
        {
            audioSource.clip = gettingHitSound;
            audioSource.Play();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.clip = gettingHitSound;
            audioSource.Play();
        }
    }


    /// <summary>
    /// PLays the player death sound.
    /// </summary>
    public void OnDeath()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();            
        }
        audioSource.clip = dyingSound;
        audioSource.Play();
    }

    /// <summary>
    /// Plays the light attack sounds.
    /// </summary>
    public void OnLightAttack()
    {
        audioSource.clip = lightAttackSound;
        audioSource.PlayOneShot(lightAttackSound);
    }

    /// <summary>
    /// Plays the heavy attack sounds.
    /// </summary>
    public void OnHeavyAttack()
    {
        audioSource.clip = heavyAttackSound;
        audioSource.PlayOneShot(heavyAttackSound);
    }
}
