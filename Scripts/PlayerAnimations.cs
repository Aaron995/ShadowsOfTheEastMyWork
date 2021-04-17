using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to control Player Animations.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimations : MonoBehaviour
{
    // References to the scripts 
    private PlayerMovement movementScript;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); ;   
        
        if (horizontal != 0)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        animator.SetBool("Grounded", movementScript.IsGrounded());
    }
 
    public void OnJump()
    {
        animator.SetTrigger("Jump");
    }
    
    public void OnLightAttack()
    {
        animator.SetTrigger("LightAttack");
    }

    public void OnHeavyAttack()
    {
        animator.SetTrigger("HeavyAttack");
    }

    public void OnDash()
    {
        animator.SetTrigger("Dash");
    }

}
