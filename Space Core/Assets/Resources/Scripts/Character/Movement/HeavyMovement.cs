using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeavyMovement : Movement
{

    //instant runspeed to prevent TOO sluggish of a start up
    [SerializeField] private float initRunSpeed;

    // Max speed dash into character values

    [SerializeField] private float collideVelocity;
    [SerializeField] private int damage;
    [SerializeField] private float knockBackX, knockBackY;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float stunTime;

    // Unique jump property values

    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpAcceleration;
    [SerializeField] private float hoverTime;
    [SerializeField] private float descendMaxSpeed;
    public bool isAscending { get; private set; }
    public bool isHovering { get; private set; }
    public bool isDescending { get; private set; }
    

    private bool waitingToHover;
    private bool waitingToLand;

    private float defaultGravityScale;

    /// <summary>
    /// Sets the basic default values
    /// </summary>
    protected override void SetDefaultValues()
    {
        runMax = 10;
        runAccel = 5;
        runDecel = 15;
        runBreak = 50;
        jumpVelocity = 0;
        gravityScale = 1;
        jumpCancelMinVel = 0.1f;
        jumpCancelVel = 0;
        airAccel = 10;
        airDecel = 10;
        airMax = 10;
        pushForce = 14;
        mass = 1;
        initRunSpeed = 4;
        collideVelocity = 9;
        damage = 1;
        knockBackX = 10;
        knockBackY = 10;
        knockbackTime = 0.2f;
        stunTime = 0.5f;
        jumpHeight = 8;
        jumpAcceleration = 15;
        hoverTime = 3;
        descendMaxSpeed = 7;
    }

    /// <summary>
    /// saves original gravity scale, for safe changing with fast fall 
    /// </summary>
    protected virtual void Start()
    {
        defaultGravityScale = gravityScale;
    }

    /// <summary>
    /// Overrides to initiate unique Hover property
    /// </summary>
    public override void Jump()
    {
        if (isJumping || charCont.isTouchingCeiling)
            return;

        forceUnground = true;
        ObjectPooler.instance.SpawnFromPool("JumpParticle", transform.position, Quaternion.identity);
        isJumping = true;

        if (waitingToHover)
            StartCoroutine(Hover());
        else if (waitingToLand)
            StartCoroutine(Descend());
        else
            StartCoroutine(Ascend());
    }

    /// <summary>
    /// handles unique ascend property, cancels immediately at any point when jump is released
    /// </summary>
    private IEnumerator Ascend()
    {

        isAscending = true;

        //gravityScale = 0;

        // Used to track height
        float initPosY = transform.position.y;

        // Before half height
        while (transform.position.y < initPosY + jumpHeight / 2 && !isJumpCanceling && (!charCont.isGrounded || forceUnground))
        {
            //cancel gravity
            velocity -= Physics2D.gravity * gravityScale * Time.fixedDeltaTime;

            // Accelerate Rise
            velocity += jumpAcceleration * Time.fixedDeltaTime * Vector2.up;

            yield return new WaitForFixedUpdate();
        }

        // After half height
        while (transform.position.y < initPosY + jumpHeight && velocity.y > 0 && !isJumpCanceling && !charCont.isGrounded)
        {
            //cancel gravity
            velocity -= Physics2D.gravity * gravityScale * Time.fixedDeltaTime;

            //Decelerate Rise
            velocity -= jumpAcceleration * Time.fixedDeltaTime * Vector2.up;

            yield return new WaitForFixedUpdate();
        }

        velocity = new Vector2(velocity.x, jumpCancelVel);

        waitingToHover = true;
        //gravityScale = defaultGravityScale;
        if (!isJumpCanceling)
            StartCoroutine(Hover());

        isAscending = false;

    }


    /// <summary>
    /// handles unique hover property, cancels immediately at any point when jump is released
    /// </summary>
    private IEnumerator Hover()
    {
        //gravityScale = 0;

        velocity = new Vector2(velocity.x, 0);

        // At max height, Hover
        isHovering = true;
        for (float timer = 0; timer < hoverTime && !isJumpCanceling && !charCont.isGrounded; timer += Time.fixedDeltaTime)
        {
            //cancel gravity
            velocity -= Physics2D.gravity * gravityScale * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();

        }
        isHovering = false;
        waitingToHover = false;
        //gravityScale = defaultGravityScale;
        waitingToLand = true;
        if (!isJumpCanceling && !charCont.isGrounded)
            StartCoroutine(Descend());

    }

    /// <summary>
    /// handles unique descend property, cancels immediately at any point when jump is released
    /// </summary>
    private IEnumerator Descend()
    {
        isDescending = true;

        gravityScale = 0;
        // After hover, decend slowly
        while (!charCont.isGrounded && !isJumpCanceling)
        {
            // Cancel gravity
            //velocity -= Physics2D.gravity * gravityScale * Time.fixedDeltaTime;

            if (velocity.y < -descendMaxSpeed)
                velocity = new Vector2(velocity.x, -descendMaxSpeed);
            else
            {
                // Accelerate decension
                velocity -= jumpAcceleration * Time.fixedDeltaTime * Vector2.up;

                if (velocity.y < -descendMaxSpeed)
                    velocity = new Vector2(velocity.x, -descendMaxSpeed);
            }
            yield return new WaitForFixedUpdate();
        }
        gravityScale = defaultGravityScale;

        isDescending = false;

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
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
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

        if (isHovering || isAscending || isDescending)
        {
            isJumping = true;
        }

        if (waitingToHover)
        {
            if (charCont.isGrounded)
                waitingToHover = false;
        }
        else if (waitingToLand)
        {
            if (charCont.isGrounded)
                waitingToLand = false;
        }

    }
}
