using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the damage powerup.
/// </summary>
public class RedLotus : MonoBehaviour
{
    [SerializeField] private float lifeTime = 15f;
    [SerializeField] private int bonusDamage = 5;
    [SerializeField] private float duration = 7.5f;
    private void Start()
    {
        // Start coroutine to start the life time.
        StartCoroutine(DespawnOverLifeTime());
    }

    IEnumerator DespawnOverLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        // Destroy the object once the lifetime is over
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player hits the trigger collider on the power up 
        // if it is the player apply the upgrade and destroy the powerup.
        PlayerCombat combat = other.GetComponent<PlayerCombat>();
        if (combat != null)
        {
            combat.AddBonusDamage(duration, bonusDamage);
            Destroy(gameObject);
        }
    }
}
