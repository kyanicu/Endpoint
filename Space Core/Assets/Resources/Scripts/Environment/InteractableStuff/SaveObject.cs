using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : InteractableEnv
{
    private void Awake()
    {
        functionalityText = "save";
    }

    public override void ActivateFunctionality()
    {
        // Open terminal window, and pass this object.
        // Object is passed so functionality can be executed on terminal window close.
        TerminalWindow.instance.openSaveWindow(this);
    }

    // RunFunctionality: Actually runs the functionality of this object (save game.)
    public void RunFunctionality()
    {
        SaveSystem.SavePlayer(PlayerController.instance.Character);
        Debug.Log($"Game saved with file id #{GameManager.SaveFileID}");

        //Open save popup on HUD
        HUDController.instance.InitiateSavePopup();
    }
}
