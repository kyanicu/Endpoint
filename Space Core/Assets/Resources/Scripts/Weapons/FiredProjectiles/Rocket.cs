using System.Collections;
using UnityEngine;

public class Rocket : Bullet
{
    //attInfo is AttackInfo passed from Heavy Weapon
    public AttackInfo attInfo;
    public GameObject DamageRadius;
    private MeshRenderer meshRenderer;
    private float explosionTime = .5f;
    private float baseVelocity;
    private float startVelocityFactor = .2f;
    private float startUpTime = .2f;

    /// <summary>
    /// Acitvate override for getting the mesh renderer
    /// </summary>
    public override void Activate()
    {
        base.Activate();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        baseVelocity = Velocity;
        Velocity = Velocity * startVelocityFactor;
        StartCoroutine(WaitToSpeedUp());
    }

    //Explodes on collision with anything
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Terrain") || collision.CompareTag("Player") || collision.CompareTag("Boss")))
        {
            //Pass the attack info on to the explosion radius
            DamageRadius.GetComponent<ExplosionInformation>().Info = attInfo;
            DamageRadius.SetActive(true);
            meshRenderer.enabled = false;
            StartCoroutine(Explosion());
            Velocity = baseVelocity;
            //ObjectPooler.instance.SpawnFromPool("RocketExplosion", transform.position, Quaternion.identity);
        }
    }

    private IEnumerator WaitToSpeedUp()
    {
        yield return new WaitForSeconds(startUpTime);
        Velocity = baseVelocity;
    }

    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(explosionTime);
        DamageRadius.SetActive(false);
        meshRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
