using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    #region Attached UI Objects
    [Header("Attached UI Objects")]
    public ButtonElementSetup[] MenuOptionButtons;
    public GameObject[] OverlayPanels;
    private Canvas overlay;
    private bool overlayVisible = true;
    #endregion

    #region Overlay Scripts
    [Header("Overlay Scripts")]
    public DataBaseOverlayManager DBManager;
    public MapOverlayManager MapManager;
    public ObjectivesOverlayManager OOManager;
    public UpgradesOverlayManager UpgradesManager;
    [Space]
    #endregion

    private int activeButtonID;

    public Panels ActivePanel;

    private static OverlayManager _instance;
    public static OverlayManager instance { get { return _instance; } }

    /// <summary>
    /// Enumerator for each of the 4 overlay panels
    /// </summary>
    public enum Panels
    {
        SkillTree,
        Objectives,
        Map,
        Database
    }

    private void Start()
    {
        //Setup singleton
        if (_instance == null || _instance != this)
        {
            _instance = this;
        }

        //Set the start button/panel values
        MenuOptionButtons[(int)Panels.Map].SwapSelect();
        ActivePanel = Panels.Map;

        //Activate the first button/panel
        activeButtonID = (int)ActivePanel;

        //Find the overlay canvas
        overlay = transform.Find("Canvas").GetComponent<Canvas>();
        ToggleOverlayVisibility();
    }

    /// <summary>
    /// Toggles the visiblity of the hud, called from Input Manager
    /// </summary>
    public void ToggleOverlayVisibility()
    {
        //Hide currently active panel
        OverlayPanels[(int)ActivePanel].SetActive(false);
        MenuOptionButtons[activeButtonID].SwapSelect();

        //If haven't recently unlocked lore entry
        if (HUDController.instance.RecentDataBaseEntry == null ||
            HUDController.instance.RecentDataBaseEntry.Length == 0)
        {
            //reset active panel
            ActivePanel = Panels.Map;
            activeButtonID = (int)ActivePanel;

            //Hide currently active panel
            OverlayPanels[(int)ActivePanel].SetActive(true);

            //Toggle overlay visibility
            overlayVisible = !overlayVisible;
            HUDController.instance.visible = !overlayVisible;
            overlay.gameObject.SetActive(overlayVisible);
            HUDController.instance.ToggleHUDVisibility();
        }
        //Otherwise go straight to database overlay
        else
        {
            //update active panel to database overlay
            ActivePanel = Panels.Database;
            activeButtonID = (int)ActivePanel;

            //Hide currently active panel
            OverlayPanels[(int)ActivePanel].SetActive(true);

            //Toggle overlay visibility
            overlayVisible = !overlayVisible;
            overlay.gameObject.SetActive(overlayVisible);

            string entryName = HUDController.instance.RecentDataBaseEntry[0];
            DBManager.OpenSpecificEntry(entryName);

            //Close the popup notification
            HUDController.instance.CloseDataBasePopup();
        }
    }

    /// <summary>
    /// function that handles input for user scrolling through panels
    /// </summary>
    /// <param name="horiz"></param>
    public void NavigateOverlay(float horiz) {
        if (horiz == 0) return;

        //Hide currently active panel
        OverlayPanels[(int)ActivePanel].SetActive(false);
        MenuOptionButtons[activeButtonID].SwapSelect();

        //If user scrolls to the left
        if (horiz > 0) {
            activeButtonID--;
            if (activeButtonID < 0)
            {
                activeButtonID = MenuOptionButtons.Length - 1;
            }
        }
        //If user scrolls to the right
        else if (horiz < 0)
        {
            activeButtonID++;
            if (activeButtonID == MenuOptionButtons.Length)
            {
                activeButtonID = 0;
            }
        }

        //Update and unhide the new active panel
        ActivePanel = (Panels) activeButtonID;
        MenuOptionButtons[activeButtonID].SwapSelect();
        OverlayPanels[(int)ActivePanel].SetActive(true);
    }

    #region RECEIVE INPUT FROM INPUT MANAGER

    #region Left Stick
    /// <summary>
    /// Directs controller left stick flick input to correct overlay panel manager
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ReceiveLeftStickFlickInput(float x, float y)
    {
        switch(ActivePanel)
        {
            case Panels.Database:
                DBManager.NavigateLeftPanel(y);
                break;
        }
    }

    /// <summary>
    /// Directs controller left stick drag input to correct overlay panel manager
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ReceiveLeftStickDragInput(float x, float y)
    {
        switch(ActivePanel)
        {
            case Panels.Map:
                MapManager.MoveCamera(x, y);
                break;
            case Panels.SkillTree:
                UpgradesManager.MoveReticle(x, y);
                break;
        }
    }
    #endregion

    /// <summary>
    /// Directs controller right stick input to correct overlay panel manager
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ReceiveRightStickInput(float x, float y)
    {
        switch(ActivePanel)
        {
            case Panels.Database:
                DBManager.NavigateArticleButtons(x);
                //DBManager.ScrollEntryInfo(y);
                break;
            case Panels.SkillTree:
                break;
        }
    }

    /// <summary>
    /// Directs controller face button input to correct overlay panel manager
    /// </summary>
    /// <param name="buttonName"></param>
    public void ReceiveFaceButtonInput(string buttonName)
    {
        switch (ActivePanel)
        {
            case Panels.Database:
                if (buttonName.Equals("a"))
                {
                    DBManager.SelectLeftPanelItem();
                }
                break;
            case Panels.SkillTree:
                break;
            case Panels.Map:
                break;
        }
    }

    /// <summary>
    /// Directs controller face button input to correct overlay panel manager
    /// </summary>
    /// <param name="buttonName"></param>
    public void ReceiveTriggerInput(bool isRightTrigger)
    {
        switch (ActivePanel)
        {
            case Panels.SkillTree:
                UpgradesManager.SwapParadigm(isRightTrigger);
                break;
            case Panels.Map:
                MapManager.ZoomCamera(isRightTrigger);
                break;
        }
    }
    #endregion
}
