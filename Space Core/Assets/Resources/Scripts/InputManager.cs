﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    /// <summary> enum type used to keep track of how the input from user should be handled </summary>
    protected enum InputState { MENU, GAMEPLAY, PAUSE }
    /// <summary> The current state of how input should be handled </summary>
    protected InputState currentState;

    protected abstract void RunGameplayFrameInput();
    protected abstract void RunMenuFrameInput();
    protected abstract void RunPauseFrameInput();
    protected abstract void RunMenuFixedInput();
    protected abstract void RunGameplayFixedInput();
    protected abstract void RunPauseFixedInput();
    public abstract QTEButton.KeyNames? CheckQTEButtonPress();

    protected void FixedUpdate()
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
    protected void Update()
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

    void LateUpdate()
    {

    }

    private static InputManager _instance = null;

    public static InputManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
                // fallback, might not be necessary.
                if (_instance == null)
                    _instance = new GameObject(typeof(InputManager).Name).AddComponent<InputManager>();

                // This breaks scene reloading
                // DontDestroyOnLoad(m_Instance.gameObject);
            }
            return _instance;
        }
    }
}