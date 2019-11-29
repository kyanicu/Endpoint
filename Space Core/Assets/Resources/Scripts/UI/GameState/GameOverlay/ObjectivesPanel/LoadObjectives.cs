using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class LoadObjectives
{
    //List<Tuple<hasBeenCompleted, Objective>>
    public static List<Tuple<bool, Objective>> PrimaryObjectives = new List<Tuple<bool, Objective>>();

    //Dictionary<Location Name, List<Objectives>>
    public static Dictionary<string, List<Objective>> SecondaryObjectives = new Dictionary<string, List<Objective>>();

    public static bool AllObjectivesLoaded = false;
    public static int currentPrimaryObjective;

    public enum SideObjectiveID
    {
        Terminals,
        SaveRooms,
        SecretAreas,
        SpecialEnemies
    }
    private enum LineID
    {
        Description,
        NumToComplete,
        IconType,
        SubDescription
    };

    public struct Objective
    {
        public string Name;
        public string Description;
        public string SubDescription;
        public Sprite Icon;
        private int CurrentlyCompleted;
        private int NumToComplete;

        /// <summary>
        /// Constructs an objective struct on load
        /// </summary>
        /// <param name="name"></param>
        /// <param name="descr"></param>
        /// <param name="ntc"></param>
        /// <param name="cc"></param>
        public Objective(string name, string descr, string subdescr, int ntc, string iconType)
        {
            Name = name;
            Description = descr;
            SubDescription = subdescr;
            NumToComplete = ntc;
            CurrentlyCompleted = 0;

            //Check if Icon could be correctly loaded
            try
            {
                Icon = Resources.Load<Sprite>("Images/ObjectiveIcons/" + iconType);
            }
            catch (Exception e)
            {
                Debug.LogWarning(iconType + " is not a valid icon type!");
                Icon = null;
            }
        }

        /// <summary>
        /// Increments Objective progress by 1
        /// </summary>
        public void IncrementProgress()
        {
            CurrentlyCompleted++;
        }
        
        /// <summary>
        /// Returns the current objective's progress in a string formatted for description
        /// </summary>
        /// <returns></returns>
        public string CheckCompletion()
        {
            //Returns whether or not objective has been completed
            if (NumToComplete == CurrentlyCompleted)
            {
                return "[DONE]";
            }
            //Ensures primary objectives don't have a counter b/c only one thing to complete
            else if (NumToComplete == 1)
            {
                return "";
            }
            //Returns amount of total completed for objective
            else
            {
                return $"[{CurrentlyCompleted}/{NumToComplete}]";
            }
        }
    }

    /// <summary>
    /// One time process of compiling all objectives from text/image files
    /// </summary>
    public static void LoadAllObjectives()
    {
        //If logs have already been loaded, don't load them again
        if (AllObjectivesLoaded) return;
        currentPrimaryObjective = 0;

        LoadPrimaryObjectives();
        LoadSecondaryObjectives();
    }

    /// <summary>
    /// Retrieves all primary objectives from primary txt files
    /// </summary>
    public static void LoadPrimaryObjectives()
    {
        //Get each text file in the primary objectives folder
        TextAsset[] pObjectivesList = Resources.LoadAll<TextAsset>("Text/Objectives/Primary");

        //Loop through each file in folder
        foreach (TextAsset txt in pObjectivesList)
        {
            Objective newPrimary = LoadObjectiveFromText(txt);
            Tuple<bool, Objective> PrimaryObjective = new Tuple<bool, Objective>(false, newPrimary);
            PrimaryObjectives.Add(PrimaryObjective);
        }
    }

    /// <summary>
    /// Retrieves all secondary objectives from primary txt files
    /// </summary>
    public static void LoadSecondaryObjectives()
    {
        //Get the directionary holding the location folders
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Text/Objectives/Secondary");
        string[] locationFolders = Directory.GetDirectories(dir.FullName);

        //Iterate through each location folder
        foreach (string locationPath in locationFolders)
        {
            //Retrieve location name
            string location = GameManager.instance.PullDirectoryEndPoint(locationPath);

            //Get each text file in the location folder
            TextAsset[] pObjectivesList = Resources.LoadAll<TextAsset>("Text/Objectives/Secondary/" + location);

            List<Objective> secondaryObjectivesList = new List<Objective>();

            //Loop through each file in folder and add objectives to locational objective list
            foreach (TextAsset txt in pObjectivesList)
            {
                Objective newSecondary = LoadObjectiveFromText(txt);
                secondaryObjectivesList.Add(newSecondary);
            }

            //Add the newly created list to dictionary with location name as key
            SecondaryObjectives.Add(location, secondaryObjectivesList);
        }
    }

    /// <summary>
    /// Progresses an objective
    /// </summary>
    /// <param name="objName"></param>
    /// <param name="isPrimary"></param>
    public static void ProgressObjective(string progObjName, bool isPrimary)
    {
        if (isPrimary)
        {
            int counter = 0;
            //Loop through all primaries
            foreach (Tuple<bool, Objective> primary in PrimaryObjectives)
            {
                Objective primaryObj = primary.Item2;

                //If primary is the one to be completed
                if (primaryObj.Name.Equals(progObjName.Substring(2)))
                {
                    //Update its item1 to true confirming that its been completed
                    Tuple<bool, Objective> update = new Tuple<bool, Objective>(true, primary.Item2);
                    PrimaryObjectives[counter] = update;
                    //TODO-Make call to function that animates notification popup
                    break;
                }
                counter++;
            }
        }
        else
        {
            //Retrieve all secondary objectives for this location
            List<Objective> secondaries = SecondaryObjectives[GameManager.Section];
            List<Objective> update = new List<Objective>();

            //Iterate through secondary objectives
            foreach (Objective obj in secondaries)
            {
                //If objective to update is found
                if (obj.Name.Equals(progObjName.Substring(2)))
                {
                    //Progress it
                    obj.IncrementProgress();
                }
                update.Add(obj);
            }

            //Save the updated list
            SecondaryObjectives[GameManager.Section] = update;
        }
    }


    /// <summary>
    /// Reads and loads an objective given a text file
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    private static Objective LoadObjectiveFromText(TextAsset txt)
    {
        //Split text file lines into an array
        string[] fLines = txt.text.Split("\n"[0]);

        //Retrieve Objective info
        string name = txt.name.Substring(2);

        int index = fLines[(int)LineID.Description].IndexOf(": ") + 2;
        string description = fLines[(int)LineID.Description].Substring(index);

        index = fLines[(int)LineID.NumToComplete].IndexOf(": ") + 2;
        string numToConvert = fLines[(int)LineID.NumToComplete].Substring(index);
        numToConvert.Replace("\r", "");
        int numToComplete = int.Parse(numToConvert);

        index = fLines[(int)LineID.IconType].IndexOf(": ") + 2;
        string iconType = fLines[(int)LineID.IconType];

        string subDescription = "";
        for (int x = (int)LineID.SubDescription; x < fLines.Length; x++)
        {
            subDescription += fLines[x] + "\n";
        }

        // public Objective(string name, string descr, string subdescr, int ntc, string iconType, bool iuso)
        return new Objective(name, description, subDescription, numToComplete, iconType);
    }
}
