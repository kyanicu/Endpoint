using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    protected Transform RespawnPointLeft;
    protected Transform RespawnPointRight;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            RespawnPointLeft = transform.Find("RespawnPointLeft");
            RespawnPointRight = transform.Find("RespawnPointRight");
            Vector2 pos = Player.instance.transform.position;
            float leftDistance = Vector2.Distance(pos, RespawnPointLeft.position);
            float rightDistance = Vector2.Distance(pos, RespawnPointRight.position);
            if(leftDistance < rightDistance)
            {
                Vector3 spawn = RespawnPointLeft.position;
                Player.instance.transform.position = spawn;
            }
            else
            {
                Vector3 spawn = RespawnPointRight.position;
                Player.instance.transform.position = spawn;
            }
            Player.instance.ReceiveAttack(new AttackInfo(Player.instance.MaxHealth / 4, Vector2.zero, 0, 0));
        }
    }
}
