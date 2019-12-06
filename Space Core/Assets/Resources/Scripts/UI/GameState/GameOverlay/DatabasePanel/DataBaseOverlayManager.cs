using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMesh Pro
using System;

using DataEntry = LoadDataBaseEntries.DataEntry;

public class DataBaseOverlayManager : MonoBehaviour
{
    #region Attached UI Objects
    [Header("Attached UI Objects")]
    public TextMeshProUGUI[] CategoryHeaders;
    public ButtonElementSetup[] ArticleButtons;
    public Image EntryImage;
    public TextMeshProUGUI EntryHeader, EntryCategory, EntryInfo;
    public List<Text> ArticleButtonText;
    [Space]
    #endregion

    [Tooltip("Drag prefab into inspector from resources")]
    [Header("From Resources")]
    public GameObject BlankTextInsert;

    private List<TextMeshProUGUI> ActiveLeftSideElements;
    private int selectedTextID = 0;
    private int selectedArticleID = 0;

    private float Y_MODIFIER;
    private float X_MODIFIER;

    private Color selectedColor = Color.yellow;
    private Color deselectedColor = Color.blue;

    private List<string> loadedArticleInfo;

    private Dictionary<string, Tuple<bool, List<TextMeshProUGUI>>> headersList =
        new Dictionary<string, Tuple<bool, List<TextMeshProUGUI>>>();

    private List<Vector3> headerPos = new List<Vector3>();

    // Start is called before the first frame update
    void Awake()
    {
        ActiveLeftSideElements = new List<TextMeshProUGUI>();

        Y_MODIFIER = BlankTextInsert.GetComponent<TextMeshProUGUI>().rectTransform.rect.height + 5;
        X_MODIFIER = 40f;

        //Populate headers list with all category types
        foreach (TextMeshProUGUI header in CategoryHeaders)
        {
            ActiveLeftSideElements.Add(header);
            headersList.Add(header.text.Substring(2), new Tuple<bool, List<TextMeshProUGUI>>(false, new List<TextMeshProUGUI>()));
            headerPos.Add(header.rectTransform.position);
        }
    }

    /// <summary>
    /// Reset some stuff on panel unhide
    /// </summary>
    public void OnEnable()
    {
        selectedTextID = 0;
        selectedArticleID = 0;
        ActiveLeftSideElements[selectedTextID].color = selectedColor;
        ResetLeftPanelData();
        clearRightPanelData();
    }

    /// <summary>
    /// Reset some stuff on panel hide
    /// </summary>
    public void OnDisable()
    {
        ActiveLeftSideElements[selectedTextID].color = deselectedColor;
        ResetLeftPanelData();

        //Populate list of left side items if it's empty
        if (ActiveLeftSideElements.Count == 0)
        {
            //Populate headers list with all category types
            foreach (TextMeshProUGUI header in CategoryHeaders)
            {
                ActiveLeftSideElements.Add(header);
            }
        }
        //Reset text value of all expanded headers
        else if (ActiveLeftSideElements.Count == 6)
        {
            foreach (TextMeshProUGUI header in CategoryHeaders)
            {
                //Check first char to see if expanded
                if(header.text.Substring(0, 1).Equals("-"))
                {
                    header.text = "+ " + header.text.Substring(2);
                }
            }
        }
    }

    /// <summary>
    /// function that handles input for user scrolling through panels
    /// </summary>
    /// <param name="horiz"></param>
    public void NavigateLeftPanel(float vert)
    {
        //If player is actually moving stick, repopulate right panel
        if (vert != 0)
        {
            //Reset the right panel
            clearRightPanelData();

            ActiveLeftSideElements[selectedTextID].color = deselectedColor;

            //If user scrolls to the up
            if (vert > 0)
            {
                selectedTextID--;
                if (selectedTextID < 0)
                {
                    selectedTextID = ActiveLeftSideElements.Count - 1;
                }
            }
            //If user scrolls to the down
            else if (vert < 0)
            {
                selectedTextID++;
                if (selectedTextID == ActiveLeftSideElements.Count)
                {
                    selectedTextID = 0;
                }
            }
            ActiveLeftSideElements[selectedTextID].color = selectedColor;
        }

        //If selected item is not a content header, populate the right side panel with its unlocked info
        if (!isHeader(selectedTextID))
        {
            foreach(ButtonElementSetup button in ArticleButtons)
            {
                button.gameObject.SetActive(false);
            }

            //Retrieve currently selected item's name
            string selectedItemName = ActiveLeftSideElements[selectedTextID].text.Substring(2);

            //Load the entry from Logs with the name we just retrieved
            DataEntry entry = LoadDataBaseEntries.Logs[selectedItemName];

            //Populate the entry image, header, and category
            EntryImage.sprite = Resources.Load<Sprite>(entry.LogImagePath);
            EntryImage.gameObject.SetActive(true);
            EntryHeader.text = entry.LogName;
            EntryCategory.text = entry.LogCategory;

            int index = -1;

            //Dictionary<string, Tuple<string, bool>>
            foreach (KeyValuePair<string, Tuple<string, bool>> article in entry.LogEntries)
            {
                index++;

                //Check that the entry article is visible
                if (article.Value.Item2)
                {
                    loadedArticleInfo.Add(article.Value.Item1);
                    ArticleButtons[index].gameObject.SetActive(true);
                    ArticleButtonText[index].text = article.Key;
                    ArticleButtons[index].GetComponent<Button>().image.color = Color.black;
                    ArticleButtonText[index].color = Color.yellow;
                }
            }
            LoadArticleInfo(0);
            ArticleButtons[selectedArticleID].GetComponent<Button>().image.color = Color.yellow;
            ArticleButtonText[selectedArticleID].color = Color.black;
        }
    }

    /// <summary>
    /// Cycle through article buttons in the right side panel of database overlay
    /// </summary>
    /// <param name="horiz"></param>
    public void NavigateArticleButtons(float horiz)
    {
        if (horiz == 0 || loadedArticleInfo.Count == 0) return;

        ArticleButtons[selectedArticleID].GetComponent<Button>().image.color = Color.black;
        ArticleButtonText[selectedArticleID].color = Color.yellow;

        //If user scrolls to the left
        if (horiz > 0)
        {
            selectedArticleID--;
            if (selectedArticleID < 0)
            {
                selectedArticleID = loadedArticleInfo.Count - 1;
            }
        }
        //If user scrolls to the right
        else if (horiz < 0)
        {
            selectedArticleID++;
            if (selectedArticleID == loadedArticleInfo.Count)
            {
                selectedArticleID = 0;
            }
        }
        
        ArticleButtons[selectedArticleID].GetComponent<Button>().image.color = Color.yellow;
        ArticleButtonText[selectedArticleID].color = Color.black;
        LoadArticleInfo(selectedArticleID);
    }

    /// <summary>
    /// When user presses A
    /// </summary>
    public void SelectLeftPanelItem()
    {
        selectedArticleID = 0;

        //If selected item is a header, hide, unhide content
        if (isHeader(selectedTextID))
        {
            TextMeshProUGUI item = ActiveLeftSideElements[selectedTextID];

            //Retrieve whether or not item has been expanded and its active content
            Tuple<bool, List<TextMeshProUGUI>> content = headersList[item.text.Substring(2)];

            //Already expanded so hide content
            if (content.Item1)
            {
                hideContent(item);
            }
            //Content hidden so expand
            else
            {
                //Rename header to show collapsed
                ActiveLeftSideElements[selectedTextID].text = "- " + item.text.Substring(2);

                //Empty list to be populated with all logs of this header category
                List<DataEntry> listToLoad = new List<DataEntry>();

                //Load all data entries that fall within this category
                foreach (DataEntry de in LoadDataBaseEntries.Logs.Values)
                {
                    //If dataEntry is of same category, add it to list
                    if (de.LogCategory == item.text.Substring(2))
                    {
                        listToLoad.Add(de);
                    }
                }

                //Now that we have a list of the logs for this category, let's create the UI elements
                //and add them to our expanded space
                int spotsToMove = 0;
                foreach (DataEntry de in listToLoad)
                {
                    if (de.Visible)
                    {
                        //Instantiate our newest text element
                        GameObject newElement = Instantiate(BlankTextInsert, item.rectTransform);
                        newElement.transform.parent = newElement.transform.parent.parent;
                        TextMeshProUGUI newElementTxt = newElement.GetComponent<TextMeshProUGUI>();
                        newElementTxt.text = "> " + de.LogName;
                        Vector3 pos = newElementTxt.rectTransform.position;
                        newElementTxt.rectTransform.position = new Vector3(pos.x + X_MODIFIER, pos.y - (Y_MODIFIER * spotsToMove), pos.z);

                        //Update count of newly add entries and add new element to active elements list
                        spotsToMove++;
                        ActiveLeftSideElements.Insert(selectedTextID + spotsToMove, newElementTxt);

                    }
                }
                //Update the positions of the rest of the list items
                moveContent(selectedTextID + spotsToMove + 1, spotsToMove, false);

                //Update the header's bool to represent that is now expanded
                content = new Tuple<bool, List<TextMeshProUGUI>>(true, content.Item2);
                headersList[item.text.Substring(2)] = content;
            }
        }
    }

    /// <summary>
    /// Loads a specific article's data entry
    /// </summary>
    /// <param name="buttonID"></param>
    public void LoadArticleInfo(int buttonID)
    {
        EntryInfo.text = loadedArticleInfo[buttonID];
    }
    /// <summary>
    /// Hides an active left side panel element
    /// </summary>
    /// <param name="item"></param>
    private void hideContent(TextMeshProUGUI item)
    {
        Tuple<bool, List<TextMeshProUGUI>> content = headersList[item.text.Substring(2)];

        //Index of the first item to be deleted
        int index = selectedTextID + 1;

        //Rename header to show expanded
        ActiveLeftSideElements[selectedTextID].text = "+ " + item.text.Substring(2);

        int newIndex = index;

        //Number of items to be deleted
        int spaceCounter = 0;

        //Loop through left side content to find next header
        while (!isHeader(newIndex + spaceCounter))
        {
            //A check case if closing the bottom most element
            if (index + spaceCounter == ActiveLeftSideElements.Count)
            {
                break;
            }
            //Remove the element from the side panel
            Destroy(ActiveLeftSideElements[index]);
            ActiveLeftSideElements.RemoveAt(index);
            spaceCounter++;
            newIndex--;
        }
        moveContent(index, spaceCounter, true);

        //Update the header's bool to represent that is now hidden
        content = new Tuple<bool, List<TextMeshProUGUI>>(false, content.Item2);
        headersList[item.text.Substring(2)] = content;
    }

    public void OpenSpecificEntry(string entryName)
    {
        #region Reset Entire Overlay
        selectedTextID = 0;
        selectedArticleID = 0;
        ActiveLeftSideElements[selectedTextID].color = selectedColor;
        ResetLeftPanelData();
        clearRightPanelData();
        #endregion

        //Deselect primary active element
        ActiveLeftSideElements[selectedTextID].color = deselectedColor;

        //Search through each log entry for specified entry
        foreach (KeyValuePair<string, DataEntry> entry in LoadDataBaseEntries.Logs)
        {
            DataEntry de = entry.Value;

            //If the category matches, set active ID to the header's ID
            if (de.LogName.Equals(entryName))
            {
                selectedTextID = findItem(de.LogCategory);
                break;
            }
        }

        //Select the left panel item at the ID we want
        SelectLeftPanelItem();
        selectedTextID = findItem(entryName);

        //Reload right hand panel
        NavigateLeftPanel(0);

        //Select new lore entry
        ActiveLeftSideElements[selectedTextID].color = selectedColor;
    }

    /// <summary>
    /// Moves all content based on which header was expanded/collapsed
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="distance"></param>
    private void moveContent(int startIndex, int distance, bool expand)
    {
        //Check for game breaking arguments passed in
        if (distance == 0 || startIndex >= ActiveLeftSideElements.Count) return;

        //Get measurement of how far each item is moving
        float distanceMod = distance * Y_MODIFIER;

        //If menu is collapsing, make sure distance is negative
        if (!expand)
        {
            distanceMod *= -1;
        }

        //Loop through each item after the index in the list and adjust item position
        for (int i = startIndex; i < ActiveLeftSideElements.Count; i++)
        {
            Transform textTransform = ActiveLeftSideElements[i].rectTransform;
            textTransform.position = new Vector2(textTransform.position.x, textTransform.position.y + distanceMod);
        }
    }

    /// <summary>
    /// Clears the right side panel when player moves off of left side item
    /// </summary>
    private void clearRightPanelData()
    {
        EntryImage.gameObject.SetActive(false);
        EntryHeader.text = "";
        EntryCategory.text = ""; 
        EntryInfo.text = "";
        loadedArticleInfo = new List<string>();
        foreach (ButtonElementSetup button in ArticleButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Resets the left side Panel
    /// </summary>
    private void ResetLeftPanelData()
    {
        //If only headers (unedited list)
        if (ActiveLeftSideElements.Count == 6)
        {
            Dictionary<string, Tuple<bool, List<TextMeshProUGUI>>> tempList =
           new Dictionary<string, Tuple<bool, List<TextMeshProUGUI>>>();
            //Search through each log entry for specified entry
            foreach (KeyValuePair<string, Tuple<bool, List<TextMeshProUGUI>>> headerItem in headersList)
            {
                Tuple<bool, List<TextMeshProUGUI>> headerContent = headerItem.Value;

                //If header is currently open
                if(headerContent.Item1)
                {
                    //Create new Tuple which now says header is closed
                    headerContent = new Tuple<bool, List<TextMeshProUGUI>>(false, headerContent.Item2);

                    //Update value with new content
                    tempList[headerItem.Key] = headerContent;
                }
            }

            //Update headerlist if adjustments were made
            if (tempList.Count > 0)
            {
                headersList = tempList;
            }
        }
        if (ActiveLeftSideElements.Count > 6)
        {
            ActiveLeftSideElements[selectedTextID].color = deselectedColor;

            //Loop through all left side elements
            for (int i = 0; i < ActiveLeftSideElements.Count; i++)
            {
                //If it's not a header, destroy it
                if (!isHeader(i))
                {
                    //Check that element hasn't already been destroyed
                    if (ActiveLeftSideElements[i] != null)
                    {
                        Destroy(ActiveLeftSideElements[i].gameObject);
                    }
                }
                else
                {
                    Tuple<bool, List<TextMeshProUGUI>> content = headersList[ActiveLeftSideElements[i].text.Substring(2)];

                    //Otherwise collapse the header if it's expanded
                    if (content.Item1)
                    {
                        content = new Tuple<bool, List<TextMeshProUGUI>>(false, content.Item2);
                        headersList[ActiveLeftSideElements[i].text.Substring(2)] = content;
                        ActiveLeftSideElements[i].text = "+ " + ActiveLeftSideElements[i].text.Substring(2);
                    }
                }
            }

            //Reposition all the header elements
            for (int i = 0; i < CategoryHeaders.Length; i++)
            {
                CategoryHeaders[i].rectTransform.position = headerPos[i];
            }

            //Reset the element list
            ActiveLeftSideElements.Clear();
        }
    }

    /// <summary>
    /// Given an index in the active element list, returns whether or not it is a header element
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool isHeader(int index)
    {
        if (index >= ActiveLeftSideElements.Count) return false;

        TextMeshProUGUI txt = ActiveLeftSideElements[index];
        //Parse text to see if it matches any category header
        foreach (string headerName in headersList.Keys)
        {
            string category = txt.text.Substring(2);
            if (category.IndexOf(headerName) != -1)
            {
                //The text element is a category header
                return true;
            }
        }
        //The text element is not a category header
        return false;
    }

    /// <summary>
    /// Retrieves index of an item in the Left Side Panel
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private int findItem(string name)
    {
        int count = -1;
        foreach(TextMeshProUGUI item in ActiveLeftSideElements)
        {
            if(item.text.Substring(2).Equals(name))
            {
                return count + 1;
            }
            count++;
        }
        return count;
    }
}
