using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockFloor : MonoBehaviour
{
    private bool shock = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !(shock))
        {
            shock = true;
            Player.instance.ReceiveAttack(new AttackInfo(15, Vector2.zero, 0, 0, DamageSource.Hazard));
            shock = false;
        }
    }
}
