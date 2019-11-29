using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LO = LoadObjectives;

public class ObjectivesOverlayManager : MonoBehaviour
{
    [Header("Primary Objective")]
    public Image PrimaryIcon;
    public TextMeshProUGUI PrimaryHeader;
    public TextMeshProUGUI PrimaryDescription;
    public TextMeshProUGUI PrimarySubDescription;

    [Header("Secondary Objectives")]
    public Image[] SecondaryIcons;
    public TextMeshProUGUI[] SecondaryHeaders;
    public TextMeshProUGUI[] SecondaryDescriptions;
    private Color unCompleted = Color.white;
    private Color completed = Color.cyan;

    [Header("Locational Objectives")]
    public TextMeshProUGUI[] SmallObjectiveNames;
    private enum SecondaryObjectives
    {
        LoreTerminals,
        SaveRooms,
        SecretAreas,
        SpecialEnemies
    }

    /// <summary>
    /// Refresh objective screen upon overlay reload
    /// </summary>
    private void OnEnable()
    {
        //Reset previously hidden objectives
        for (int i = 0; i < 2; i++)
        {
            SecondaryIcons[i].gameObject.SetActive(true);
            SecondaryHeaders[i].gameObject.SetActive(true);
            SecondaryDescriptions[i].gameObject.SetActive(true);
        }

        //Reload all objectives
        loadPrimaryObjective();
        loadSecondaryObjectives();
        loadLocationalObjectives();
    }

    /// <summary>
    /// Loads all primary objective info onto Objectives Overlay
    /// </summary>
    private void loadPrimaryObjective()
    {
        //Only run if primary objective list is populated
        if (LO.PrimaryObjectives.Count > 0)
        {
            //Check that current objective hasn't already been completed
            for(int i = LO.currentPrimaryObjective; i < LO.PrimaryObjectives.Count; i++)
            {
                Tuple<bool, LO.Objective> tempObj = LO.PrimaryObjectives[i];
                if (tempObj.Item1)
                {
                    LO.currentPrimaryObjective++;
                }
                else break;
            }
            Tuple<bool, LO.Objective> currentObjective = LO.PrimaryObjectives[LO.currentPrimaryObjective];
            LO.Objective PrimaryObjective = currentObjective.Item2;
            
            if (PrimaryObjective.IconPath != null)
            {
                PrimaryIcon.sprite = Resources.Load<Sprite>(PrimaryObjective.IconPath);
            }
            PrimaryHeader.text = PrimaryObjective.Name;
            PrimaryDescription.text = PrimaryObjective.Description;
            PrimarySubDescription.text = PrimaryObjective.SubDescription;
        }
    }
    /// <summary>
    /// Loads all locational objective info onto Objectives Overlay
    /// </summary>
    private void loadSecondaryObjectives()
    {
        //Retrieve list of secondary objectives for current location
        List<LO.Objective> secondaries = LO.SecondaryObjectives[GameManager.Section];

        //Only run if secondary objective list is populated
        if (LO.SecondaryObjectives.Count > 0)
        {
            //Loop through the main secondary objectives
            for (int index = 0; index < 2; index++)
            {
                LO.Objective tempObjective = secondaries[index];
                if (tempObjective.Name != "")
                {
                    if (tempObjective.IconPath != null)
                    {
                        SecondaryIcons[index].sprite = Resources.Load<Sprite>(tempObjective.IconPath);
                    }
                    SecondaryHeaders[index].text = tempObjective.Name + tempObjective.CheckCompletion();
                    SecondaryDescriptions[index].text = tempObjective.Description;
                }
                else
                {
                    SecondaryIcons[index].gameObject.SetActive(false);
                    SecondaryHeaders[index].gameObject.SetActive(false);
                    SecondaryDescriptions[index].gameObject.SetActive(false);
                }
            }
        } 
    }

    /// <summary>
    /// Loads all locational objective info onto Objectives Overlay
    /// </summary>
    private void loadLocationalObjectives()
    {
        //Retrieve list of secondary objectives for current location
        List<LO.Objective> secondaries = LO.SecondaryObjectives[GameManager.Section];
        const int OFFSET = 2;
        foreach (SecondaryObjectives objectiveNum in Enum.GetValues(typeof(SecondaryObjectives)))
        {
            int index = (int)objectiveNum + OFFSET;
            LO.Objective tempObjective = secondaries[index];
            string modifier = tempObjective.CheckCompletion();
            SmallObjectiveNames[index - OFFSET].text = tempObjective.Name + modifier;
            SmallObjectiveNames[index - OFFSET].color = modifier.Equals("[DONE]") ? completed : unCompleted;
        }
    }
}
