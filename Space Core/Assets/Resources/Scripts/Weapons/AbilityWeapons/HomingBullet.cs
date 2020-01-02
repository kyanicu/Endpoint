using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Homing bullets lock onto nearest enemy in the direction it is going and moves towards that enemy
/// </summary>
public class HomingBullet : Bullet
{
    //Enemy that the bullet will lock onto
    public Transform LockedEnemy;
    //radar for homing into an enemy. Need to update its position as well.
    public GameObject HomingRadar;

    // Start is called before the first frame update
    public new void Activate()
    {
        base.Activate();
        LockedEnemy = null;
    }

    /// <summary>
    /// Have the bullet lock onto and change direction to go towards enemy
    /// </summary>
    new void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        if (LockedEnemy != null)
        {
            Vector3 targetDirection = LockedEnemy.transform.position - transform.position;
            transform.position += (targetDirection.normalized * Velocity * Time.deltaTime);
            HomingRadar.transform.position += (targetDirection.normalized * Velocity * Time.deltaTime);
        }
        else
        {
            HomingRadar.transform.position += (transform.right * Velocity * Time.deltaTime);
            base.Update();
        }
    }

    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
        {
            transform.parent.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!(Enum.GetName(typeof(DamageSource), Source) == collision.tag))
            {
                collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage, KnockbackImpulse * transform.right, KnockbackTime, StunTime, Source));
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
