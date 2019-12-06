using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPGrenade : MonoBehaviour
{
    public ParticleSystem psImplosion;
    public ParticleSystem psExplosion;
    public GameObject ExplosionRadius;

    [SerializeField]
    private float explosionTime;
    private MeshRenderer meshRenderer;
    private bool activated;

    private void Start()
    {
        activated = false;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// When it collides with an enemy or terrain, then it will activate the emp damage area
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("Terrain")) && !activated)
        {
            activated = true;
            ExplosionRadius.SetActive(true);
            meshRenderer.enabled = false;
            StartCoroutine(Explosion());
            ObjectPooler.instance.SpawnFromPool("EMPParticle", transform.position, Quaternion.identity);
        }        
    }

    /// <summary>
    /// Explosion released by the EMP grenade
    /// </summary>
    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(explosionTime);
        ExplosionRadius.SetActive(false);
        meshRenderer.enabled = true;
        gameObject.SetActive(false);
        activated = false;
    }
}
