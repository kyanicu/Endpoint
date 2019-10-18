﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackBullet : MonoBehaviour
{
    public float Speed = 15f;

    public void Start()
    {
        Destroy(gameObject, 5f);
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Player.instance.enemy = col.gameObject.GetComponent<Enemy>();
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }
}
