using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

/// <summary>
/// Controls the input of the game through use of enum type and switch statements
/// Based on current InputState, input is handled differently
/// </summary>
public class ControllerInputManager : InputManager
{
    private GamePadState? state;
    private GamePadState? prevState;
    private bool jumped;

    // Start is called before the first frame update
    void Start()
    {
        jumped = false;
        state = null;
        prevState = null;
    }

    new void Update()
    {
        GetControllerState();
        base.Update();
    }

    public override QTEButton.KeyNames? CheckQTEButtonPress()
    {
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return null;
        }

        if (state.Value.Buttons.A == ButtonState.Pressed && prevState.Value.Buttons.A == ButtonState.Released)
        {
            return (QTEButton.KeyNames) 0;
        }
        else if (state.Value.Buttons.B == ButtonState.Pressed && prevState.Value.Buttons.B == ButtonState.Released)
        {
            return (QTEButton.KeyNames) 1;
        }
        else if (state.Value.Buttons.X == ButtonState.Pressed && prevState.Value.Buttons.X == ButtonState.Released)
        {
            return (QTEButton.KeyNames) 2;
        }
        else if (state.Value.Buttons.Y == ButtonState.Pressed && prevState.Value.Buttons.Y == ButtonState.Released)
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
        if(!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }

        #region Triggers

        if (state.Value.Triggers.Right > 0.3f)
        {
            PlayerController.instance.Fire();
            HUDController.instance.UpdateAmmo(PlayerController.instance.Character);
        }

        if (state.Value.Triggers.Left > 0.3f && prevState.Value.Triggers.Left < 0.3f)
        {
            PlayerController.instance.Jump();
        }
        else if (state.Value.Triggers.Left == 0)
        {
            PlayerController.instance.JumpCancel();
        }

        #endregion

        #region ABXY Buttons

        if (prevState.Value.Buttons.A == ButtonState.Released && state.Value.Buttons.A == ButtonState.Pressed)
        {
            PlayerController.instance.ActivateEnvironmentObj();
        }

        if (prevState.Value.Buttons.B == ButtonState.Released && state.Value.Buttons.B == ButtonState.Pressed)
        {
            HUDController.instance.UpdateHUD(PlayerController.instance.Character);
        }

        if (prevState.Value.Buttons.X == ButtonState.Released && state.Value.Buttons.X == ButtonState.Pressed)
        {
            PlayerController.instance.Reload();
        }

        if (prevState.Value.Buttons.Y == ButtonState.Released && state.Value.Buttons.Y == ButtonState.Pressed)
        {
            //TBD
        }

        #endregion

        #region Control Stick

        if (state.Value.ThumbSticks.Right.X != 0 || state.Value.ThumbSticks.Right.Y != 0)
        {
            float angle = Mathf.Atan2(state.Value.ThumbSticks.Right.Y, state.Value.ThumbSticks.Right.X) * Mathf.Rad2Deg;
            PlayerController.instance.AimWeapon(angle);
        }

        if (prevState.Value.Buttons.RightStick == ButtonState.Released && state.Value.Buttons.RightStick == ButtonState.Pressed)
        {
            // Toggle Diagnostic Panels
            HUDController.instance.toggleDiagnosticPanels();
        }

        if (prevState.Value.Buttons.LeftStick == ButtonState.Released && state.Value.Buttons.LeftStick == ButtonState.Pressed)
        {
            //TBD
        }

        #endregion

        #region Guide Buttons

        if (prevState.Value.Buttons.Start == ButtonState.Released && state.Value.Buttons.Start == ButtonState.Pressed)
        {
            // If the pause menu isn't already open...
            if (!PauseMenuManager.instance.PauseMenuPanelIsActive)
            {
                // Open the pause menu.
                PauseMenuManager.instance.OpenPauseMenu();
            }
        }

        if (prevState.Value.Buttons.Back == ButtonState.Released && state.Value.Buttons.Back == ButtonState.Pressed)
        {
            //Make sure overlay can only be opened if not loading 
            if (HUDController.instance.visible)
            {
                currentState = InputState.OVERLAY;
                OverlayManager.instance.ToggleOverlayVisibility();
            }
        }

        #endregion

        #region Shoulder Buttons

        if (prevState.Value.Buttons.RightShoulder == ButtonState.Released && state.Value.Buttons.RightShoulder == ButtonState.Pressed)
        {
            PlayerController.instance.ActivateActiveAbility();
        }

        if (prevState.Value.Buttons.LeftShoulder == ButtonState.Released && state.Value.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            PlayerController.instance.HackSelector();
        }

        #endregion

        #region D-Pad
        /*
        float horiz = 0;
        float vert = 0;
        if (state.Value.DPad.Right == ButtonState.Pressed)
        {
            horiz = +1f;
        }
        else if (state.Value.DPad.Left == ButtonState.Pressed)
        {
            horiz = -1f;
        }
        if (state.Value.DPad.Up == ButtonState.Pressed)
        {
            vert = +1f;
        }
        else if (state.Value.DPad.Down == ButtonState.Pressed)
        {
            vert = -1f;
        }

        Vector2 direction = horiz * Vector2.right + vert * Vector2.up;
        PlayerController.instance.Move(direction);
        */    
        #endregion
        
    }

    /// <summary>
    /// Runs the frame input intended while in the main menu InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    protected override void RunMainMenuFrameInput()
    {
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }

        //Check vertical movement through menu with D-Pad
        if (state.Value.DPad.Up == ButtonState.Pressed && prevState.Value.DPad.Up == ButtonState.Released)
        {
            MainMenuManager.instance.TraverseMenu(-1);
        }
        else if (state.Value.DPad.Down == ButtonState.Pressed && prevState.Value.DPad.Down == ButtonState.Released)
        {
            MainMenuManager.instance.TraverseMenu(1);
        }
        //Check vertical movement through menu with Left Stick
        else if (state.Value.ThumbSticks.Left.Y != 0 && prevState.Value.ThumbSticks.Left.Y == 0)
        {
            MainMenuManager.instance.TraverseMenu(state.Value.ThumbSticks.Left.Y * -1);
        }

        //If player selects the currently highlighted button, invoke it
        if (state.Value.Buttons.A == ButtonState.Pressed && prevState.Value.Buttons.A == ButtonState.Released)
        {
            MainMenuManager.instance.SelectButton();
        }
        else if (state.Value.Buttons.B == ButtonState.Pressed && prevState.Value.Buttons.B == ButtonState.Released)
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
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }

        #region Back Button
        if (prevState.Value.Buttons.Back == ButtonState.Released && state.Value.Buttons.Back == ButtonState.Pressed)
        {
            OverlayManager.instance.ToggleOverlayVisibility();
            currentState = InputState.GAMEPLAY;
        }
        #endregion

        #region Shoulder Buttons
        if (prevState.Value.Buttons.RightShoulder == ButtonState.Released && state.Value.Buttons.RightShoulder == ButtonState.Pressed)
        {
            OverlayManager.instance.NavigateOverlay(-1);
        }

        if (prevState.Value.Buttons.LeftShoulder == ButtonState.Released && state.Value.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            OverlayManager.instance.NavigateOverlay(1);
        }
        #endregion

        #region Thumbsticks
        //Check for left stick flick movement
        if (state.Value.ThumbSticks.Left.X != 0 && prevState.Value.ThumbSticks.Left.X == 0 ||
            state.Value.ThumbSticks.Left.Y != 0 && prevState.Value.ThumbSticks.Left.Y == 0)
        {
            OverlayManager.instance.ReceiveLeftStickFlickInput(state.Value.ThumbSticks.Left.X, state.Value.ThumbSticks.Left.Y);
        }
        //Check if player is still holding left stick
        else if (state.Value.ThumbSticks.Left.X != 0 && prevState.Value.ThumbSticks.Left.X != 0 ||
            state.Value.ThumbSticks.Left.Y != 0 && prevState.Value.ThumbSticks.Left.Y != 0)
        {
            OverlayManager.instance.ReceiveLeftStickDragInput(state.Value.ThumbSticks.Left.X, state.Value.ThumbSticks.Left.Y);
        }
        //Check if player is still holding right stick
        if (state.Value.ThumbSticks.Right.X != 0 && prevState.Value.ThumbSticks.Right.X == 0 ||
            state.Value.ThumbSticks.Right.Y != 0 && prevState.Value.ThumbSticks.Right.Y == 0)
        {
            OverlayManager.instance.ReceiveRightStickInput(state.Value.ThumbSticks.Right.X, state.Value.ThumbSticks.Right.Y);
        }
        #endregion

        #region ABXY
        //Check for A button press
        if (state.Value.Buttons.A == ButtonState.Pressed && prevState.Value.Buttons.A == ButtonState.Released)
        {
            OverlayManager.instance.ReceiveFaceButtonInput("a");
        }
        //Check for B button press
        if (state.Value.Buttons.B == ButtonState.Pressed && prevState.Value.Buttons.B == ButtonState.Released)
        {
            OverlayManager.instance.ReceiveFaceButtonInput("b");
        }
        //Check for X button press
        if (state.Value.Buttons.X == ButtonState.Pressed && prevState.Value.Buttons.X == ButtonState.Released)
        {
            OverlayManager.instance.ReceiveFaceButtonInput("x");
        }
        //Check for Y button press
        if (state.Value.Buttons.Y == ButtonState.Pressed && prevState.Value.Buttons.Y == ButtonState.Released)
        {
            OverlayManager.instance.ReceiveFaceButtonInput("y");
        }
        #endregion

        #region Triggers
        //Check for right trigger press
        if (state.Value.Triggers.Right > 0.3f)
        {
            OverlayManager.instance.ReceiveTriggerInput(true);
        }
        //Check for left trigger press
        if (state.Value.Triggers.Left > 0.3f)
        {
            OverlayManager.instance.ReceiveTriggerInput(false);
        }
        #endregion
    }

        /// <summary>
        /// Runs the frame input intended while in the in-game player menu InputState
        /// Called only on an update frame through Update() function
        /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
        /// </summary>
    protected override void RunPlayerMenuFrameInput()
    {
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }
    }

    /// <summary>
    /// Runs the frame input intended while in the pause InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    protected override void RunPauseFrameInput()
    {
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }

        //Check vertical movement through menu with D-Pad
        if (state.Value.DPad.Up == ButtonState.Pressed && prevState.Value.DPad.Up == ButtonState.Released)
        {
            PauseMenuManager.instance.TraverseMenu(-1);
        }
        else if (state.Value.DPad.Down == ButtonState.Pressed && prevState.Value.DPad.Down == ButtonState.Released)
        {
            PauseMenuManager.instance.TraverseMenu(1);
        }
        //Check vertical movement through menu with Left Stick
        else if (state.Value.ThumbSticks.Left.Y != 0 && prevState.Value.ThumbSticks.Left.Y == 0)
        {
            PauseMenuManager.instance.TraverseMenu(state.Value.ThumbSticks.Left.Y * -1);
        }

        //If player selects the currently highlighted button, invoke it
        if (state.Value.Buttons.A == ButtonState.Pressed && prevState.Value.Buttons.A == ButtonState.Pressed)
        {
            PauseMenuManager.instance.SelectButton();
        }
        else if (prevState.Value.Buttons.Start == ButtonState.Released && state.Value.Buttons.Start == ButtonState.Pressed)
        {
            PauseMenuManager.instance.ClosePauseMenu();
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
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }

        float horiz = state.Value.ThumbSticks.Left.X;
        float vert = state.Value.ThumbSticks.Left.Y;
        Vector2 direction = horiz * Vector2.right + vert * Vector2.up;

        PlayerController.instance.Move(direction);
    }

    /// <summary>
    /// Runs the fixed input intended while in the menu InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunMainMenuFixedInput()
    {
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }
    }

    /// <summary>
    /// Runs the fixed input intended while in the in-game player Overlay InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunPlayerOverlayFixedInput()
    {
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }
    }

    /// <summary>
    /// Runs the fixed input intended while in the in-game player menu InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunPlayerMenuFixedInput()
    {
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }
    }

    /// <summary>
    /// Runs the fixed input intended while in the gameplay InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunPauseFixedInput()
    {
        if (!CheckControllerConnected() || state == null || prevState == null)
        {
            return;
        }
    }

    private bool CheckControllerConnected()
    {
        if (prevState == null || !prevState.Value.IsConnected)
        {
            GamePadState checkState = GamePad.GetState(PlayerIndex.One);
            if(checkState.IsConnected)
            {
                return true;
            }
            return false;
        }

        return true;
    }

    private void GetControllerState()
    {
        prevState = state;
        state = GamePad.GetState(PlayerIndex.One);
    }
}
