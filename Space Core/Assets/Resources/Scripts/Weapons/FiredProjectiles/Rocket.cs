using System.Collections;
using UnityEngine;

public class Rocket : Bullet
{
    //attInfo is AttackInfo passed from Heavy Weapon
    public AttackInfo attInfo;
    public GameObject DamageRadius;
    public GameObject ParticleSystem;
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
        ParticleSystem.SetActive(true);
        baseVelocity = Velocity;
        Velocity = Velocity * startVelocityFactor;
        StartCoroutine(WaitToSpeedUp());
    }

    public override void Update()
    {

        base.Update();
    }

    //Explodes on collision with anything
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Terrain") || collision.CompareTag("Player") || collision.CompareTag("Boss")))
        {
            //Pass the attack info on to the explosion radius
            DamageRadius.GetComponent<ExplosionInformation>().Info = attInfo;
            DamageRadius.SetActive(true);
            ParticleSystem.SetActive(false);
            ObjectPooler.instance.SpawnFromPool("BMHit", gameObject.transform.position, Quaternion.identity);
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
        ParticleSystem.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Homing = false;
        transform.parent.gameObject.SetActive(false);
    }
}
