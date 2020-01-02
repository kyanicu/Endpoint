using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using oteTag = GameManager.OneTimeEventTags;
public class DestructibleWall : MonoBehaviour
{
    public int Health;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (other.gameObject.GetComponent<Bullet>().Source == DamageSource.Player)
            {
                TakeDamage(other.gameObject.GetComponent<Bullet>().Damage);
                other.gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(int damage)
    {

        if (Health - damage <= 0)
        {
            //Add this object to one time events that get detroyed on scene load
            GameManager.OneTimeEvents.Add(name, oteTag.Destroy);
            Destroy(gameObject);
        }
        else
        {
            Health -= damage;
        }

    }
}
