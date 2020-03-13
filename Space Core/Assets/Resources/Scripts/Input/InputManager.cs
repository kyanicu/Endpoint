﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class InputManager : MonoBehaviour
{
    /// <summary> enum type used to keep track of how the input from user should be handled </summary>
    public enum InputState { MAIN_MENU, OVERLAY, PLAYER_MENU, GAMEPLAY, PAUSE, LOADING, GAME_OVER }
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
            default:
                break;
        }

    }

    void LateUpdate()
    {

    }
}
