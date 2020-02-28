using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Dynamic music system that changes music when player is in range of an enemy
/// </summary>
public class DynamicMusic : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] AudioClips;
    public int BattleClipIdx;
    public int BackgroundMusicClipIdx;

    //sets audio source
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        //check if any enemies are in range
        bool InRangeOfEnemy = CheckInRange();

        //set to battle music or background depending on range
        if (InRangeOfEnemy)
        {
            if (audioSource.clip.name != AudioClips[BattleClipIdx].name)
            {
                audioSource.clip = AudioClips[BattleClipIdx];
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip.name != AudioClips[BackgroundMusicClipIdx].name)
            {
                audioSource.clip = AudioClips[BackgroundMusicClipIdx];
                audioSource.Play();
            }
        }
    }

    /// <summary>
    /// Checks if there is an enemy in range of the player
    /// </summary>
    /// <returns>true if enemy in range, false otherwise</returns>
    private bool CheckInRange()
    {
        //check if any enemies are in range
        //potential null reference exception thrown here if an enemy gets killed mid for-loop.
        try
        {
            for (int i = 0; i < GameManager.EnemyControllers.Count; i++)
            {
                GameObject enemy = GameManager.EnemyControllers[i];
                if (enemy != null)
                {
                    AIController[] controllers = enemy.GetComponents<AIController>();
                    foreach (AIController controller in controllers)
                    {
                        if(controller.enabled && controller.IsPlayerInRange())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
        
    }
}
