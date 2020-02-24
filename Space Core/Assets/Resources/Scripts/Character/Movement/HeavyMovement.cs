using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeavyMovement : Movement
{

    //instant runspeed to prevent TOO sluggish of a start up
    private float initRunSpeed = 2;

    // Max speed dash into character values

    private float collideVelocity = 9;
    private int damage = 1;
    private float knockBackX = 10, knockBackY = 10;
    private float knockbackTime = 0.2f;
    private float stunTime = 0.5f;

    // Unique jump property values

    private float jumpHeight = 8;
    private float jumpAcceleration = 8;
    private float hoverTime = 2;
    public bool isHovering { get; private set; }


    /// <summary>
    /// Sets the basic default values
    /// </summary>
    protected override void SetDefaultValues()
    {
        runMax = 10;
        runAccel = 5;
        runDecel = 20;
        runBreak = 30;
        jumpVelocity = 0;
        gravityScale = 1;
        jumpCancelMinVel = 0.1f;
        jumpCancelVel = 0;
        airAccel = 20;
        airDecel = 20;
        airMax = 10;
        pushForce = 14;
        mass = 1;
    }

    /// <summary>
    /// Overrides to initiate unique Hover property
    /// </summary>
    public override void Jump()
    {
        if(isJumping)
            return;
        base.Jump();

        StartCoroutine(Hover());

    }

    /// <summary>
    /// handles unique hover property, cancels immediately at any point when jump is released
    /// </summary>
    private IEnumerator Hover()
    {
        // Used to track height
        float initPosY = transform.position.y;

        // Before half height
        while (transform.position.y < initPosY + jumpHeight/2 && !isJumpCanceling)
        {
            //cancel gravity
            velocity -= Physics2D.gravity * gravityScale * Time.fixedDeltaTime;

            // Accelerate Rise
            velocity += jumpAcceleration * Time.fixedDeltaTime * Vector2.up;

            yield return new WaitForFixedUpdate();
        }
        
        // After half height
        while (transform.position.y < initPosY + jumpHeight && velocity.y > 0 && !isJumpCanceling)
        {
            //cancel gravity
            velocity -= Physics2D.gravity * gravityScale * Time.fixedDeltaTime;

            //Decelerate Rise
            velocity -= jumpAcceleration * Time.fixedDeltaTime * Vector2.up;

            yield return new WaitForFixedUpdate();
        }

        // At max height, Hover
        velocity = new Vector2(velocity.x, 0);
        isHovering = true;
        for (float timer = 0; timer < hoverTime && !isJumpCanceling; timer += Time.fixedDeltaTime)
        {
            //cancel gravity
            velocity -= Physics2D.gravity * gravityScale * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();

        }
        isHovering = false;

        // After hover, decend slowly
        while (!charCont.isGrounded && !isJumpCanceling)
        {
            // Cancel gravity
            velocity -= Physics2D.gravity * gravityScale * Time.fixedDeltaTime;
            
            // Accelerate decension
            velocity -= jumpAcceleration * Time.fixedDeltaTime * Vector2.up;

            yield return new WaitForFixedUpdate();
        }

    }

    /// <summary>
    /// Overrides to allow jump canceling to work with hover
    /// </summary>
    public override void JumpCancel()
    {
        if (isJumping)
        {
            if (velocity.y <= jumpCancelMinVel)
                isJumping = false;
            else if (velocity.y >= 0 || !charCont.isGrounded)
                isJumpCanceling = true;
        }
    }

    /// <summary>
    /// Overrides to allow for instant low starting speed, rather than accelerating from 0 speed
    /// </summary>
    /// <param name="direction"> Direction of intended running</param>
    protected override void Run(float direction)
    {
        if(direction != 0 && charCont.isGrounded 
            && (Mathf.Sign(velocity.x) == 0 || Mathf.Sign(velocity.x) == Mathf.Sign(direction)) 
            && Mathf.Abs(velocity.x) < initRunSpeed )
        {
            velocity = direction * charCont.currentSlope * initRunSpeed; 
        }
        base.Run(direction);
    }

    /// <summary>
    /// Overrides to handle collision with enemy at max speed
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // While dashing, check collision with character
        if (Mathf.Abs(velocity.x) >= collideVelocity && (other.tag == "Player" || other.tag == "Enemy")) 
        {
            other.GetComponent<Character>().ReceiveAttack(
                new AttackInfo(
                    damage,
                    new Vector2(knockBackX * owner.facingDirection, knockBackY),
                    knockbackTime,
                    stunTime,
                    DamageSource.Enemy));
            velocity = new Vector2(0, velocity.y);
        }
    }

    /// <summary>
    /// Overrides to cancel base jump properties to allow for unique Hover ability
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isJumping && isHovering && (velocity.y <= 0 || (charCont.isGrounded && !forceUnground)))
        {
            isJumping = true;
        }

    }
}
