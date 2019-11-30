using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro; // TextMesh Pro
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// The enum id for each button in the main menu list
    /// </summary>
    private enum MenuItemID
    {
        ResumeButton,
        NewButton,
        LoadButton,
        SettingsButton,
        ExitButton
    }

    private enum activeScreenName
    {
        MainMenu,
        LoadingFiles,
        Settings
    }
    private activeScreenName activeScreen;

    private const int TOTAL_MENU_ITEMS = 5;
    private MenuItemID selectedID;
    public Button[] MenuButtons;
    public GameObject MainMenuButtonsGroup;

    [SerializeField]
    private Sprite[] MenuButtonImages = { };

    public TextMeshProUGUI TagText;

    Color colorMenuButtonSelectedText = new Color32(0x00, 0x00, 0x00, 255);
    Color colorMenuButtonSelectedImage = new Color32(0xff, 0x9f, 0x0a, 255);
    Color colorMenuButtonUnselectedText = new Color32(0xff, 0xff, 0xff, 255);
    Color colorMenuButtonUnselectedImage = new Color32(0xff, 0xff, 0xff, 255);

    public MainMenuAnimations MainMenuAnims;

    #region Loading Stuff
    public GameObject LoadingFilePanel;
    private int[] selectedFileIDHolders = { 0, 1, 2, 3 };
    private int selectedFileID = 0;
    public Button[] FileButtons;
    public TextMeshProUGUI[] FileButtonsText;
    #endregion

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
        // Set current state to Main Menu.
        InputManager.instance.currentState = InputManager.InputState.MAIN_MENU;

        //Make main menu group the active starting screen
        activeScreen = activeScreenName.MainMenu;

        // Set selected button ID to the resume button.
        selectedID = MenuItemID.ResumeButton;

        // Trigger selection of first menu item.
        TraverseMenu(0);

        // Run the main menu logo's glitching image animation.
        StartCoroutine(MainMenuAnims.AnimationMenuLogoGlitchImage());

        // Run the helper function for animating the tinybits around the logo.
        MainMenuAnims.AnimationTinybitHelper();

        // Run the helper function for animating the text of the tinybits around the logo.
        MainMenuAnims.AnimationTinybitTextHelper();


        //Check how many of the 4 load buttons we'll need to activate
        int maxcount = 0;
        if (GameManager.SaveFileID >= 4)
            maxcount = 4;
        else
            maxcount = GameManager.SaveFileID - 1;

        //Then activate that amount
        for (int y = 0; y < maxcount ; y++)
        {
            FileButtons[y].gameObject.SetActive(true);
        }

        //Update load button texts
        for (int x = 0; x < selectedFileIDHolders.Length; x++)
        {
            FileButtonsText[x].text = "File " + selectedFileIDHolders[x];
        }
        LoadingFilePanel.SetActive(false);
    }

    /// <summary>
    /// Changes the currently selected menu item 
    /// </summary>
    /// <param name="vert"></param>
    public void TraverseMenu(float vert)
    {
        //There are different buttons to navigate if player is on menu button group
        if (activeScreen == activeScreenName.MainMenu)
        {
            // Find out the previously selected button.
            int selected = (int)selectedID;

            // Change style of previously selected button to regular.
            changeButtonToUnselected(MenuButtons[selected]);

            // Based on input vert, change the current selected button ID.
            if (vert > 0)
            {
                selected++;
            }
            else if (vert < 0)
            {
                selected--;
            }
            if (selected < 0)
            {
                selected = TOTAL_MENU_ITEMS - 1;
            }
            else if (selected == TOTAL_MENU_ITEMS)
            {
                selected = 0;
            }
            selectedID = (MenuItemID)selected;

            // Change style of newly selected button to selected.
            changeButtonToSelected(MenuButtons[selected]);

            // Set the tag text based on the selected button.
            changeTag();
        }
        //There are different buttons to navigate if player is on load file button group
        else if (activeScreen == activeScreenName.LoadingFiles)
        {
            // Change style of previously selected button to regular.
            changeButtonToUnselected(FileButtons[selectedFileID]);

            // Based on input vert, change the current selected button ID.
            if (vert > 0 && selectedFileID < GameManager.SaveFileID - 1)
            {
                selectedFileID++;
                
                //If player has gone past the bottom, we need to update the buttons
                if (selectedFileID > selectedFileIDHolders[3])
                {
                    //Increment each button's text's file ID
                    for (int x = 0; x < selectedFileIDHolders.Length; x++)
                    {
                        selectedFileIDHolders[x]++;
                    }
                }
            }
            else if (vert < 0 && selectedFileID > 0)
            {
                selectedFileID--;

                //If player has gone past the top, we need to update the buttons
                if (selectedFileID < selectedFileIDHolders[0])
                {
                    //Decrement each button's text's file ID
                    for (int x = 0; x < selectedFileIDHolders.Length; x++)
                    {
                        selectedFileIDHolders[x]--;
                    }
                }
            }

            //Update file button text now that selectedFileIDHolders values 
            for (int x = 0; x < selectedFileIDHolders.Length; x++)
            {
                FileButtonsText[x].text = "File " + selectedFileIDHolders[x];
            }

            // Change style of newly selected button to selected.
            changeButtonToSelected(FileButtons[selectedFileID]);
        }
        else if (activeScreen == activeScreenName.Settings)
        {
            //TO DO - Settings screen item traversal
        }
    }

    public void InteractionPointerEnterButton(Button thisButton)
    {
        // Find out the previously selected button.
        int selected = (int)selectedID;

        // Change style of previously selected button to regular.
        changeButtonToUnselected(MenuButtons[selected]);

        // Change current selected ID to the button that was just hovered.
        selectedID = (MenuItemID)System.Enum.Parse(typeof(MenuItemID), thisButton.name);
        selected = (int)selectedID;

        // Change style of newly selected button to selected.
        changeButtonToSelected(MenuButtons[selected]);

        // Set the tag text based on the selected button.
        changeTag();
    }

    private void changeButtonToSelected(Button thisButton)
    {
        thisButton.Select();
        thisButton.GetComponent<Image>().sprite = MenuButtonImages[1];
        thisButton.GetComponent<Image>().color = colorMenuButtonSelectedImage;
        thisButton.GetComponentInChildren(typeof(TextMeshProUGUI)).GetComponent<TextMeshProUGUI>().color = colorMenuButtonSelectedText;

        // Animate the side arrows on the button, to selected position.
        Image leftArrow = thisButton.GetComponentsInChildren<Image>()[1];
        Animator leftAnimator = leftArrow.GetComponent<Animator>();
        leftAnimator.Play("LeftArrowIn");

        Image rightArrow = thisButton.GetComponentsInChildren<Image>()[2];
        Animator rightAnimator = rightArrow.GetComponent<Animator>();
        rightAnimator.Play("RightArrowIn");
    }

    private void changeButtonToUnselected(Button thisButton)
    {
        thisButton.GetComponent<Image>().sprite = MenuButtonImages[0];
        thisButton.GetComponent<Image>().color = colorMenuButtonUnselectedImage;
        thisButton.GetComponentInChildren(typeof(TextMeshProUGUI)).GetComponent<TextMeshProUGUI>().color = colorMenuButtonUnselectedText;

        // Animate the side arrows on the button, to unselected position.
        Image leftArrow = thisButton.GetComponentsInChildren<Image>()[1];
        Animator leftAnimator = leftArrow.GetComponent<Animator>();
        leftAnimator.Play("LeftArrowOut");

        Image rightArrow = thisButton.GetComponentsInChildren<Image>()[2];
        Animator rightAnimator = rightArrow.GetComponent<Animator>();
        rightAnimator.Play("RightArrowOut");
    }

    /// <summary>
    /// Updates the text above the menu button group depending on which 
    /// button is currently highlighted
    /// </summary>
    private void changeTag()
    {
        if (selectedID == MenuItemID.ResumeButton)
        {
            TagText.text = "Resume latest game";
        }
        else if (selectedID == MenuItemID.NewButton)
        {
            TagText.text = "Create a new game";
        }
        else if (selectedID == MenuItemID.LoadButton)
        {
            TagText.text = "Load a saved game";
        }
        else if (selectedID == MenuItemID.SettingsButton)
        {
            TagText.text = "Change settings";
        }
        else if (selectedID == MenuItemID.ExitButton)
        {
            TagText.text = "Exit the game";
        }
    }

    /// <summary>
    /// Invoke the currently selected main menu button
    /// </summary>
    public void SelectButton()
    {
        if (activeScreen == activeScreenName.MainMenu)
        {
            MenuButtons[(int)selectedID].onClick.Invoke();
        }
        else if(activeScreen == activeScreenName.LoadingFiles)
        {
            FileButtons[selectedFileID].onClick.Invoke();
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
        InputManager.instance.currentState = InputManager.InputState.GAMEPLAY;
        GameManager.currentScene = GameManager.Scenes.CentralProcessing;
        SceneManager.LoadScene((int)GameManager.currentScene);
    }

    public void ToggleLoadingFileMenu(int screenID)
    {
        //Update active screen to id passed as argument
        activeScreen = (activeScreenName)screenID;

        //Reset starting menu item indexer for main menu buttons
        changeButtonToUnselected(MenuButtons[(int)MenuItemID.LoadButton]);
        selectedID = MenuItemID.ResumeButton;
        changeButtonToSelected(MenuButtons[(int)selectedID]);

        //Reset starting menu item indexer for load file buttons
        changeButtonToUnselected(FileButtons[selectedFileID]);
        selectedFileID = 0;
        changeButtonToSelected(FileButtons[selectedFileID]);

        //Toggle button groups' visiblity depending on active screen
        MainMenuButtonsGroup.SetActive(activeScreen == activeScreenName.MainMenu);
        LoadingFilePanel.SetActive(activeScreen == activeScreenName.LoadingFiles);
        //SettingsPanel.SetActive(activeScreen == activeScreenName.Settings); <-- uncomment when settings done
    }

    /// <summary>
    /// Loads a savefile given its file ID
    /// </summary>
    /// <param name="saveID"></param>
    public void LoadGame(int saveID)
    {
        //If file wasn't loaded from resume, updated the saveID
        if (saveID == -1)
        {
            saveID = selectedFileID;
        }

        //Get file path with specified file ID
        string path = $"{GameManager.FILE_PATH}{saveID}.sav";

        //Only load the file if the file at specified path exists
        if (File.Exists(path))
        {
            SaveSystem.loadedData = SaveSystem.LoadPlayer(saveID);
            GameManager.SaveFileID = saveID;
            GameManager.Section = SaveSystem.loadedData.Location;
            GameManager.currentScene = (GameManager.Scenes)SaveSystem.loadedData.Scene;
            SceneManager.LoadScene((int)GameManager.currentScene);
        }
    }

    /// <summary>
    /// Open the settings panel to adjust the game's settings
    /// </summary>
    public void OpenSettings()
    {
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
