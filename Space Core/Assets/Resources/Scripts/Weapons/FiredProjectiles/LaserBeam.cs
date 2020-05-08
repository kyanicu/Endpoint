using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public int Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.ReceiveAttack(new AttackInfo(Damage, Vector2.zero, 0, 0, DamageSource.Enemy));
        }
    }
}
