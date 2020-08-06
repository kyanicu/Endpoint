using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    // Player's current level
    public int level;

    // Player's current experience
    public int experience;
    // Player's current total sum of experience, including all levels
    public int totalExperience;

    // How much experience the player needs to progress to the next level
    public int nextLevelExperience;
    // How much experience the player needs to progress to the next level, including all levels
    public int totalNextLevelExperience;

    private static ExperienceSystem _instance;
    public static ExperienceSystem instance { get { return _instance; } }

    private void Awake()
    {
        // Setup singleton
        if (_instance == null)
        {
            _instance = this;
        }
    }

    // Initializes default values for the experience system, if no save file is loaded.
    public void StartExperienceSystem()
    {
        level = 0;
        experience = 0;
        totalExperience = 0;
        nextLevelExperience = 100;
        totalNextLevelExperience = 100;
    }

    // Adds experience to the system.
    public void AddPlayerExperience(int addXP)
    {
        // Add gained experience to current experience level.
        experience = experience + addXP;
        totalExperience = totalExperience + addXP;
        // Update player's level.
        updatePlayerLevel();
        // Update UI.
        HUDController.instance.UpdateExperiencePanel(level, experience, nextLevelExperience);
        OverlayManager.instance.UpdatePlayerLevelUpgrades(level);
        // Debug.Log("New XP: " + experience.ToString() + " Total XP: " + totalExperience.ToString() + " nextLevelExperience: " + nextLevelExperience.ToString() + " totalNextLevelExperience: " + totalNextLevelExperience.ToString());
    }

    // Checks if it's time for a level up.
    private void updatePlayerLevel()
    {
        // Check if current experience is greater than next level experience. If so, level the player up.
        if (totalExperience > totalNextLevelExperience)
        {
            levelUp();
        }
    }

    // Levels up the player.
    private void levelUp()
    {
        // Level up!
        level = level + 1;

        // Find out how much experience is needed for the next level to happen.
        int newLevelExperienceNeeded = ((level + 1) * 100);

        // Update the total experience needed for next level.
        totalNextLevelExperience = totalNextLevelExperience + newLevelExperienceNeeded;
        // Reset the small next level experience level;
        nextLevelExperience = newLevelExperienceNeeded;

        // Reset the small experience level.
        experience = nextLevelExperience - (totalNextLevelExperience - totalExperience);

        // If after all that, the player still has more experience than needed for level up, re-run this function.
        if (totalExperience > totalNextLevelExperience)
        {
            levelUp();
        }
    }
}
