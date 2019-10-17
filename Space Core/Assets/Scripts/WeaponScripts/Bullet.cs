using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get; set; }
    public float Range { get; set; }
    public float Movement = 0.4f;
    private float startX;

    public void Start()
    {
        startX = transform.position.x;
    }

    public void Update()
    {
        if (transform.position.x + startX > startX + Range)
        {
            Destroy(gameObject);
        }
        transform.position += (transform.right * Movement);
    }
}
