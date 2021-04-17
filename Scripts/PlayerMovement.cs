using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to make the player move.
/// </summary>
[RequireComponent(typeof(PlayerAnimations))]
public class PlayerMovement : MonoBehaviour
{
    private float PlayerPositionX;
    private Rigidbody2D rigidBody;
    private Animator animator;
    public HUD hud;

    [Header("Stamina Settings")]
    [SerializeField] public int maxStamina = 100;
    [SerializeField] private int staminaPerSecond = 5;
    [Header("Movement settings")]
    [SerializeField] private float speed = 20f;
    [Header("Dash settings")]
    [SerializeField] private float dashDistance = 20f;
    [SerializeField] private float dashCooldown = 4f;
    [SerializeField] private int dashStaminaCost = 25;
    [SerializeField] private GameObject dashTrailObject;   
    [Header("Jump settings")]
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform jumpCheckPoint;
    
    [Header("Runtime variables")]
    public int stamina;
    public int staminaReduction { private set; get; } = 0;

    public float lastDirection { private set; get; } = 1;

    private float dashCooldownTimer = 0;
    private float jumpBuffer = 0;
    private float extraSpeed = 0;
    private float staminaRechargeTimer;
    
    private PlayerAnimations animScript;

    private void Awake()
    {
        animScript = GetComponent<PlayerAnimations>();
        rigidBody = GetComponent<Rigidbody2D>();
        stamina = maxStamina;
    }

    /// <summary>
    /// A check to see if the player is grounded.
    /// </summary>
    /// <returns>Returns a boolean if the player is grounded or not.</returns>
    public bool IsGrounded()
    {
        return Physics2D.Raycast(jumpCheckPoint.transform.position, Vector3.down, 0.2f, groundLayers);
    }

    /// <summary>
    /// Increase the player movement speed and give stamina reduction. Fades off over time.
    /// </summary>
    /// <param name="duration">How long the buff stays on the player.</param>
    /// <param name="speed">How much faster the player goes.</param>
    /// <param name="_staminaReduction">How much stamina is being reduced.</param>
    public void AddMovementSpeed(float duration, float speed, int _staminaReduction)
    {
        extraSpeed += speed;
        staminaReduction += _staminaReduction;
        StartCoroutine(RemoveBonusSpeed(duration, speed, staminaReduction));
    }  
    IEnumerator RemoveBonusSpeed(float duration, float speed, float _staminaReduction)
    {
        yield return new WaitForSeconds(duration);

        extraSpeed -= speed;
        staminaReduction -= _staminaReduction;
    }

    private void Update()
    {
        // Takes the current position on the X axis.
        PlayerPositionX = transform.position.x;
        
        // Run all the update functions for the player.
        UpdatePlayerMovement();
        UpdateDash();        
        UpdateJump();
        UpdateTimers();

        // Clamp the player on the X axis and update the transform.
        transform.position = new Vector2(Mathf.Clamp(PlayerPositionX, (float)-8.5, (float)8.5), transform.position.y);
    }

    private void UpdatePlayerMovement()
    {
        // Takes the horizontal input as we only move along the horizontal axis.
        float horizontal = Input.GetAxis("Horizontal");

        // If there was any horizontal input chanse the rotation and update the last direction.
        if (horizontal != 0)
        {
            if (horizontal > 0)
            {
                lastDirection = 1;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                lastDirection = -1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        // Add the horizontal movement to the X variable.
        PlayerPositionX += horizontal * Time.deltaTime * (speed + extraSpeed);
    }

    private void UpdateDash()
    {
        // If we have enough stamina and dash it off cooldown we do our dash logic.
        if (dashCooldownTimer <= 0 && Input.GetAxis("Dash") > 0 && EnoughStaminaToDash())
        {
            // Our dash is very lazy due save some time.       
            // We start this Coroutine to make it look like we have a proper dash trail.
            StartCoroutine(DashTrail());
           
            // We teleport the player the set distance in the direction we face.
            PlayerPositionX += lastDirection * dashDistance;
            

            // Then we set everything we need, do the animation and update the hud.
            dashCooldownTimer = dashCooldown;
            stamina -= Mathf.Clamp(dashStaminaCost - staminaReduction, 0, maxStamina);
            animScript.OnDash();
            hud.UpdateMana();
        }
    }

    IEnumerator DashTrail()
    {
        dashTrailObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        dashTrailObject.SetActive(false);
    }

    private bool EnoughStaminaToDash()
    {
        // Checks to see if there is enough Stamina to use dash.
        int dashCost = dashStaminaCost - staminaReduction;
        if (stamina >= dashCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateJump()
    {
        // When we press the jump button we set our jump buffer to a small number.
        if (Input.GetAxis("Jump") > 0)
        {
            jumpBuffer = 0.2f;
        }

        // When we are grounded and the jump buffer is still active we jump.
        if (jumpBuffer > 0 && IsGrounded())
        {
            jumpBuffer = 0;
            rigidBody.velocity = Vector2.up * jumpForce;
            animScript.OnJump();
        }
    }

    private void UpdateTimers()
    {
        // Updating all our timers.

        dashCooldownTimer -= Time.deltaTime;
        jumpBuffer -= Time.deltaTime;

        if (stamina < maxStamina)
        {
            staminaRechargeTimer += Time.deltaTime;
            if (staminaRechargeTimer >= 1f)
            {
                stamina += staminaPerSecond;
                Mathf.Clamp(stamina, 0, maxStamina);
                staminaRechargeTimer = 0;
                hud.UpdateMana();
            }
        }
        else
        {
            staminaRechargeTimer = 0;
        }
    } 
}
