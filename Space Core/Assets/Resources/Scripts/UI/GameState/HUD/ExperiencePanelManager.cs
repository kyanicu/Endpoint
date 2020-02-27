using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperiencePanelManager : MonoBehaviour
{
    // UI elements depicting experience
    public Image ExperienceBar;
    public TextMeshProUGUI ExperienceLevelLabel;

    /// <summary>
    /// Updates experience bar UI element in Player HUD Canvas.
    /// </summary>
    public void UpdateExperience(int level, int experience, int nextLevelExperience)
    {
        // Tween the experience bar to the upated value.
        ExperienceBar.DOFillAmount((float)experience / (float)nextLevelExperience, 0.02f);
        ExperienceLevelLabel.text = level.ToString();
    }

    public void UpdateExperienceFromSystem()
    {
        ExperienceBar.DOFillAmount((float)ExperienceSystem.instance.experience / (float)ExperienceSystem.instance.nextLevelExperience, 0.02f);
        ExperienceLevelLabel.text = ExperienceSystem.instance.level.ToString();
    }
}
