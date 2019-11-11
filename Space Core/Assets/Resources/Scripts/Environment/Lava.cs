using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player.instance.TakeDamage(Player.instance.MaxHealth / 4);
        //Return player to last location they were at
    }
}
