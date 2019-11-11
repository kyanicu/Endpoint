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
            StartCoroutine(Damage());
        }
    }

    private IEnumerator Damage()
    {
        yield return new WaitForSeconds(1f);
        Player.instance.TakeDamage(15);
        shock = false;
    }
}
