using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : InteractableEnv
{
    private void Awake()
    {
        functionalityText = "save";
    }

    /// <summary>
    /// Unlocks the lore entry provided through inspector
    /// </summary>
    public override void ActivateFunctionality()
    {
        SaveSystem.SavePlayer(Player.instance);
        Debug.Log($"Game saved with file id #{GameManager.SaveFileID}");
    }
}
