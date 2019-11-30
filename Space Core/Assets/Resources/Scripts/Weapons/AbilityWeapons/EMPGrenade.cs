using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPGrenade : MonoBehaviour
{
    private float Radius;
    public ParticleSystem psImplosion;
    public ParticleSystem psExplosion;

    // Start is called before the first frame update
    void Start()
    {
        Radius = 10;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Terrain"))
        {
            psImplosion.transform.parent.gameObject.SetActive(true);
            psImplosion.Play();
            psExplosion.Play();
            Collider[] Colliders = Physics.OverlapSphere(transform.localPosition, Radius);
            foreach (Collider collider in Colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.gameObject.GetComponent<Enemy>().Freeze();
                }
            }
            Destroy(gameObject);
        }        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Terrain")
        {
            Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.localPosition, Radius);
            foreach (Collider2D collider in Colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.gameObject.GetComponent<Enemy>().Freeze();
                }
            }
            Destroy(gameObject);
        }
    }
}
