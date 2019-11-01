using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// Used to load specific scenes based on add order in build settings
    /// </summary>
    private enum Scenes
    {
        MainMenu,
        DemoScene
    }

    /// <summary>
    /// The enum id for each button in the main menu list
    /// </summary>
    private enum MenuItemID
    {
        Resume,
        New,
        Load,
        Settings,
        Exit
    }

    private const int TOTAL_MENU_ITEMS = 5;
    private MenuItemID selectedID;
    public Button[] MenuButtons;

    private Color deselectedColor = Color.white;
    private Color selectedColor = Color.yellow;


    private static MainMenuManager _instance = null;
    public static MainMenuManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainMenuManager>();
                // fallback, might not be necessary.
                if (_instance == null)
                    _instance = new GameObject(typeof(MainMenuManager).Name).AddComponent<MainMenuManager>();

                // This breaks scene reloading
                // DontDestroyOnLoad(m_Instance.gameObject);
            }
            return _instance;
        }
    }

    private void Start()
    {
        InputManager.instance.currentState = InputManager.InputState.MAIN_MENU;
        selectedID = MenuItemID.Resume;
        TraverseMenu(0);
    }

    /// <summary>
    /// Changes the currently selected menu item 
    /// </summary>
    /// <param name="vert"></param>
    public void TraverseMenu(float vert)
    {
        int selected = (int)selectedID;
        MenuButtons[selected].GetComponent<Image>().color = deselectedColor;
        if (vert > 0)
        {
            selected++;
        }
        else if (vert < 0)
        {
            selected--;
        }
        if(selected < 0)
        {
            selected = TOTAL_MENU_ITEMS - 1;
        }
        else if (selected == TOTAL_MENU_ITEMS)
        {
            selected = 0;
        }
        selectedID = (MenuItemID)selected;
        MenuButtons[selected].Select();
        MenuButtons[selected].GetComponent<Image>().color = selectedColor;
    }

    /// <summary>
    /// Invoke the currently selected main menu button
    /// </summary>
    public void SelectButton()
    {
        MenuButtons[(int)selectedID].onClick.Invoke();
    }

    /// <summary>
    /// Resume the most recently saved game
    /// </summary>
    public void ResumeGame()
    {

    }

    /// <summary>
    /// Takes the player to the first level of the game
    /// </summary>
    public void StartNewGame()
    {
        InputManager.instance.currentState = InputManager.InputState.GAMEPLAY;
        SceneManager.LoadScene((int)Scenes.DemoScene);
    }

    public void LoadGame()
    {

    }

    /// <summary>
    /// Open the settings panel to adjust the game's settings
    /// </summary>
    public void OpenSettings()
    {

    }

    /// <summary>
    /// Closes the game window
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
