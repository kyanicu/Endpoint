using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    #region Attached UI Objects
    [Header("Attached UI Objects")]
    public ButtonElementSetup[] MenuOptionButtons;
    public GameObject OverlayPanelsContainer;
    public GameObject[] OverlayPanels;
    public Canvas overlay;
    private bool overlayVisible = true;
    [Space]
    #endregion

    #region Overlay Manager Scripts
    [Header("Overlay Scripts")]
    public DataBaseOverlayManager DBManager;
    public MapOverlayManager MapManager;
    public ObjectivesOverlayManager OOManager;
    public UpgradesOverlayManager UpgradesManager;
    public TopPanelManager TopPM;
    [Space]
    #endregion

    #region Overlay BG Animation Objects
    [Header("Overlay BG Animation Objects")]
    public Canvas OverlayCanvas;
    public GameObject OverlayBGAnimPanel, DarkBGPanel, FrostedGlassBlurGameplay;
    public Image[] OverlayBGAnimBlocks;
    #endregion

    #region Overlay Top Panel Objects
    [Header("Overlay Top Panel Objects")]
    public GameObject OverlayTopPanel;
    public Image TopPanelBGImage, TopPanelSymbiosLogo, TopPanelC1, TopPanelC2, TopPanelC3, TopPanelC4;
    public TextMeshProUGUI TopPanelClock, TopPanelProcessesText;
    #endregion

    #region Overlay Bottom Panel Objects
    [Header("Overlay Bottom Panel Objects")]
    public GameObject OverlayBottomPanel;
    public Image BottomPanelBG;
    #endregion

    #region Overlay Nav Panel Objects
    [Header("Overlay Nav Panel Objects")]
    public GameObject NavPanel;
    public Image NavPanelMapButton, NavPanelObjectivesButton, NavPanelUpgradesButton, NavPanelDatabaseButton;
    #endregion

    private int activeButtonID;
    public Panels ActivePanel;

    private OverlayAnimations OverlayAnims;

    private static OverlayManager _instance;
    public static OverlayManager instance { get { return _instance; } }

    /// <summary>
    /// Enumerator for each of the 4 overlay panels
    /// </summary>
    public enum Panels
    {
        Map,
        Objectives,
        SkillTree,
        Database
    }

    private void Start()
    {
        //Setup singleton
        if (_instance == null || _instance != this)
        {
            _instance = this;
        }

        OverlayAnims = OverlayAnimations.instance;
        OverlayAnims.OverlayAnimationsInit();

        // Find the overlay canvas and trigger it.
        overlay = transform.Find("OverlayCanvas").GetComponent<Canvas>();
        ToggleOverlayVisibility();

        //Set the start button/panel values
        MenuOptionButtons[(int)Panels.Map].SwapSelect();
        ActivePanel = Panels.Map;

        //Activate the first button/panel
        activeButtonID = (int)ActivePanel;

        // Update experience system
        UpdatePlayerLevelUpgrades(ExperienceSystem.instance.level);
    }

    // Stores coroutine for opening/closing overlay.
    private Coroutine OverlayOpenClose;
    // Stores tween for hiding or showing active panels container.
    private Tween OverlayPanelsContainerOpenClose;

    /// <summary>
    /// Toggles the visiblity of the hud, called from Input Manager
    /// </summary>
    public void ToggleOverlayVisibility()
    {
        // Hide the currently active overlay panel, whether overlay is visible or not.
        OverlayPanels[(int)ActivePanel].SetActive(false);
        OverlayPanelsContainerOpenClose = OverlayPanelsContainer.GetComponent<CanvasGroup>().DOFade(0f, 0.2f);

        MenuOptionButtons[activeButtonID].SwapSelect();

        // If the player hasn't recently unlocked lore entry
        if (HUDController.instance.RecentDataBaseEntry == null ||
            HUDController.instance.RecentDataBaseEntry.Length == 0)
        {
            // Reset the active panel to the default (map).
            ActivePanel = Panels.Map;
            activeButtonID = (int)ActivePanel;

            // Toggle overlay visibility.
            overlayVisible = !overlayVisible;

            // If it is not visible, open everything, including the newly reset active panel.
            if (overlayVisible)
            {
                OverlayOpenClose = StartCoroutine(OverlayAnims.OpenOverlayAnimation());

                // Enable and open the new active panel.
                OverlayPanels[(int)ActivePanel].SetActive(true);
                OverlayPanelsContainerOpenClose = OverlayPanelsContainer.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
                OverlayAnims.NavigateOpenAnimation((int)ActivePanel);
                
            }
            // If it is visible, close everything.
            else
            {
                OverlayManager.instance.FrostedGlassBlurGameplay.GetComponent<Image>();
                OverlayOpenClose = StartCoroutine(OverlayAnims.CloseOverlayAnimation());

                // Close and disable the currently active panel.
                // Enable and open the new active panel.
            }

            HUDController.instance.visible = !overlayVisible;
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

            // If it is not visible, open everything, including the newly reset active panel.
            if (overlayVisible)
            {
                OverlayOpenClose = StartCoroutine(OverlayAnims.OpenOverlayAnimation());

                // Enable and open the new active panel.
                OverlayPanels[(int)ActivePanel].SetActive(true);
                OverlayPanelsContainerOpenClose = OverlayPanelsContainer.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
                OverlayAnims.NavigateOpenAnimation((int)ActivePanel);

            }

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

        //OverlayPanels[(int)ActivePanel].SetActive(false);
        MenuOptionButtons[activeButtonID].SwapSelect();

        //If user scrolls to the left
        if (horiz > 0)
        {
            // Hide currently active panel, play the "close to right" animation
            OverlayAnims.NavigatePanelCloseRightAnimation((int)ActivePanel);

            activeButtonID--;
            if (activeButtonID < 0)
            {
                activeButtonID = MenuOptionButtons.Length - 1;
            }

            // Update the new active panel.
            ActivePanel = (Panels)activeButtonID;
            MenuOptionButtons[activeButtonID].SwapSelect();

            // Make sure the new panel is set to active.
            OverlayPanels[(int)ActivePanel].SetActive(true);

            // Play opening animation for next panel.
            OverlayAnims.NavigatePanelOpenRightAnimation((int)ActivePanel);
        }
        //If user scrolls to the right
        else if (horiz < 0)
        {
            // Hide currently active panel, play the "close to left" animation
            OverlayAnims.NavigatePanelCloseLeftAnimation((int)ActivePanel);

            activeButtonID++;
            if (activeButtonID == MenuOptionButtons.Length)
            {
                activeButtonID = 0;
            }

            // Update the new active panel.
            ActivePanel = (Panels)activeButtonID;
            MenuOptionButtons[activeButtonID].SwapSelect();

            // Make sure the new panel is set to active.
            OverlayPanels[(int)ActivePanel].SetActive(true);

            // Play opening animation for next panel.
            OverlayAnims.NavigatePanelOpenLeftAnimation((int)ActivePanel);
        }
    }

    public void UpdatePlayerLevelUpgrades(int level)
    {
        UpgradesManager.SetLevel(level);
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
                if (buttonName.Equals("a"))
                {
                    UpgradesManager.EquipNewParadigm();
                }
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
