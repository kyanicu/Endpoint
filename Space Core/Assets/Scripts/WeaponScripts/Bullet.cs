using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get; set; }
    public Vector3 Movement = new Vector3(0.5f, 0);

    public void Update()
    {
        transform.position += Movement;
    }
}
