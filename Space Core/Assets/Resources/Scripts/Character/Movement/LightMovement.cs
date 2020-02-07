using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LightMovement : Movement
{

    // Blink Dash attributes
    private bool isBlinkDashing = false;
    private bool landingWait = false;
    private float dashTime = 0.1f;
    private float dashTimer = 0f;
    private float dashDistance = 5;

    // Unique Jump values;
    float jumpHeight = 8;
    bool jumpStunned = false;

    // Unique gravity attributes
    float defaultGravityScale;
    float fastFallGravity = 5f;
    float fastFallCap = 20f;

    /// <summary>
    /// Sets the basic default values
    /// </summary>
    protected override void SetDefaultValues()
    {
        runMax = 11;
        runAccel = 40;
        runDecel = 20;
        runBreak = 40;
        jumpVelocity = 30;
        gravityScale = 0.5f;
        jumpCancelVel = 3;
        jumpCancelMinVel = Mathf.Sqrt((jumpVelocity*jumpVelocity) + 2*Physics2D.gravity.y*gravityScale*jumpHeight);
        airAccel = 80;
        airDecel = 20;
        airMax = 11;
        pushForce = 14;
        mass = 1;
    }

    /// <summary>
    /// saves original gravity scale, for safe changing with fast fall 
    /// </summary>
    protected virtual void Start()
    {
        defaultGravityScale = gravityScale;
    }

    /// <summary>
    /// Overrides jump for unique jump properties and to allow for a mid air jump in the form of the Blink Dash
    /// </summary>
    public override void Jump()
    {
        if(charCont.isGrounded){
            base.Jump();
            owner.isStunned++;
            jumpStunned = true;
        }
        else
        {
            SpecialAbility();
        }
        
    }

    /// <summary>
    /// Used to activate and deactivate fast fall when down is pushed
    /// </summary>
    /// <param name="direction">Direction of intended movement</param>
    public override void Move(Vector2 direction)
    {
        base.Move(direction);

         if (isBlinkDashing)
            return;

        if (gravityScale != fastFallGravity && !isJumping && velocity.y > -fastFallCap && direction != Vector2.zero && Vector2.Angle(direction, Vector2.down) <= 46)
            gravityScale = fastFallGravity;
        else if (gravityScale != defaultGravityScale)
            gravityScale = defaultGravityScale;
    }

    /// <summary>
    /// LightMovement blink dash ability
    /// </summary>
    public override void SpecialAbility()
    {
        if (!charCont.isGrounded && !landingWait && !isBlinkDashing)
            StartCoroutine(BlinkDash((movingDirection != Vector2.zero) ? movingDirection : Vector2.right * owner.facingDirection));
    }

    /// <summary>
    /// Instantaneous dash
    /// </summary>
    /// <param name="direction">Direction of intended dashing</param>
    private IEnumerator BlinkDash(Vector2 direction)
    {
        // Set dash values
        isBlinkDashing = true;
        owner.isStunned++;
        freezeRun = true;
        gravityScale = 0;

        /// Set velocity for dash
        velocity = direction * (dashDistance / dashTime);

        // Check conditional during dash
        for (dashTimer = 0; dashTimer < dashTime; dashTimer += Time.fixedDeltaTime)
        {
            // Is dash condition no longer valid?
            if (charCont.isGrounded
                || charCont.isTouchingRightWall
                || charCont.isTouchingLeftWall)
            {

                break;
            }

            yield return new WaitForFixedUpdate();
        }
        dashTimer = 0;

        // Reset dash values
        isBlinkDashing = false;
        owner.isStunned--;
        freezeRun = false;
        gravityScale = defaultGravityScale;

        // Reset velocity to zero after dash
        velocity = Vector2.zero;

        // Cannot dash again until land
        landingWait = true;

    }

    /// <summary>
    /// Overrides to end Blink dash if dashing into character
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        // While dashing, check collision with character
        if (isBlinkDashing && (other.tag == "Player" || other.tag == "Enemy"))
            dashTimer = dashTime;

    }

    /// <summary>
    /// Overrides to handle unique jumping properties
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (isJumping)
        {   
            if (velocity.y <= jumpCancelMinVel)
                JumpCancel();

        }
        if(jumpStunned && !isJumping)
        {
            owner.isStunned--;
            jumpStunned = false;
        }

        if (landingWait && charCont.isGrounded)
            landingWait = false;

    }

}
