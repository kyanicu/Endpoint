using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class InputManager : MonoBehaviour
{
    /// <summary> enum type used to keep track of how the input from user should be handled </summary>
<<<<<<< HEAD:Space Core/Assets/Resources/Scripts/Input/InputManager.cs
    public enum InputState { MAIN_MENU, OVERLAY, PLAYER_MENU, GAMEPLAY, PAUSE, LOADING, GAME_OVER, TERMINAL_WINDOW, HACKING }
=======
    public enum InputState { MAIN_MENU, OVERLAY, PLAYER_MENU, GAMEPLAY, PAUSE, LOADING }
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2:Space Core/Assets/Resources/Scripts/InputManager.cs
    /// <summary> The current state of how input should be handled </summary>
    public InputState currentState { get; set; }

    private static InputManager _instance;
    public static InputManager instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null || _instance != this)
        {
            _instance = this;
        }
    }

    protected abstract void RunGameplayFrameInput();
    protected abstract void RunMainMenuFrameInput();
    protected abstract void RunPlayerOverlayFrameInput();
    protected abstract void RunPlayerMenuFrameInput();
    protected abstract void RunPauseFrameInput();
    protected abstract void RunGameOverFrameInput();
    protected abstract void RunTerminalWindowFrameInput();
    protected abstract void RunMainMenuFixedInput();
    protected abstract void RunPlayerOverlayFixedInput();
    protected abstract void RunPlayerMenuFixedInput();
    protected abstract void RunGameplayFixedInput();
    protected abstract void RunPauseFixedInput();
    public abstract QTEButton.KeyNames? CheckQTEButtonPress();

    protected void FixedUpdate()
    {
        // based on current InputState handle input appropriately
        switch (currentState)
        {
            case (InputState.MAIN_MENU):
                RunMainMenuFixedInput();
                break;
            case (InputState.OVERLAY):
                RunPlayerOverlayFixedInput();
                break;
            case (InputState.PLAYER_MENU):
                RunPlayerMenuFixedInput();
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
    protected void Update()
    {
        switch (currentState)
        {
            case (InputState.MAIN_MENU):
                RunMainMenuFrameInput();
                break;
            case (InputState.OVERLAY):
                RunPlayerOverlayFrameInput();
                break;
            case (InputState.PLAYER_MENU):
                RunPlayerMenuFrameInput();
                break;
            case (InputState.GAMEPLAY):
                RunGameplayFrameInput();
                break;
            case (InputState.PAUSE):
                RunPauseFrameInput();
                break;
            case (InputState.GAME_OVER):
                RunGameOverFrameInput();
                break;
            case (InputState.TERMINAL_WINDOW):
                RunTerminalWindowFrameInput();
                break;
            default:
                break;
        }

    }

    void LateUpdate()
    {

    }
}
