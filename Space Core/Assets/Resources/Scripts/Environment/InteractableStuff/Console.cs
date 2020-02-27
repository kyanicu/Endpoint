using UnityEngine;
using oteTag = GameManager.OneTimeEventTags;

public class Console : InteractableEnv
{
    [Header("Lore Entry to Unlock")]
    public string EntryName;
    public string EntryArticle;

    public int ExperienceGiven = -1;
    private const int DEFAULT_EXPERIENCE_GIVEN = 50;

    public bool AlreadyPressed { private get; set; }

    private void Awake()
    {
        functionalityText = "access console";
        if (ExperienceGiven == -1)
        {
            ExperienceGiven = DEFAULT_EXPERIENCE_GIVEN;
        }
    }

    /// <summary>
    /// Unlocks the lore entry provided through inspector
    /// </summary>
    public override void ActivateFunctionality()
    {
        if (!AlreadyPressed)
        {
            //Add this object to one time events that get unlocked on scene load
            GameManager.OneTimeEvents.Add(name, oteTag.Console);
            LoadDataBaseEntries.UnlockDataEntry(EntryName, EntryArticle);

            // Add experience if first time interacting with terminal
            ExperienceSystem.instance.AddPlayerExperience(ExperienceGiven);

            AlreadyPressed = true;
        }
    }
}
