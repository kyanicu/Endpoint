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
<<<<<<< HEAD
        // Open terminal window, and pass this object.
        // Object is passed so functionality can be executed on terminal window close.
        TerminalWindow.instance.openSaveWindow(this);
    }

    // RunFunctionality: Actually runs the functionality of this object (save game.)
    public void RunFunctionality()
    {
=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
        SaveSystem.SavePlayer(PlayerController.instance.Character);
        Debug.Log($"Game saved with file id #{GameManager.SaveFileID}");

        //Open save popup on HUD
        HUDController.instance.InitiateSavePopup();
    }
}
