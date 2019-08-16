using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the input of the game through use of enum type and switch statements
/// Based on current InputState, input is handled differently
/// </summary>
public class InputController : MonoBehaviour
{

    /// <summary> enum type used to keep track of how the input from user should be handled </summary>
    private enum InputState { MENU, GAMEPLAY, PAUSE }
    /// <summary> The current state of how input should be handled </summary>
    private InputState currentState;


    // Start is called before the first frame update
    void Start()
    {

        // Initialize state
        currentState = InputState.GAMEPLAY;

    }

    /// <summary>
    /// Runs the frame input intended while in the gameplay InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    private void RunGameplayFrameInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Player.instance.Jump();
        }
    }

    /// <summary>
    /// Runs the frame input intended while in the menu InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    private void RunMenuFrameInput()
    {

    }

    /// <summary>
    /// Runs the frame input intended while in the pause InputState
    /// Called only on an update frame through Update() function
    /// Should not handle anything physics related that does not require use of "Input.Get___Down/Up"
    /// </summary>
    private void RunPauseFrameInput()
    {

    }

    /// <summary>
    /// Runs the fixed input intended while in the gameplay InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    private void RunGameplayFixedInput()
    {
        float horiz = Input.GetAxisRaw("Horizontal");

        Player.instance.Run(horiz);
    }

    /// <summary>
    /// Runs the fixed input intended while in the menu InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    private void RunMenuFixedInput()
    {

    }

    /// <summary>
    /// Runs the fixed input intended while in the gameplay InputState
    /// Called only on an physics tick through FixedUpdate() function
    /// Should only handle things physics related
    /// Never use "Input.Get___Down/Up" in this function as fixed updates may mix it
    /// </summary>
    private void RunPauseFixedInput()
    {

    }

    // FixedUpdate called once per physics tick
    private void FixedUpdate()
    {
        // based on current InputState handle input appropriately
        switch (currentState)
        {
            case (InputState.MENU):
                RunMenuFixedInput();
                break;
            case (InputState.GAMEPLAY):
                RunGameplayFixedInput();
                break;
            case (InputState.PAUSE):
                RunPauseFixedInput();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case (InputState.MENU):
                RunMenuFrameInput();
                break;
            case (InputState.GAMEPLAY):
                RunGameplayFrameInput();
                break;
            case (InputState.PAUSE):
                RunPauseFrameInput();
                break;
            default:
                break;
        }

    }

    private void LateUpdate()
    {

    }
}
