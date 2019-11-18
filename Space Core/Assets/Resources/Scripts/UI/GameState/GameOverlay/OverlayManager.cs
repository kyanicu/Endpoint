using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    public ButtonInfo[] MenuOptionButtons;
    public GameObject[] OverlayPanels;
    private Canvas overlay;
    private bool overlayVisible = true;

    private int activeButtonID;

    public Panels ActivePanel;

    private static OverlayManager _instance = null;
    public static OverlayManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<OverlayManager>();
                // fallback, might not be necessary.
                if (_instance == null)
                    _instance = new GameObject(typeof(OverlayManager).Name).AddComponent<OverlayManager>();

                // This breaks scene reloading
                // DontDestroyOnLoad(m_Instance.gameObject);
            }
            return _instance;
        }
    }

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
        overlayVisible = !overlayVisible;
        overlay.gameObject.SetActive(overlayVisible);
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
}
