using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockFloor : MonoBehaviour
{
    [SerializeField] 
    private int damage;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player.instance.ReceiveAttack(new AttackInfo(damage*Time.fixedDeltaTime, Vector2.zero, 0, 0, DamageSource.Hazard));
        }
    }
}
