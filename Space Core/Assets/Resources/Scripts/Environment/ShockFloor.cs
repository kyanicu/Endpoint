using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockFloor : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player.instance.TakeDamage(15);
        }
    }
}
