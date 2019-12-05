using System;
using UnityEngine;

/// <summary>
/// Class that holds all information the bullet may need
/// </summary>
public class Bullet : MonoBehaviour
{
    //enum source to let objects know who fired the bullet

    public int Damage { get; set; }
    public float StunTime { get; set; }
    public float KnockbackImpulse { get; set; }
    public float KnockbackTime { get; set; }
    public float Range { get; set; }
    public float Velocity { get; set; }
    public DamageSource Source { get; set; }
    protected float startX;
    protected float lowRange;
    protected float highRange;

    /// <summary>
    /// Initialize start x to the base x position of the bullet
    /// </summary>
    public void Start()
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
        if (collision.tag.Contains("Terrain"))
        {
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!(Enum.GetName(typeof(DamageSource), Source) == collision.tag))
            {
                collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage, KnockbackImpulse * transform.right, KnockbackTime, StunTime, Source));
                gameObject.SetActive(false);
            }
        }
    }
}
