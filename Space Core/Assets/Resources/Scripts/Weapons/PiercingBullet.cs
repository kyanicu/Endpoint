using System;

/// <summary>
/// Script that controls the bullet object for the piercingShotAbility
/// </summary>
public class PiercingBullet : Bullet
{
    //Number of enemies passed
    public int NumPassed { get; set; }
    //MaxNumber of enemies that the bullet can pass through
    public int MaxPassed { get; set; }

    /// <summary>
    /// Setting NumPassed and MaxPassed to arbitrary values for now
    /// </summary>
    private new void Start()
    {
        base.Start();
        NumPassed = 0;
        MaxPassed = 1;
    }

    /// <summary>
    /// If the Number of enemies passed is greater than MaxPassed, destroy the bullet
    /// </summary>
    private new void Update()
    {
        if (NumPassed > MaxPassed)
        {
            Destroy(gameObject);
        }

        base.Update();
    }

    private new void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.tag.Contains("Terrain"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            if (!(Enum.GetName(typeof(DamageSource), Source) == collision.tag))
            {
                collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage, KnockbackImpulse * transform.right, KnockbackTime, StunTime, Source));
                NumPassed++;
            }
        }
    }
}
