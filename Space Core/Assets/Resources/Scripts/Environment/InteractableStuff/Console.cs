using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : InteractableEnv
{
    [Header("Lore Entry to Unlock")]
    public string EntryName;
    public string EntryArticle;

    private bool alreadyPressed;

    /// <summary>
    /// Unlocks the lore entry provided through inspector
    /// </summary>
    public override void ActivateFunctionality()
    {
        if (!alreadyPressed)
        {
            LoadDataBaseEntries.UnlockDataEntry(EntryName, EntryArticle);
            alreadyPressed = true;
        }
    }
}
