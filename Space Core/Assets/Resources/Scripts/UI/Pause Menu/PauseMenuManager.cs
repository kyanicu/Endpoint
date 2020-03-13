using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro; // TextMesh Pro

public class PauseMenuManager : MonoBehaviour {

    /// <summary>
    /// The enum id for each button in the main menu list
    /// </summary>
    private enum MenuItemID
    {
        ResumeButton,
        LoadButton,
        SettingsButton,
        ExitGameButton,
        ExitDesktopButton
    }

    private const int TOTAL_MENU_ITEMS = 5;
    private MenuItemID selectedID;
    public Button[] MenuButtons;

    [SerializeField]
    private Sprite[] MenuButtonImages = { };

    public TextMeshProUGUI TagText;

    Color colorMenuButtonSelectedText = new Color32(0x00, 0x00, 0x00, 255);
    Color colorMenuButtonSelectedImage = new Color32(0xff, 0x9f, 0x0a, 255);
    Color colorMenuButtonUnselectedText = new Color32(0xff, 0xff, 0xff, 255);
    Color colorMenuButtonUnselectedImage = new Color32(0xff, 0xff, 0xff, 255);

    public GameObject PauseMenuPanel;
    public bool PauseMenuPanelIsActive { get; set; }

    private static PauseMenuManager _instance = null;
    public static PauseMenuManager instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null || _instance != this)
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set selected button ID to the resume button.
        selectedID = MenuItemID.ResumeButton;

        // Trigger selection of first menu item.
        TraverseMenu(0);

        // Hide this panel.
        PauseMenuPanel.gameObject.SetActive(false);
        this.PauseMenuPanelIsActive = false;
    }

    // This function is called when the pause menu is opened in the game.
    public void OpenPauseMenu()
    {
        // Show this panel.
        PauseMenuPanel.gameObject.SetActive(true);
        this.PauseMenuPanelIsActive = true;

        // Set selected button ID to the resume button.
        selectedID = MenuItemID.ResumeButton;

        // Trigger selection of first menu item.
        TraverseMenu(0);
    }

    // This function is called when the pause menu is closed.
    public void ClosePauseMenu()
    {
        // Hide this panel.
        PauseMenuPanel.gameObject.SetActive(false);
        this.PauseMenuPanelIsActive = false;

        // Set current state to Gameplay.
        InputManager.instance.currentState = InputManager.InputState.GAMEPLAY;
    }

    /// <summary>
    /// Changes the currently selected menu item 
    /// </summary>
    /// <param name="vert"></param>
    public void TraverseMenu(float vert)
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

    private void changeTag()
    {
        if (selectedID == MenuItemID.ResumeButton)
        {
            TagText.text = "Resume the game";
        }
        else if (selectedID == MenuItemID.LoadButton)
        {
            TagText.text = "Load another saved game";
        }
        else if (selectedID == MenuItemID.SettingsButton)
        {
            TagText.text = "Change settings";
        }
        else if (selectedID == MenuItemID.ExitGameButton)
        {
            TagText.text = "Exit this game to main menu";
        }
        else if (selectedID == MenuItemID.ExitDesktopButton)
        {
            TagText.text = "Exit this game to desktop";
        }
    }
    public void SelectButton()
    {
        MenuButtons[(int)selectedID].onClick.Invoke();
    }

    public void ResumeGame()
    {
        ClosePauseMenu();
    }
    public void LoadGame()
    {
        Debug.Log("clicked load");
    }
    public void OpenSettings()
    {
        Debug.Log("clicked settings");
    }
    public void QuitGameToMenu()
    {
        // Load main menu scene
        InputManager.instance.currentState = InputManager.InputState.MAIN_MENU;
        SceneManager.LoadScene(0);
    }
    public void QuitGameToDesktop()
    {
        Application.Quit();
    }
}
