using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableParticle : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void OnEnable()
    {
        particleSystem.Play();
    }

    private void Awake()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particleSystem.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
