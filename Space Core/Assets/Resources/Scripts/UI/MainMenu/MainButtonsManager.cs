using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainButtonsManager : MonoBehaviour
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

    private const int TOTAL_MENU_ITEMS = 5;
    private MenuItemID selectedID;
    public Button[] MenuButtons;
    public GameObject MainMenuButtonsGroup;
    public TextMeshProUGUI TagText;

    // Start is called before the first frame update
    void Start()
    {
        // Set selected button ID to the resume button.
        selectedID = MenuItemID.ResumeButton;
    }

    /// <summary>
    /// Changes the currently selected menu item 
    /// </summary>
    /// <param name="vert"></param>
    public void TraverseMenu(float vert)
<<<<<<< HEAD
    {
        // Find out the previously selected button.
=======
    {// Find out the previously selected button.
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
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

    /// <summary>
    /// Selects the currently highlighted button
    /// </summary>
    public void SelectButton()
    {
<<<<<<< HEAD
        if (selectedID == MenuItemID.SettingsButton)
            TagText.gameObject.SetActive(false);
=======
>>>>>>> 2f6d9b00abb4d75f634655ee7111d4f1c2f6abd2
        MenuButtons[(int)selectedID].onClick.Invoke();
    }

    /// <summary>
    /// Resets visiblity of this manager's buttons group
    /// </summary>
    /// <param name="visible"></param>
    public void MainButtonsMenuReset(bool visible)
    {
        if (!visible)
        {
            changeButtonToUnselected(MenuButtons[(int)selectedID]);
        }
        //Reset starting menu item indexer for load file buttons
        changeButtonToUnselected(MenuButtons[(int)MenuItemID.LoadButton]);
        selectedID = MenuItemID.ResumeButton;
        changeButtonToSelected(MenuButtons[(int)selectedID]);
        MainMenuButtonsGroup.SetActive(visible);
    }

    /// <summary>
    /// When mouse enters button, update animations
    /// </summary>
    /// <param name="thisButton"></param>
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

    private void changeButtonToSelected(Button thisButton)
    {
        thisButton.Select();
        thisButton.GetComponent<Image>().sprite = MainMenuManager.instance.MenuButtonImages[1];
        thisButton.GetComponent<Image>().color = MainMenuManager.instance.colorMenuButtonSelectedImage;
        thisButton.GetComponentInChildren(typeof(TextMeshProUGUI)).GetComponent<TextMeshProUGUI>().color =
            MainMenuManager.instance.colorMenuButtonSelectedText;

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
        thisButton.GetComponent<Image>().sprite = MainMenuManager.instance.MenuButtonImages[0];
        thisButton.GetComponent<Image>().color = MainMenuManager.instance.colorMenuButtonUnselectedImage;
        thisButton.GetComponentInChildren(typeof(TextMeshProUGUI)).GetComponent<TextMeshProUGUI>().color =
            MainMenuManager.instance.colorMenuButtonUnselectedText;

        // Animate the side arrows on the button, to unselected position.
        Image leftArrow = thisButton.GetComponentsInChildren<Image>()[1];
        Animator leftAnimator = leftArrow.GetComponent<Animator>();
        leftAnimator.Play("LeftArrowOut");

        Image rightArrow = thisButton.GetComponentsInChildren<Image>()[2];
        Animator rightAnimator = rightArrow.GetComponent<Animator>();
        rightAnimator.Play("RightArrowOut");
    }
}
