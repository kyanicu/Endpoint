using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackBullet : MonoBehaviour
{
    private float Speed = 45f;

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
            Player.instance.Enemy = col.gameObject.GetComponent<Enemy>();
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }
}
