using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro; // TextMesh Pro
using System.IO;
using LDB = LoadDataBaseEntries;
using LO = LoadObjectives;
using LD = LoadDialogue;

public class MainMenuManager : MonoBehaviour
{
    private enum activeScreenName
    {
        MainMenu,
        LoadingFiles,
        Settings
    }
    private activeScreenName activeScreen;

    public Sprite[] MenuButtonImages = { };

    public Color colorMenuButtonSelectedText = new Color32(0x00, 0x00, 0x00, 255);
    public Color colorMenuButtonSelectedImage = new Color32(0xff, 0x9f, 0x0a, 255);
    public Color colorMenuButtonUnselectedText = new Color32(0xff, 0xff, 0xff, 255);
    public Color colorMenuButtonUnselectedImage = new Color32(0xff, 0xff, 0xff, 255);

    public MainMenuAnimations MainMenuAnims;

    public LoadingFileManager FileManager;
    public MainButtonsManager MainButtonsManager;

    private static MainMenuManager _instance;
    public static MainMenuManager instance { get { return _instance; } }

    private void Awake()
    {
         _instance = this;

        // Set current state to Main Menu.
        InputManager.instance.currentState = InputManager.InputState.MAIN_MENU;

        //Make main menu group the active starting screen
        activeScreen = activeScreenName.MainMenu;
    }

    private void Start()
    {
        // Trigger selection of first menu item.
        TraverseMenu(0);

        // Run the main menu logo's glitching image animation.
        StartCoroutine(MainMenuAnims.AnimationMenuLogoGlitchImage());

        // Run the helper function for animating the tinybits around the logo.
        MainMenuAnims.AnimationTinybitHelper();

        // Run the helper function for animating the text of the tinybits around the logo.
        MainMenuAnims.AnimationTinybitTextHelper();
    }

    /// <summary>
    /// Changes the currently selected menu item 
    /// </summary>
    /// <param name="vert"></param>
    public void TraverseMenu(float vert)
    {
        AudioManager.instance.PlaySound(AudioManager.Clips.MenuScroll);
        //There are different buttons to navigate if player is on menu button group
        if (activeScreen == activeScreenName.MainMenu)
        {
            MainButtonsManager.TraverseMenu(vert);
        }
        //There are different buttons to navigate if player is on load file button group
        else if (activeScreen == activeScreenName.LoadingFiles)
        {
            FileManager.TraverseMenu(vert);
        }
        else if (activeScreen == activeScreenName.Settings)
        {
            //TO DO - Settings screen item traversal
        }
    }

    /// <summary>
    /// Invoke the currently selected main menu button
    /// </summary>
    public void SelectButton()
    {
        AudioManager.instance.PlaySound(AudioManager.Clips.MenuSelect);
        if (activeScreen == activeScreenName.MainMenu)
        {
            MainButtonsManager.SelectButton();
        }
        else if(activeScreen == activeScreenName.LoadingFiles)
        {
            FileManager.SelectButton();
        }
        else if(activeScreen == activeScreenName.Settings)
        {
            //TO DO - Implements settings functionality
        }
    }

    /// <summary>
    /// Called when player presses B returning them to main menu button group
    /// </summary>
    public void ReturnToMainMenu()
    {
        if (activeScreen == activeScreenName.LoadingFiles ||
            activeScreen == activeScreenName.Settings)
        {
            AudioManager.instance.PlaySound(AudioManager.Clips.Back);
            ToggleLoadingFileMenu((int)activeScreenName.MainMenu);
        }
    }

    /// <summary>
    /// Resume the most recently saved game
    /// </summary>
    public void ResumeGame()
    {
        //If there is a game to resume
        if (GameManager.SaveFileID > 0)
        {
            //Load the most previously saved file
            LoadGame(GameManager.SaveFileID - 1);
        }
    }

    /// <summary>
    /// Takes the player to the first level of the game
    /// </summary>
    public void StartNewGame()
    {
        GameManager.Initialized = false;
        SaveSystem.loadedData = null;
        InputManager.instance.currentState = InputManager.InputState.GAMEPLAY;
        GameManager.currentScene = GameManager.Scenes.CentralProcessing;
        SceneManager.LoadScene((int)GameManager.currentScene);

    }

    public void ToggleLoadingFileMenu(int screenID)
    {
        //Update active screen to id passed as argument
        activeScreen = (activeScreenName)screenID;

        //Toggle each button groups' visiblity depending on active screen
        MainButtonsManager.MainButtonsMenuReset(activeScreen == activeScreenName.MainMenu);
        FileManager.FileMenuReset(activeScreen == activeScreenName.LoadingFiles);
        //SettingsManager.SettingsMenuReset(activeScreen == activeScreenName.Settings); <-- uncomment when settings done
    }

    /// <summary>
    /// Loads a savefile given its file ID
    /// </summary>
    /// <param name="saveID"></param>
    public void LoadGame(int saveID)
    {
        FileManager.LoadGame(saveID);
    }

    /// <summary>
    /// Open the settings panel to adjust the game's settings
    /// </summary>
    public void OpenSettings()
    {
        AudioManager.instance.PlaySound(AudioManager.Clips.Deny);
        //ToggleLoadingFileMenu((int)activeScreenName.Settings); <-- uncomment when settings done
    }

    /// <summary>
    /// Closes the game window
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
