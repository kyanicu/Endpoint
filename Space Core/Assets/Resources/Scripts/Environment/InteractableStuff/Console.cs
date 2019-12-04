using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : InteractableEnv
{
    [Header("Lore Entry to Unlock")]
    public string EntryName;
    public string EntryArticle;

    private bool alreadyPressed;

    private void Awake()
    {
        functionalityText = "access console";
    }

    /// <summary>
    /// Unlocks the lore entry provided through inspector
    /// </summary>
    public override void ActivateFunctionality()
    {
        if (!alreadyPressed)
        {
            LoadDataBaseEntries.UnlockDataEntry(EntryName, EntryArticle);
            string[] unlockedData = { EntryName, EntryArticle };
            HUDController.instance.RecentDataBaseEntry = unlockedData;
            HUDController.instance.InitiateDatabasePopup();
            alreadyPressed = true;
        }
    }
}
