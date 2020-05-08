using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRadius : MonoBehaviour
{
    public int Damage;
    public int Knockback;
    public float KnockbackTime;
    public float StunTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.ReceiveAttack(new AttackInfo(Damage, Vector2.zero, KnockbackTime, StunTime, DamageSource.Enemy));
        }
    }
}
