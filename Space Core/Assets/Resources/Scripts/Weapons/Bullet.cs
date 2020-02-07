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
    public int Damage { get; set; }
    public float StunTime { get; set; }
    public float KnockbackImpulse { get; set; }
    public float KnockbackTime { get; set; }
    public float Velocity { get; set; }
    public DamageSource Source { get; set; }
    public float Range { get; set; }
    protected float startX;
    protected float lowRange;
    protected float highRange;

    /// <summary>
    /// Initialize start x to the base x position of the bullet
    /// </summary>
    public void Activate()
    {
        startX = transform.position.x;
        highRange = startX + Range;
        lowRange = startX - Range;
    }

    /// <summary>
    /// Move the bullet forward until it is out of range
    /// </summary>
    public void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        //if we have travelled outside the range, destroy the bullet
        if (transform.position.x > highRange || transform.position.x < lowRange)
        {
            gameObject.SetActive(false);
        }

        transform.position += (transform.right * Velocity * Time.deltaTime);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
        {
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!(Enum.GetName(typeof(DamageSource), Source) == collision.tag))
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
                        collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage/2, KnockbackImpulse * transform.right, KnockbackTime, StunTime, Source));
                    }
                    else if (gameObject.transform.position.x > PlayerController.instance.Character.transform.position.x && !PlayerController.instance.isFacingLeft)
                    {
                        collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage / 2, KnockbackImpulse * transform.right, KnockbackTime, StunTime, Source));
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage * 1.25f, KnockbackImpulse * transform.right, KnockbackTime, StunTime, Source));
                    }
                }
                else
                {
                    collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage, KnockbackImpulse * transform.right, KnockbackTime, StunTime, Source));
                }
                gameObject.SetActive(false);
            }
        }
    }
}
