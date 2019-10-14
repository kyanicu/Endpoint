using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health = 32;

    void Start()
    {
        WeaponGenerator.GenerateWeapon(transform.Find("WeaponLocation"));
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            TakeDamage(other.gameObject.GetComponent<Bullet>().Damage);
            Destroy(other.gameObject);
        }
    }
}
