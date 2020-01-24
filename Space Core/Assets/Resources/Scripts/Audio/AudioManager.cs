using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] SoundEffects;
    public static AudioManager instance;
    //Add an id for each audio clip called in game
    public enum Clips
    {
        MenuSelect,
        MenuScroll,
        Back,
        Deny
    }

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a clip given its sound id
    /// </summary>
    /// <param name="clipName"></param>
    public void PlaySound(Clips clipName)
    {
        audioSource.clip = SoundEffects[(int)clipName];
        audioSource.Play();
    }
}
