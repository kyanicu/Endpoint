using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : ActiveAbility
{
    protected override bool activationCondition
<<<<<<< HEAD
        { get { return owner.movement.charCont.isGrounded && owner.isStunned <= 0 && activationTimer <= 0f; } }
=======
        { get { return owner.movement.charCont.isGrounded && !owner.isStunned && activationTimer <= 0f; } }
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2

    private bool isDashing;

    private float dashTime = 1;
    private float dashSpeed = 20;
    private int damage = 5;
    private float knockBackX = 20, knockBackY = 15;
    private float knockbackTime = 0.2f;
    private float stunTime = 0.5f;

    public void Start()
    {
        Cooldown = 10f;
    }

    // The method that runs the Ability
    protected override void Activate()
    {
        StartCoroutine(Dash());
    }

    private new void Awake()
    {
        base.Awake();
        AbilityName = "Dash Attack";
        AbilityShortName = "DASH";
        AbilityDescription = "Push RB to dash forward towards enemies and hit them.";
    }

    /// <summary>
    /// Initiates the dash, canceling when hitting a wall or ungrounding
    /// </summary>
    public IEnumerator Dash()
    {
        int facingDirection = owner.facingDirection;
        
        // Set dash values
        isDashing = true;
        owner.movement.collideWithCharacters = false;
        owner.isStunned++;
        owner.movement.freezeRun = true;
        owner.Invincible++;
        owner.movement.velocity = owner.movement.charCont.currentSlope * facingDirection * dashSpeed;
        
        // Check conditional during dash
        for (float i = 0; i < dashTime; i += Time.fixedDeltaTime)
        {
            // Is dash condition no longer valid?
            if (!owner.movement.charCont.isGrounded 
                || owner.movement.charCont.isTouchingRightWall 
                || owner.movement.charCont.isTouchingLeftWall)
            {
                //owner.movement.TakeKnockback(new Vector2(knockBackX * -facingDirection, knockBackY));
                break;
            }

            yield return new WaitForFixedUpdate();
        }
        
        // Reset dash values
        isDashing = false;
        owner.movement.collideWithCharacters = true;
        owner.isStunned--;
        owner.movement.freezeRun = false;
        owner.Invincible--;
        activationTimer = Cooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // While dashing, check collision with character
        if (isDashing && (other.tag == "Player" || other.tag == "Enemy"))
            other.GetComponent<Character>().ReceiveAttack(
                new AttackInfo(
                    damage,
                    new Vector2(knockBackX * owner.facingDirection, knockBackY),
                    knockbackTime,
                    stunTime,
                    DamageSource.Enemy));
    }

}
