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
    public void OnEnable()
    {
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
            gameObject.SetActive(false);
        }

        base.Update();
    }

    private new void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.tag.Contains("Terrain"))
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
                        collision.gameObject.GetComponent<Character>().ReceiveAttack(new AttackInfo(Damage / 2, KnockbackImpulse * transform.right, KnockbackTime, StunTime, Source));
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
                NumPassed++;
            }
        }
    }
}
