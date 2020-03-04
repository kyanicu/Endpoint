using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsSerlializer
{
    public float MasterVolSliderValue;
    public float MusicVolSliderValue;
    public float SoundVolSliderValue;
    public int ResolutionID;
    public bool FullScreenActive;

    public SettingsSerlializer(float MasterVal, float MusicVal, float SoundVal, int SelectedRes, bool FSActive)
    {
        MasterVolSliderValue = MasterVal;
        MusicVolSliderValue = MusicVal;
        SoundVolSliderValue = SoundVal;
        ResolutionID = SelectedRes;
        FullScreenActive = FSActive;
    }
}
