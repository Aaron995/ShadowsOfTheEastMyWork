using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the speed powerup.
/// </summary>
public class BlueLotus : MonoBehaviour
{
    [Header("Power up stats")]
    [SerializeField] private float lifeTime = 15f;
    [SerializeField] private float bonusSpeed = 3f;    
    [SerializeField] private float duration = 7.5f;
    [SerializeField] private int staminaReduction = 10;
    [SerializeField] private int shurikens = 10;
    [Header("Script references")]
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private HUD hud;

    private void Start()
    {
        // Start coroutine to start the life time.
        StartCoroutine(DespawnOverLifeTime());
        // Find the player and the hud in the scene.
        playerCombat = GameObject.Find("Player").GetComponent<PlayerCombat>();
        hud = GameObject.Find("HUD").GetComponent<HUD>();
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
        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.AddMovementSpeed(duration, bonusSpeed, staminaReduction);
            playerCombat.shurikenAmount += shurikens;
            hud.UpdateShuriken();
            Destroy(gameObject);
        }
    }
}
