﻿using UnityEngine;
using oteTag = GameManager.OneTimeEventTags;

public class Console : InteractableEnv
{
    [Header("Lore Entry to Unlock")]
    public string EntryName;
    public string EntryArticle;

<<<<<<< HEAD
    public int ExperienceGiven = -1;
    private const int DEFAULT_EXPERIENCE_GIVEN = 50;

=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
    public bool AlreadyPressed { private get; set; }

    private void Awake()
    {
        functionalityText = "access console";
        if (ExperienceGiven == -1)
        {
            ExperienceGiven = DEFAULT_EXPERIENCE_GIVEN;
        }
    }

    // Opens the terminal window.
    public override void ActivateFunctionality()
    {
        if (!AlreadyPressed)
        {
<<<<<<< HEAD
            // Open terminal window, and pass this object.
            // Object is passed so functionality can be executed on terminal window close.
            TerminalWindow.instance.openLoreWindow(this);
=======
            //Add this object to one time events that get unlocked on scene load
            GameManager.OneTimeEvents.Add(name, oteTag.Console);
            LoadDataBaseEntries.UnlockDataEntry(EntryName, EntryArticle);
            AlreadyPressed = true;
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
        }
    }

    // RunFunctionality: Actually runs the functionality of this object (unlocks lore, adds experience.)
    public void RunFunctionality()
    {
        //Add this object to one time events that get unlocked on scene load
        GameManager.OneTimeEvents.Add(name, oteTag.Console);
        LoadDataBaseEntries.UnlockDataEntry(EntryName, EntryArticle);

        // Add experience if first time interacting with terminal
        ExperienceSystem.instance.AddPlayerExperience(ExperienceGiven);

        AlreadyPressed = true;
    }
}
