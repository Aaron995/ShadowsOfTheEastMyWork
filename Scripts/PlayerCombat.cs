using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for the player combat mechanics.
/// </summary>
public class PlayerCombat : MonoBehaviour, IDamagable
{
    [Header("General settings")]
    [SerializeField] private PlayerMovement movementScript;
    [SerializeField] private PlayerSound soundScript;
    [SerializeField] private PlayerAnimations animScript;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private HUD hud;
    [SerializeField] private GameOverScript gameOver;

    [Header("Light attack settings")]
    [SerializeField] public int lightAttackDamage = 12;
    [SerializeField] private float lightAttackCooldown = 0.5f;

    [Header("Heavy attack settings")]
    [SerializeField] public int heavyAttackDamage = 25;
    [SerializeField] private float heavyAttackCooldown = 0.8f;

    [Header("Health Settings")]
    [SerializeField] public int maxHealth = 20;
    [SerializeField] public int health = 20;

    [Header("Defence Settings")]
    [SerializeField] public int defence = 0;

    private bool isDead = false;

    private float attackCooldownTimer;

    private int bonusDamage = 0;

    
    private void Update()
    {
        UpdateMeleeCombat();
    }       

    private void UpdateMeleeCombat()
    {
        if (attackCooldownTimer <= 0)
        {
            // If we have no cooldown on our attacks and we perform one, do an attack
            if (Input.GetAxis("LightAttack") > 0)
            {
                LightAttack();
            }

            if (Input.GetAxis("HeavyAttack") > 0)
            {
                HeavyAttack();
            }           
        }
        else
        {
            // Count down our cooldown timer
            attackCooldownTimer -= Time.deltaTime;
        }
    }
    private void LightAttack()
    {
        // Overlap a circle on the enemy layer mask.
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Apply damage to all Damagable Interfaces hit.
        foreach (Collider2D collider in hitColliders)
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(lightAttackDamage + bonusDamage);
            }
        }

        soundScript.OnLightAttack();
        animScript.OnLightAttack();
        attackCooldownTimer = lightAttackCooldown;
    }

    private void HeavyAttack()
    {
        // Overlap a circle on the enemy layer mask.
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Apply damage to all Damagable Interfaces hit.
        foreach (Collider2D collider in hitColliders)
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(heavyAttackDamage + bonusDamage);
            }
        }

        soundScript.OnHeavyAttack();
        animScript.OnHeavyAttack();
        attackCooldownTimer = heavyAttackCooldown;
    }   

    public void TakeDamage(int damageAmount)
    {
        if (!isDead)
        {
            // Reduce our health taking our defence in mind
            // An attack will always do 1 damage
            health -= Mathf.Clamp(damageAmount - defence, 1, damageAmount);

            soundScript.OnGettingHit();
            hud.UpdateHealth();

            // If the health drops to 0 or lower the player dies
            if (health <= 0)
            {
                soundScript.OnDeath();
                GetComponent<PlayerMovement>().enabled = false;
                gameOver.GameOver();
                isDead = true;
                enabled = false;
            }
        }
    }

    public void AddBonusDamage(float duration, int damage)
    {
        // Adds the bonus damage
        bonusDamage += damage;
        // Start the coroutine that drops off the bonus damage
        StartCoroutine(RemoveBonusDamage(duration, damage));
    }

    IEnumerator RemoveBonusDamage(float duration, int damage)
    {
        yield return new WaitForSeconds(duration);

        // Remove the bonus damage if the duration is over
        bonusDamage -= damage;
    }
}
