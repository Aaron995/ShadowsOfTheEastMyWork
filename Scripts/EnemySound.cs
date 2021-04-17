using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for enemy sounds.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class EnemySound : MonoBehaviour
{
    [Header("Moan settings")]
    [SerializeField] private float minimumTimeBeforeMoan = 3f;
    [SerializeField] private float maximumTimeBeforeMoan = 10f;

    [Header("Sound clips")]
    [SerializeField] private AudioClip moanClip;
    [SerializeField] private AudioClip gettingHitSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip dyingSound;

    private AudioSource AudioSource; // The audio source used for playing the sound
    private float timerUntilNextMoan; // Timer used for random moans

    void Start()
    {
        // Get the audio source and set the first moan timer
        AudioSource = GetComponent<AudioSource>();
        timerUntilNextMoan = Random.Range(minimumTimeBeforeMoan, maximumTimeBeforeMoan);
    }

    /// <summary>
    /// Plays the sound when the enemy gets hit.
    /// </summary>
    public void GotHit()
    {
        if (!AudioSource.isPlaying)
        {
            if (SoundManager.Instance.AllowedToPlayGettingHitSound(AudioSource))
            {
                AudioSource.clip = gettingHitSound;
                AudioSource.Play();
            }
        }
    }

    /// <summary>
    /// Plays the death sound of the enemy.
    /// </summary>
    public void Died()
    {
        if (!AudioSource.isPlaying)
        {
            if (SoundManager.Instance.AllowedToPlayDyingSound(AudioSource))
            {
                AudioSource.clip = dyingSound;
                AudioSource.Play();
            }
        }
    }

    /// <summary>
    /// Plays the attack sound of the enemy.
    /// </summary>
    public void Attacked()
    {
        if (!AudioSource.isPlaying)
        {
            if (SoundManager.Instance.AllowedToPlayAttackSound(AudioSource))
            {
                AudioSource.clip = attackSound;
                AudioSource.Play();
            }
        }
    }

    void Update()
    {
        // Count down the timer and when it hits 0 or lower play a moan sound.
        timerUntilNextMoan -= Time.deltaTime;
        if (timerUntilNextMoan <= 0 && !AudioSource.isPlaying)
        {
            if (SoundManager.Instance.AllowedToPlayMoan(AudioSource))
            {
                AudioSource.clip = moanClip;
                AudioSource.Play();
            }            
            timerUntilNextMoan = Random.Range(minimumTimeBeforeMoan, maximumTimeBeforeMoan);
        }
    }
}
