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
    private bool playerIndexSet = false;
    private GamePadState? state;
    private GamePadState? prevState;

    // Start is called before the first frame update
    void Start()
    {
        state = null;
        prevState = null;
        // Initialize state
        currentState = InputState.GAMEPLAY;

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

        if (state.Value.DPad.Up == ButtonState.Pressed && prevState.Value.DPad.Up == ButtonState.Released)
        {
            return (QTEButton.KeyNames) 0;
        }
        else if (state.Value.DPad.Down == ButtonState.Pressed && prevState.Value.DPad.Down == ButtonState.Released)
        {
            return (QTEButton.KeyNames) 1;
        }
        else if (state.Value.DPad.Left == ButtonState.Pressed && prevState.Value.DPad.Left == ButtonState.Released)
        {
            return (QTEButton.KeyNames) 2;
        }
        else if (state.Value.DPad.Right == ButtonState.Pressed && prevState.Value.DPad.Right == ButtonState.Released)
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
            Player.instance.Fire();
        }

        /*
        if (state.Value.Triggers.Left > 0.3f)
        {
            //no plans yet
        }
        */

        #endregion

        #region ABXY Buttons

        if (state.Value.Buttons.A == ButtonState.Pressed)
        {
            Player.instance.Jump();
        }

        if (state.Value.Buttons.A == ButtonState.Released)
        {
            Player.instance.JumpCancel();
        }

        if (prevState.Value.Buttons.X == ButtonState.Released && state.Value.Buttons.X == ButtonState.Pressed)
        {
            Player.instance.Reload();
        }

        if (prevState.Value.Buttons.B == ButtonState.Released && state.Value.Buttons.B == ButtonState.Pressed)
        {
            Player.instance.HackSelector();
        }

        /*
        if (prevState.Value.Buttons.Y == ButtonState.Released && state.Value.Buttons.Y == ButtonState.Pressed)
        {
            //Have Player Inspect Object/Interact with environment
        }
        */

        #endregion

        #region Control Stick

        if (state.Value.ThumbSticks.Right.X != 0 || state.Value.ThumbSticks.Right.Y != 0)
        {
            float angle = Mathf.Atan2(state.Value.ThumbSticks.Right.Y, state.Value.ThumbSticks.Right.X) * Mathf.Rad2Deg;
            Player.instance.AimWeapon(angle);
        }

        /*
        if (prevState.Value.Buttons.LeftStick == ButtonState.Released && state.Value.Buttons.LeftStick == ButtonState.Pressed)
        {
            //Swap UI Pannels
        }
        */

        /*
        if (prevState.Value.Buttons.RightStick == ButtonState.Released && state.Value.Buttons.RightStick == ButtonState.Pressed)
        {
            //Deselect Hack Target
        }
        */

        #endregion

        #region Guide Buttons

        /*
        if (prevState.Value.Buttons.Start == ButtonState.Released && state.Value.Buttons.Start == ButtonState.Pressed)
        {
            //Pause Game
        }
        */

        /*
        if (prevState.Value.Buttons.Guide == ButtonState.Released && state.Value.Buttons.Guide == ButtonState.Pressed)
        {
            //No plans yet
        }
        */

        #endregion

        #region Shoulder Buttons

        /*
        if (prevState.Value.Buttons.RightShoulder == ButtonState.Released && state.Value.Buttons.RightShoulder == ButtonState.Pressed)
        {
            //Use Ability 2
        }
        */

        /*
        if (prevState.Value.Buttons.LeftShoulder == ButtonState.Released && state.Value.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            //Use Ability 2
        }
        */

        #endregion

        //TODO: Remove me
        if (Input.GetKeyDown(KeyCode.Return))
            Player.instance.transform.position = new Vector2(0, 0);
    }

    /// <summary>
    /// Runs the frame input intended while in the menu InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    protected override void RunMenuFrameInput()
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

        Player.instance.Move(horiz);
    }

    /// <summary>
    /// Runs the fixed input intended while in the menu InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    protected override void RunMenuFixedInput()
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
