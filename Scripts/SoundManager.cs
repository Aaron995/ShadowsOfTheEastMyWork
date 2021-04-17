using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Background settings")]
    [SerializeField] private AudioSource backgroundMusic;

    [Header("Moan settings")]
    [SerializeField] private int maxAmountOfMoans = 5;
    private List<AudioSource> moanSources = new List<AudioSource>();

    [Header("Zombie getting hit setting")]
    [SerializeField] private int maxAmountOfHitSettings = 5;
    private List<AudioSource> gettingHitSources = new List<AudioSource>();

    [Header("Zombie dying sound settings")]
    [SerializeField] private int maxAmountOfDyingSounds = 5;
    private List<AudioSource> dyingSources = new List<AudioSource>();

    [Header("Zombie attack sound settings")]
    [SerializeField] private int maxAmountOfAttackSounds = 5;
    private List<AudioSource> attackSources = new List<AudioSource>();

    private void Awake()
    {
        // Make this class a singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    /// <summary>
    /// Starts playing the background music.
    /// </summary>
    public void StartBackgroundMusic()
    {
        backgroundMusic.Play();
    }

    /// <summary>
    /// Stops playing the background music.
    /// </summary>
    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    /// <summary>
    /// Checks if the Zombie is allowed to play it's getting hit sound.
    /// </summary>
    /// <param name="source">The audio source used to play the sound effect.</param>
    /// <returns>Returns a boolean depending if the sound is allowed to be played.</returns>
    public bool AllowedToPlayGettingHitSound(AudioSource source)
    {
        gettingHitSources = UpdateAudioSourceList(gettingHitSources);
        if (gettingHitSources.Count >= maxAmountOfHitSettings)
        {
            return false;
        }
        else
        {
            gettingHitSources.Add(source);
            return true;
        }
    }

    /// <summary>
    /// Checks if the Zombie is allowed to play a moan sound.
    /// </summary>
    /// <param name="source">The audio source used to play the sound effect.</param>
    /// <returns>Returns a boolean depending if the sound is allowed to be played.</returns>
    public bool AllowedToPlayMoan(AudioSource source)
    {
        moanSources = UpdateAudioSourceList(moanSources);
        if (moanSources.Count >= maxAmountOfMoans)
        {
            return false;
        }
        else
        {
            moanSources.Add(source);
            return true;
        }
    }

    /// <summary>
    /// Checks if the Zombie is allowed to play the dying sound.
    /// </summary>
    /// <param name="source">The audio source used to play the sound effect.</param>
    /// <returns>Returns a boolean depending if the sound is allowed to be played.</returns>
    public bool AllowedToPlayDyingSound(AudioSource source)
    {
        dyingSources = UpdateAudioSourceList(dyingSources);
        if (dyingSources.Count >= maxAmountOfDyingSounds)
        {
            return false;
        }
        else
        {
            dyingSources.Add(source);
            return true;
        }
    }

    /// <summary>
    /// Checks if the Zombie is allowed to play the attack sound.
    /// </summary>
    /// <param name="source">The audio source used to play the sound effect.</param>
    /// <returns>Returns a boolean depending if the sound is allowed to be played.</returns>
    public bool AllowedToPlayAttackSound(AudioSource source)
    {
        attackSources = UpdateAudioSourceList(attackSources);
        if (attackSources.Count >= maxAmountOfAttackSounds)
        {
            return false;
        }
        else
        {
            attackSources.Add(source);
            return true;
        }
    }

    private List<AudioSource> UpdateAudioSourceList(List<AudioSource> audioSources)
    {
        // Initialize a list for sources to remove at the end
        List<AudioSource> toRemoveSources = new List<AudioSource>();
        
        foreach (AudioSource source in audioSources)
        {
            // If the audio source still exists and the audio source isn't playing anymore it can be removed from the list 
            if (source != null)
            {
                if (!source.isPlaying)
                {
                    toRemoveSources.Add(source);
                }
            }
            // If the audio source doesn't exists anymore it can be removed
            else
            {
                toRemoveSources.Add(source);
            }
        }

        // Remove all the sources collected from the list given
        foreach (AudioSource source in toRemoveSources)
        {
            audioSources.Remove(source);
        }

        return audioSources;
    }
}
