using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MediumMovement : Movement
{

    // Combat Roll Values

    public bool isCombatRolling { get; private set; }
    private float coolDown = 0.5f;
    private float coolDownTimer = 0f;
    private float rollTime = 0.3f;
    private float rollVelocityIncrease = 15;

    // Double Tap Detection system values

    private float lastRunDirection;
    private float doubleTapTime = 0.2f;
    private float doubleTapRightTimer;
    private float doubleTapLeftTimer;
    private bool doubleTapRightCheck = false;
    private bool doubleTapLeftCheck = false;

    /// <summary>
    /// Sets the basic default values
    /// </summary>
    protected override void SetDefaultValues()
    {
        runMax = 7;
        runAccel = 40;
        runDecel = 40;
        runBreak = 100;
        jumpVelocity = 18;
        gravityScale = 1;
        jumpCancelMinVel = 12;
        jumpCancelVel = 2;
        airAccel = 50;
        airDecel = 25;
        airMax = 9;
        pushForce = 14;
        mass = 1;
    }

    /// <summary>
    /// Overrides to detect for doubletapping so that Combat Roll may be activated
    /// </summary>
    /// <param name="direction">Direction of Intended movement</param>
    protected override void Run(float direction)
    {
        base.Run(direction);

        // Direction changed

        if (direction != lastRunDirection)
        {
            // Double tap detected

            if(doubleTapRightCheck && direction == +1)
            {
                SpecialAbility();
                doubleTapRightCheck = false;
                doubleTapRightTimer = 0;
                doubleTapLeftCheck = false;
                doubleTapLeftTimer = 0;
            }
            if(doubleTapLeftCheck && direction == -1)
            {
                SpecialAbility();
                doubleTapRightCheck = false;
                doubleTapRightTimer = 0;
                doubleTapLeftCheck = false;
                doubleTapLeftTimer = 0;
            }

            // Start double tap check
            if(direction == +1)
            {
                doubleTapRightCheck = true;
                doubleTapRightTimer = 0;
            }
            else if (direction == -1)
            {
                doubleTapLeftCheck = true;
                doubleTapLeftTimer = 0;
            }

        }

        // Store previous frame run direction
        lastRunDirection = direction;

    }

    /// <summary>
    /// MediumMovement Combat Roll ability
    /// </summary>
    public override void SpecialAbility()
    {
        if (coolDownTimer == 0 && !isCombatRolling)
            StartCoroutine(CombatRoll((movingDirection.x != 0) ? Mathf.Sign(movingDirection.x) : owner.facingDirection));
    }

    /// <summary>
    /// Combat roll that gives a short burst of speed and grants invulnerability
    /// </summary>
    /// <param name="direction">Direction of intended Rolling</param>
    /// <returns></returns>
    private  IEnumerator CombatRoll(float direction)
    {
        // Set roll values
        isCombatRolling = true;
        owner.isStunned++;
        owner.Invincible++;
        owner.IsBlinking = true;
        freezeRun = true;

        // Rolling via set velocity
        velocity = Vector2.right * direction * (rollVelocityIncrease + runMax) ;
        Vector2 deceleration = -(direction * Vector2.right) * (rollVelocityIncrease/rollTime);

        // Check conditional during roll
        for (float timer = 0; timer < rollTime; timer += Time.fixedDeltaTime)
        {
            if(Mathf.Abs(velocity.x) > runMax)
                velocity += deceleration * Time.fixedDeltaTime;

            // Is roll condition no longer valid?
            if (charCont.isTouchingRightWall
                || charCont.isTouchingLeftWall)
            {
                velocity = new Vector2(0, velocity.y);
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        // Reset velocity after roll
        velocity = new Vector2(runMax * direction, velocity.y);

        // Reset roll values
        isCombatRolling = false;
        owner.isStunned--;
        owner.Invincible--;
        owner.IsBlinking = false;
        freezeRun = false;

        // Roll Cool down
        for (coolDownTimer = 0.001f; coolDownTimer < coolDown; coolDownTimer += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        coolDownTimer = 0;

    }

    /// <summary>
    /// Overrides to stop velocity when rolling into character 
    /// </summary>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        // While rolling, check collision with character
        if (isCombatRolling && (other.tag == "Player" || other.tag == "Enemy"))
            velocity = new Vector2(0, velocity.y);

    }

    /// <summary>
    /// Overrides to update double tap detection system's timers
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (doubleTapLeftCheck)
        {
            if (doubleTapLeftTimer < doubleTapTime)
            {
                doubleTapLeftTimer += Time.fixedDeltaTime;
            }
            else
            {
                doubleTapLeftTimer = 0;
                doubleTapLeftCheck = false;
            }
        }

        if (doubleTapRightCheck)
        {
            if (doubleTapRightTimer < doubleTapTime)
            {
                doubleTapRightTimer += Time.fixedDeltaTime;
            }
            else
            {
                doubleTapRightTimer = 0;
                doubleTapRightCheck = false;
            }
        }

    }

}
