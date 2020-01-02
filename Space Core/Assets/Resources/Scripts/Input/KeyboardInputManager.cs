using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the input of the game through use of enum type and switch statements
/// Based on current InputState, input is handled differently
/// </summary>
public class KeyboardInputManager : InputManager
{
    private static KeyCode[] keys =
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };

    // Start is called before the first frame update
    void Start()
    {

        // Initialize state
        currentState = InputState.GAMEPLAY;

    }

    public override QTEButton.KeyNames? CheckQTEButtonPress()
    {
        if (Input.GetKeyDown(keys[0]))
        {
            return (QTEButton.KeyNames) 0;
        }
        else if (Input.GetKeyDown(keys[1]))
        {
            return (QTEButton.KeyNames) 1;
        }
        else if (Input.GetKeyDown(keys[2]))
        {
            return (QTEButton.KeyNames) 2;
        }
        else if (Input.GetKeyDown(keys[3]))
        {
            return (QTEButton.KeyNames) 3;
        }
        return null;
    }

    /// <summary>
    /// Runs the frame input intended while in the gameplay InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    protected override void RunGameplayFrameInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Player.instance.Jump();
        }
        if (Input.GetButtonUp("Jump"))
        {
            Player.instance.JumpCancel();
        }

        if (Input.GetMouseButton(0))
        {
            Player.instance.Fire();
            HUDController.instance.UpdateAmmo(Player.instance);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Player.instance.Reload();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            Player.instance.ActivateEnvironmentObj();
        }

        if (Input.GetKey(KeyCode.F))
        {
            Player.instance.HackSelector();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Toggle Diagnostic Overlays
            HUDController.instance.toggleDiagnosticPanels();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // use player ability
            Player.instance.ActivateActiveAbility();
        }

        // Toggles/deals with Pause Menu.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the pause menu isn't already open...
            if (!PauseMenuManager.instance.PauseMenuPanelIsActive)
            {
                // Open the pause menu.
                PauseMenuManager.instance.OpenPauseMenu();
            } 
        }

        Vector2 positionOnScreen = Vector2.zero;
        if (Player.instance.RotationPoint != null)
        {
            positionOnScreen = Camera.main.WorldToViewportPoint(Player.instance.RotationPoint.transform.position);
        }
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mouseOnScreen.y - positionOnScreen.y, mouseOnScreen.x - positionOnScreen.x) * Mathf.Rad2Deg;

        Player.instance.AimWeapon(angle);

        if (Input.GetKeyDown(KeyCode.Return))
            Player.instance.transform.position = new Vector2(0, 0);
    }

    /// <summary>
    /// Runs the frame input intended while in the menu InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    protected override void RunMainMenuFrameInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MainMenuManager.instance.TraverseMenu(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MainMenuManager.instance.TraverseMenu(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            MainMenuManager.instance.SelectButton();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MainMenuManager.instance.ReturnToMainMenu();
        }
    }

    /// <summary>
    /// Runs the frame input intended while in the in-game player Overlay InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    protected override void RunPlayerOverlayFrameInput()
    {

    }

    /// <summary>
    /// Runs the frame input intended while in the in-game player menu InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    protected override void RunPlayerMenuFrameInput()
    {

    }

    /// <summary>
    /// Runs the frame input intended while in the pause InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    protected override void RunPauseFrameInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            PauseMenuManager.instance.TraverseMenu(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            PauseMenuManager.instance.TraverseMenu(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            PauseMenuManager.instance.SelectButton();
        }
        // Toggles/deals with Pause Menu.
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the pause menu is already open...
            if (PauseMenuManager.instance.PauseMenuPanelIsActive)
            {
                // Close the pause menu.
                PauseMenuManager.instance.ClosePauseMenu();
            }
        }
    }

    /// <summary>
    /// Runs the fixed input intended while in the gameplay InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunGameplayFixedInput()
    {
        float horiz = Input.GetAxisRaw("Horizontal");

        Player.instance.Move(horiz);
    }

    /// <summary>
    /// Runs the fixed input intended while in the menu InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunMainMenuFixedInput()
    {

    }

    /// <summary>
    /// Runs the fixed input intended while in the in-game player Overlay InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunPlayerOverlayFixedInput()
    {

    }

    /// <summary>
    /// Runs the fixed input intended while in the in-game player menu InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunPlayerMenuFixedInput()
    {

    }

    /// <summary>
    /// Runs the fixed input intended while in the gameplay InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunPauseFixedInput()
    {

    }

    
}
