using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LDB = LoadDataBaseEntries;
using LO = LoadObjectives;
using LD = LoadDialogue;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadingFileManager : MonoBehaviour
{
    public GameObject LoadingFilePanel;
    public GameObject LoadingFileInfoPanel;
    private int[] selectedFileIDHolders = { 0, 1, 2, 3 };
    private int selectedFileID = 0;
    public Button[] FileButtons;
    public TextMeshProUGUI[] FileButtonsText;
    public TextMeshProUGUI FileLevel;
    public TextMeshProUGUI FileSector;
    public TextMeshProUGUI FileTimer;

    public PlayerData loadedData;

    // Start is called before the first frame update
    void Start()
    {
        //Check how many of the 4 load buttons we'll need to activate
        int maxcount = 0;
        if (GameManager.SaveFileID >= 4)
            maxcount = 4;
        else
            maxcount = GameManager.SaveFileID;

        //Then activate that amount
        for (int y = 0; y < maxcount; y++)
        {
            FileButtons[y].gameObject.SetActive(true);
        }

        //Update load button texts
        for (int x = 0; x < selectedFileIDHolders.Length; x++)
        {
            FileButtonsText[x].text = "File " + selectedFileIDHolders[x];
        }
        LoadingFilePanel.SetActive(false);
        LoadingFileInfoPanel.SetActive(false);

        loadedData = SaveSystem.LoadPlayer(selectedFileID);

        //Verify that a file was actually loaded 
        if (loadedData != null)
        {
            //Populate additional panel with file info
            FileLevel.text = "Level " + loadedData.PlayerLevel;
            FileSector.text = loadedData.Sector;
            FileTimer.text = GameManager.RetrievePlayTime(loadedData.PlayerTimer);
            //TO DO - populate panel background image with sector image?
        }
        //Otherwise hide text fields
        else
        {
            FileLevel.text = "";
            FileSector.text = "";
            FileTimer.text = "";
        }
    }

    /// <summary>
    /// Changes the currently selected menu item 
    /// </summary>
    /// <param name="vert"></param>
    public void TraverseMenu(float vert)
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

        //Verify that a file was actually loaded 
        loadedData = SaveSystem.LoadPlayer(selectedFileID);
        if (loadedData != null)
        {
            //Populate additional panel with file info
            FileLevel.text = "Level " + loadedData.PlayerLevel;
            FileSector.text = loadedData.Sector;
            FileTimer.text = GameManager.RetrievePlayTime(loadedData.PlayerTimer);
            //TO DO - populate panel background image with sector image?
        }
        //Otherwise hide text fields
        else
        {
            FileLevel.text = "";
            FileSector.text = "";
            FileTimer.text = "";
        }
    }
    public void FileMenuReset(bool visible)
    {
        //Reset starting menu item indexer for load file buttons
        changeButtonToUnselected(FileButtons[selectedFileID]);
        selectedFileID = 0;
        changeButtonToSelected(FileButtons[selectedFileID]);
        LoadingFilePanel.SetActive(visible);
        LoadingFileInfoPanel.SetActive(visible);
    }

    /// <summary>
    /// Selects the currently highlighted button
    /// </summary>
    public void SelectButton()
    {
        if (GameManager.SaveFileID > 0)
        {
            FileButtons[selectedFileID].onClick.Invoke();
        }
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
            GameManager.Sector = SaveSystem.loadedData.Sector;
            GameManager.currentScene = (GameManager.Scenes)SaveSystem.loadedData.Scene;
            GameManager.OneTimeEvents = SaveSystem.loadedData.OneTimeEventsList;
            GameManager.Timer = SaveSystem.loadedData.PlayerTimer;
            GameManager.PlayerLevel = SaveSystem.loadedData.PlayerLevel;

            //Load player's Objectives progress
            LO.PrimaryObjectives = SaveSystem.loadedData.PrimaryObjectives;
            LO.SecondaryObjectives = SaveSystem.loadedData.SecondaryObjectives;
            LO.currentPrimaryObjective = SaveSystem.loadedData.currentPrimaryObjective;

            //Load player's unlocked database entries
            LDB.Logs = SaveSystem.loadedData.DatabaseEntries;

            //Load
            LD.DialogueItems = SaveSystem.loadedData.DialogueItems;
            SceneManager.LoadScene((int)GameManager.currentScene);
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
