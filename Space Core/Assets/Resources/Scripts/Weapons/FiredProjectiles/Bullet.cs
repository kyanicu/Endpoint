using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds all information the bullet may need
/// </summary>
public class Bullet : MonoBehaviour
{
    //enum source to let objects know who fired the bullet
    public static bool VampireBullet { get; set; }
    public bool Homing { get; set; }
    public float Damage { get; set; }
    public float StunTime { get; set; }
    public float KnockbackImpulse { get; set; }
    public float KnockbackTime { get; set; }
    public float Velocity { get; set; }
    public DamageSource Source { get; set; }
    //Enemy that the bullet will lock onto
    public Transform LockedEnemy;
    //radar for homing into an enemy. Need to update its position as well.
    public GameObject HomingRadar;
    public float Range { get; set; }
    protected float startX;
    protected float lowRange;
    protected float highRange;
    protected float spreadFallOfSpeed;
    protected float baseDamage;


    /// <summary>
    /// Initialize start x to the base x position of the bullet
    /// </summary>
    public virtual void Activate()
    {
        startX = transform.position.x;
        highRange = startX + Range;
        lowRange = startX - Range;
        spreadFallOfSpeed = (Damage / Range) * 0.5f;
        baseDamage = Damage;
    }

    /// <summary>
    /// Move the bullet forward until it is out of range
    /// </summary>
    public virtual void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        //if we have travelled outside the range, destroy the bullet
        if (transform.position.x > highRange || transform.position.x < lowRange)
        {
            gameObject.SetActive(false);
        }

        if (Source == DamageSource.Spread && Damage > baseDamage / 2.0f)
        {
            Damage -= spreadFallOfSpeed * Time.deltaTime;
        }

        if (Homing && LockedEnemy != null)
        {
            Vector3 targetDirection = LockedEnemy.transform.position - transform.position;
            transform.position += (targetDirection.normalized * Velocity * Time.deltaTime);
            HomingRadar.transform.position += (targetDirection.normalized * Velocity * Time.deltaTime);
        }
        else
        {
            HomingRadar.transform.position += (transform.right * Velocity * Time.deltaTime);
            transform.position += (transform.right * Velocity * Time.deltaTime);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
        {
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!(Enum.GetName(typeof(DamageSource), Source) == collision.tag))
            {
                DealDamage(collision);
                gameObject.SetActive(false);
            }
        }
    }

    protected void DealDamage(Collider2D collision)
    {
        
        if (VampireBullet && Source == DamageSource.Player)
        {
            float healthToHeal = Damage * 0.25f;
            if (healthToHeal < 0)
            {
                PlayerController.instance.HealCharacter(1);
            }
            else
            {
                PlayerController.instance.HealCharacter((int)healthToHeal);
            }
        }

        if (PlayerController.instance.ForceCompensatorActive && Source == DamageSource.Enemy)
        {
            if (gameObject.transform.position.x < PlayerController.instance.Character.transform.position.x && PlayerController.instance.isFacingLeft)
            {
                collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage / 2, Vector2.zero, 0, StunTime, Source));
            }
            else if (gameObject.transform.position.x > PlayerController.instance.Character.transform.position.x && !PlayerController.instance.isFacingLeft)
            {
                collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage / 2, Vector2.zero, 0, StunTime, Source));
            }
            else
            {
                collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage * 1.25f, Vector2.zero, 0, StunTime, Source));
            }
        }
        else
        {
            collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage, Vector2.zero, 0, StunTime, Source));
        }
    }
    
}
