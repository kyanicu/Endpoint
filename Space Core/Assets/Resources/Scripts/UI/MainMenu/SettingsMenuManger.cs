using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenuManger : MonoBehaviour
{
    public enum SettingsOption { MaVS, MuVS, SVS, RV, FSAV }
    private SettingsOption selectedID;
    const int TOTAL_SETTINGS_OPTIONS = 5;
    const float SLIDER_INC_VAL = 5f;
    const int SLIDER_MIN_VAL = -80;

    #region Audio Stuff
    public Slider MasterVolSlider;
    public Image MaVS_Background;
    public Slider MusicVolSlider;
    public Image MuVS_Background;
    public Slider SoundVolSlider;
    public Image SVS_Background;

    public AudioMixer AudioMixer;
    #endregion

    #region Video Stuff
    public TextMeshProUGUI ResolutionValue;
    public TextMeshProUGUI FullScreenActiveValue;
    List<Tuple<int, int>> resolutions = new List<Tuple<int, int>>();
    int selectedRes = 2;
    private bool fullscreenActive;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        //Add possible resolutions
        resolutions.Add(new Tuple<int, int>(1280, 720));
        resolutions.Add(new Tuple<int, int>(1600, 900));
        resolutions.Add(new Tuple<int, int>(1920, 1280));

        //Set starting resolution
        Screen.SetResolution(1920, 1280, true);
        ResolutionValue.text = $"{Screen.currentResolution.width} X {Screen.currentResolution.height}";

        //Get window status
        fullscreenActive = Screen.fullScreen;
        FullScreenActiveValue.text = fullscreenActive ? "On" : "Off";

        //Hide this screen
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        selectedID = 0;
        Tuple<int, int> newRes = resolutions[selectedRes];
        FullScreenActiveValue.text = fullscreenActive ? "On" : "Off";
        ResolutionValue.text = $"{newRes.Item1} X {newRes.Item2}";
        changeButtonToSelected((int)selectedID);
    }

    /// <summary>
    /// Handles user cursor movement between the ui elements 
    /// </summary>
    /// <param name="vert"></param>
    public void TraverseMenu(float vert)
    {
        // Find out the previously selected button.
        int selected = (int)selectedID;

        // Change style of previously selected button to regular.
        changeButtonToUnselected(selected);

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
            selected = TOTAL_SETTINGS_OPTIONS - 1;
        }
        else if (selected == TOTAL_SETTINGS_OPTIONS)
        {
            selected = 0;
        }
        selectedID = (SettingsOption)selected;

        // Change style of newly selected button to selected.
        changeButtonToSelected(selected);
    }

    /// <summary>
    /// Toggles a specific setting item using horizontal movement
    /// </summary>
    /// <param name="horiz"></param>
    public void EditSetting(float horiz)
    {
        float currentVal;
        switch(selectedID)
        {
            //Master Volume Slider
            case SettingsOption.MaVS:
                currentVal = MasterVolSlider.value;
                currentVal += horiz < 0 ? SLIDER_INC_VAL : -SLIDER_INC_VAL;
                SetVolume(currentVal);
                break;

            //Music Volume Slider
            case SettingsOption.MuVS:
                currentVal = MusicVolSlider.value;
                currentVal += horiz < 0 ? SLIDER_INC_VAL : -SLIDER_INC_VAL;
                SetVolume(currentVal);
                break;

            //Sound Volume Slider
            case SettingsOption.SVS:
                currentVal = SoundVolSlider.value;
                currentVal += horiz < 0 ? SLIDER_INC_VAL : -SLIDER_INC_VAL;
                SetVolume(currentVal);
                break;

            //Resolution Value
            case SettingsOption.RV:
                selectedRes += horiz < 0 ? 1 : -1;
                if (selectedRes < 0) selectedRes += resolutions.Count;
                selectedRes = selectedRes % resolutions.Count;
                Tuple<int, int> newRes = resolutions[selectedRes];
                ResolutionValue.text = $"{newRes.Item1} X {newRes.Item2}";
                Screen.SetResolution(newRes.Item1, newRes.Item2, true);
                break;

            //Fullscreen Active Value
            case SettingsOption.FSAV:
                fullscreenActive = !fullscreenActive;
                Screen.fullScreen = fullscreenActive;
                FullScreenActiveValue.text = fullscreenActive ? "On" : "Off";
                break;
        }
    }

    /// <summary>
    /// Updates the volume of a specific mixer
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume(float volume)
    {
        if (volume > 0) volume = 0;
        if (volume < SLIDER_MIN_VAL) volume = SLIDER_MIN_VAL;
        switch (selectedID)
        {
            //Master Volume Slider
            case SettingsOption.MaVS:
                AudioMixer.SetFloat("MasterVol", volume);
                MasterVolSlider.value = volume;
                break;

            //Music Volume Slider
            case SettingsOption.MuVS:
                AudioMixer.SetFloat("MusicVol", volume);
                MusicVolSlider.value = volume;
                break;


            //Sound Volume Slider
            case SettingsOption.SVS:
                AudioMixer.SetFloat("SoundVol", volume);
                SoundVolSlider.value = volume;
                break;
        }
    }

    /// <summary>
    /// Toggles the visibility of the settings menu panel
    /// </summary>
    /// <param name="visible"></param>
    public void SettingsMenuReset(bool visible)
    {
        gameObject.SetActive(visible);
    }

    /// <summary>
    /// Changes a ui element appearance when a cursor moves off of it
    /// </summary>
    /// <param name="option"></param>
    private void changeButtonToUnselected(int option)
    {
        switch(option)
        {
            //Master Volume Slider
            case 0:
                MaVS_Background.color = Color.white;
                break;

            //Music Volume Slider
            case 1:
                MuVS_Background.color = Color.white;
                break;

            //Sound Volume Slider
            case 2:
                SVS_Background.color = Color.white;
                break;

            //Resolution Value
            case 3:
                ResolutionValue.color = Color.white;
                break;

            //Fullscreen Active Value
            case 4:
                FullScreenActiveValue.color = Color.white;
                break;
        }
    }

    /// <summary>
    /// Changes a ui element appearance when a cursor moves off onto it
    /// </summary>
    /// <param name="option"></param>
    private void changeButtonToSelected(int option)
    {
        switch (option)
        {
            //Master Volume Slider
            case 0:
                MaVS_Background.color = Color.yellow;
                break;

            //Music Volume Slider
            case 1:
                MuVS_Background.color = Color.yellow;
                break;

            //Sound Volume Slider
            case 2:
                SVS_Background.color = Color.yellow;
                break;

            //Resolution Value
            case 3:
                ResolutionValue.color = Color.yellow;
                break;

            //Fullscreen Active Value
            case 4:
                FullScreenActiveValue.color = Color.yellow;
                break;
        }
    }
}
