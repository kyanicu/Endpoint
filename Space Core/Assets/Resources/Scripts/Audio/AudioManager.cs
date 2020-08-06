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

<<<<<<< HEAD
    private void Awake()
=======
    // Start is called before the first frame update
    void Start()
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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
