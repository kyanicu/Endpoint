using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    //attInfo is AttackInfo passed from Heavy Weapon
    public AttackInfo attInfo;
    public GameObject DamageRadius;
    private MeshRenderer meshRenderer;
    private float explosionTime = .5f;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    //Explodes on collision with anything
    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Terrain") || collision.CompareTag("Player")))
        {
            //Pass the attack info on to the explosion radius
            DamageRadius.GetComponent<ExplosionInformation>().Info = attInfo;
            DamageRadius.SetActive(true);
            meshRenderer.enabled = false;
            StartCoroutine(Explosion());
            //ObjectPooler.instance.SpawnFromPool("RocketExplosion", transform.position, Quaternion.identity);
        }
    }

    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(explosionTime);
        DamageRadius.SetActive(false);
        meshRenderer.enabled = true;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
