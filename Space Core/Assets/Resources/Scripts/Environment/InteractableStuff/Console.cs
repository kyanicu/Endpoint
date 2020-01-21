using UnityEngine;
using oteTag = GameManager.OneTimeEventTags;

public class Console : InteractableEnv
{
    [Header("Lore Entry to Unlock")]
    public string EntryName;
    public string EntryArticle;

    public bool AlreadyPressed { private get; set; }

    private void Awake()
    {
        functionalityText = "access console";
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
            AlreadyPressed = true;
        }
    }
}
