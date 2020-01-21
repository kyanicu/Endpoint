using UnityEngine;
using System.Collections;
 
 public class OnParticleEnd : MonoBehaviour
{
    private ParticleSystem[] systems;
    private float waitTime = 0;


    public void Awake()
    {
        systems = GetComponentsInChildren<ParticleSystem>();

        //Loop through each particle and find the highest particle duration
        foreach (ParticleSystem particles in systems)
        {
            if (particles.main.duration > waitTime)
                waitTime = particles.main.duration;
        }
    }

    public void OnEnable()
    {
        StartCoroutine(particleDuration());
    }

    private IEnumerator particleDuration()
    {
        //Wait until the longest particle system has ended
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
    }
}